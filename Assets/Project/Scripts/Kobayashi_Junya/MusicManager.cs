using UnityEngine;
using TMPro;

using BatotteChannel.InGame.Notes;
using BatotteChannel.GameState;
using BatotteChannel.DataAssets;
using BatotteChannel.InGame.Players;

namespace BatotteChannel.InGame.MusicSystem
{
    public enum EInitiativePlayerState
    {
        None,
        One,
        Two
    }

    #region 変数

    /// <summary>リズムゲームパートを管理するクラス AudioSourceとBeatCounterとの併用を想定</summary>
    [RequireComponent(typeof(AudioSource)), RequireComponent(typeof(BeatCounter))]
    public class MusicManager : MonoBehaviour
    {
        /// <summary>
        /// 主導権を握っているプレイヤーのステートマシン
        /// </summary>
        private EInitiativePlayerState _initiativePlayerState;

        /// <summary>
        /// 主導権ステートマシンのゲッタープロパティ
        /// </summary>
        public EInitiativePlayerState InitiativePlayerState { get { return _initiativePlayerState; } }

        /// <summary>
        /// ビート計上クラスのインスタンス
        /// </summary>
        private BeatCounter _beatCounter;

        /// <summary>ビート計上クラスのゲッタープロパティ</summary>
        public BeatCounter BeatCounterInstance
        {
            get { return _beatCounter; }
        }

        [Tooltip("残り時間を表示するオブジェクトを設定"), SerializeField]
        GameObject _remainingTimeManagerObj;

        /// <summary>残り時間を表示するクラスのインスタンス</summary>
        private RemainingTimeManager _remainingTimeManager;

        /// <summary>AudioSourceのインスタンス</summary>
        private AudioSource _audioSource;

        /// <summary>オーディオソースのゲッタープロパティ</summary>
        public AudioSource AudioSource { get { return _audioSource; } }

        [Tooltip("リズムゲームBGMを設定"), SerializeField]
        private AudioClip _audioClip;

        [Tooltip("プレイヤー1のスコアを表示するオブジェクトを設定"), SerializeField]
        private TextMeshProUGUI _p1ScoreTMPro;

        [Tooltip("プレイヤー2のスコアを表示するオブジェクトを設定"), SerializeField]
        private TextMeshProUGUI _p2ScoreTMPro;

        [Tooltip("楽曲のノーツ生成データベースを設定"), SerializeField]
        private GenerateSettingDataBase _generateSettingDataBase;

        [Tooltip("Player1のノーツ管理クラスを設定"), SerializeField]
        private NoteManager _noteManagerP1;

        [Tooltip("Player1のノーツ管理クラスを設定"), SerializeField]
        private NoteManager _noteManagerP2;

        [Tooltip("Player1のスコアを格納するためのデータを設定"), SerializeField]
        private ResultData _resultDataP1;

        [Tooltip("Player1のスコアを格納するためのデータを設定"), SerializeField]
        private ResultData _resultDataP2;

        /// <summary>P1のスコア(主導権を取得した時間)</summary>
        private float _initiativeTimeP1;
        /// <summary>P2のスコア(主導権を取得した時間)</summary>
        private float _initiativeTimeP2;

        /// <summary>リズムゲームパートが実行中か</summary>
        private bool _isPlaying;

        /// <summary>リズムゲームパートが実行中かのゲッタープロパティ</summary>
        public bool IsPlaying
        {
            get { return _isPlaying; }
        }

        /// <summary>リズムゲームパートの終了処理が開始されたか</summary>
        private bool _isEndProcessing = false;

        /// <summary>ノーツ生成インデックス番号を格納</summary>
        int genSetIndex = 0;

        #endregion

        #region イベント関数

        private void Start()
        {
            InisializeMusicManager();
        }

        private void Update()
        {
            if (!_isPlaying) return;
            AdditionInitiativeTime();
            ViewScore();

            if (_remainingTimeManager.IsCompleteCount == true && _isEndProcessing == false)
            {
                EndRhythmGamePart();
            }

            if (genSetIndex == _generateSettingDataBase.generateSettingList.Count) return;
            if (_beatCounter.Beat >= _generateSettingDataBase.generateSettingList[genSetIndex].timing)
            {
                GenerateNotesByTimming();
            }
        }

        #endregion

        #region 関数

        /// <summary>
        /// Start関数で実行される初期化処理
        /// </summary>
        private void InisializeMusicManager()
        {
            // キャッシュ
            _beatCounter = GetComponent<BeatCounter>();
            _audioSource = GetComponent<AudioSource>();
            _remainingTimeManager = _remainingTimeManagerObj.GetComponent<RemainingTimeManager>();

            // 初期設定
            SetAudioClip(_audioClip);
            // 一旦仮で実数値で設定してます
            _remainingTimeManager.SetMusicTime(0, 48);

            // ノーツが取得されたときのコールバックにOnGetNoteを登録
            // 秒数換算に使用します
            _noteManagerP1.getNoteCallBack += OnGetNote;
            _noteManagerP2.getNoteCallBack += OnGetNote;
        }

        /// <summary>
        /// リズムゲームパートの終了処理
        /// </summary>
        private void EndRhythmGamePart()
        {
            _isPlaying = false;
            _isEndProcessing = true;

            // 最終スコアにデータをセット
            // Player1
            _resultDataP1.SetGoodCount(_noteManagerP1.GotGoodCount);
            _resultDataP1.SetMissCount(_noteManagerP1.GotMissCount);
            _resultDataP1.SetInitiativeTime(_initiativeTimeP1);

            // Player2
            _resultDataP2.SetGoodCount(_noteManagerP2.GotGoodCount);
            _resultDataP2.SetMissCount(_noteManagerP2.GotMissCount);
            _resultDataP2.SetInitiativeTime(_initiativeTimeP2);

            InGameStateManager.Instance.SetCurrentInGameState(InGameState.End);
        }

        /// <summary>
        /// ノーツ生成をNoteManagerに指示
        /// </summary>
        private void GenerateNotesByTimming()
        {
            // プレイヤー1か2かを判定し、それに対する生成を行う
            // スタン状態処理はNoteManager側で行っているため、こちらで判定を行う必要はないです
            if (_generateSettingDataBase.generateSettingList[genSetIndex].player == Players.PlayerNumberState.Two)
            {
                Debug.Log("ノーツを生成をプレイヤー2に指示します");
                _noteManagerP2.GenerateNote(_generateSettingDataBase.generateSettingList[genSetIndex].generatePos);
                genSetIndex++;

                return;
            }

            Debug.Log("ノーツを生成をプレイヤー1に指示します");
            _noteManagerP1.GenerateNote(_generateSettingDataBase.generateSettingList[genSetIndex].generatePos);
            genSetIndex++;
        }

        /// <summary>
        /// スコアを表示する
        /// </summary>
        private void ViewScore()
        {
            int roundedInitiativeTimeP1 = Mathf.RoundToInt(_initiativeTimeP1);
            int roundedInitiativeTimeMinuteP1 = Mathf.RoundToInt(roundedInitiativeTimeP1 / 60);
            int roundedInitiativeTimeSecondsP1 = Mathf.RoundToInt(roundedInitiativeTimeP1 % 60);
            _p1ScoreTMPro.text = $"{roundedInitiativeTimeMinuteP1}:{roundedInitiativeTimeSecondsP1.ToString("F0").PadLeft(2, '0')}";

            int roundedInitiativeTimeP2 = Mathf.RoundToInt(_initiativeTimeP2);
            int roundedInitiativeTimeMinuteP2 = Mathf.RoundToInt(roundedInitiativeTimeP2 / 60);
            int roundedInitiativeTimeSecondsP2 = Mathf.RoundToInt(roundedInitiativeTimeP2 % 60);
            _p2ScoreTMPro.text = $"{roundedInitiativeTimeMinuteP2}:{roundedInitiativeTimeSecondsP2.ToString("F0").PadLeft(2, '0')}";

            // ノーツ数で表示
            // _p1ScoreTMPro.text = $"Good:{_noteManagerP1.GotGoodCount}\nMiss:{_noteManagerP1.GotMissCount}";
            // _p2ScoreTMPro.text = $"Good:{_noteManagerP2.GotGoodCount}\nMiss:{_noteManagerP2.GotMissCount}";
        }

        /// <summary>
        /// ノーツ生成がされる状態かのセッター関数
        /// </summary>
        /// <param name="setValue">設定したい値</param>
        public void SetIsPlaying(bool setValue)
        {
            _isPlaying = setValue;
#if UNITY_EDITOR
            Debug.Log($"_isPlayingを{_isPlaying}にセットしました");
#endif
        }

        /// <summary>
        /// 再生するBGMをセットする
        /// </summary>
        /// <param name="audioClip">セットしたいBGM</param>
        public void SetAudioClip(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
        }

        /// <summary>
        /// 譜面データをセットする
        /// </summary>
        /// <param name="generateSettingDataBase">セットしたい譜面データ</param>
        public void SetGenerateSettingsDB(GenerateSettingDataBase generateSettingDataBase)
        {
            _generateSettingDataBase = generateSettingDataBase;
        }

        /// <summary>
        /// 楽曲の再生と残り時間のカウントを開始する
        /// </summary>
        public void StartMusic()
        {
            _remainingTimeManagerObj.SetActive(true);
            _audioSource.loop = false;
            _audioSource.Play();
            _remainingTimeManager.SetIsCount(true);
        }

        /// <summary>
        /// 主導権を取得した時間(スコア)を加算する
        /// </summary>
        private void AdditionInitiativeTime()
        {
            if (_initiativePlayerState == EInitiativePlayerState.None) return;
            if (_initiativePlayerState == EInitiativePlayerState.One)
            {
                _initiativeTimeP1 += Time.deltaTime;
                return;
            }

            _initiativeTimeP2 += Time.deltaTime;
        }

        /// <summary>
        /// グッド判定を取得したプレイヤーに主導権を付与
        /// コールバック関数として実装する
        /// </summary>
        /// <param name="playerNumber">プレイヤーの番号</param>
        public void OnGetNote(PlayerNumberState playerNumber)
        {
            if (playerNumber == PlayerNumberState.One)
            {
                if (_initiativePlayerState == EInitiativePlayerState.One) return;
                _initiativePlayerState = EInitiativePlayerState.One;
#if UNITY_EDITOR
                Debug.Log($"主導権を握っているプレイヤーを変更:{_initiativePlayerState}");
#endif
                return;

            }

            if (_initiativePlayerState == EInitiativePlayerState.Two) return;
            _initiativePlayerState = EInitiativePlayerState.Two;
#if UNITY_EDITOR
            Debug.Log($"主導権を握っているプレイヤーを変更:{_initiativePlayerState}");
#endif
        }

        #endregion
    }
}
