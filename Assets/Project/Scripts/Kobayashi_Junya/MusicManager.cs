using UnityEngine;
using TMPro;
using BatotteChannel.InGame.Notes;
using BatotteChannel.GameState;
using BatotteChannel.DataAssets;
using System.Collections;

namespace BatotteChannel.InGame.MusicSystem
{
    /// <summary>リズムゲームパートを管理するクラス AudioSourceとBeatCounterとの併用を想定</summary>
    [RequireComponent(typeof(AudioSource)), RequireComponent(typeof(BeatCounter))]
    public class MusicManager : MonoBehaviour
    {
        #region 定数

        // スタン秒数を難易度ごとに定義
        private const float STAN_TIME_KIDS = 0.0f;
        private const float STAN_TIME_EASY = 3.0f;
        private const float STAN_TIME_NORMAL = 2.0f;
        private const float STAN_TIME_HARD = 1.0f;

        #endregion

        #region 変数

        /// <summary>楽曲データを格納</summary>
        private MusicDataScriptableObject _musicData;

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

        [Tooltip("ENDテキストを表示させるオブジェクト"), SerializeField]
        private GameObject _endTextObj;

        [Tooltip("楽曲データを設定"), SerializeField]
        private MusicDataScriptableObject _MusicData;

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
        private int _genSetIndex = 0;

        /// <summary>主導権を握ったプレイヤーが変更されたか</summary>
        private bool _isSetInitiativePlayerState = false;

        // 現在の難易度ステート
        private EDifficultyState _currentDifficultyState;

        // 現在難易度のゲッタープロパティ
        public EDifficultyState CurrentDifficultyState { get { return _currentDifficultyState; } }

        /// <summary>チャンネルの切り替えアニメーションクラス</summary>
        [SerializeField]
        private ChannelChangeAnim _channelChangeAnim;

        #endregion

        #region イベント関数

        private void Start()
        {
            InisializeMusicManager();
        }

        private void Update()
        {
            if (!_isPlaying) return;

            // スコアの加算処理
            AdditionInitiativeTime(_initiativePlayerState);
            ViewScore();

            // リズムゲームパートの終了
            if (_remainingTimeManager.IsCompleteCount == true && _isEndProcessing == false)
            {
                EndRhythmGamePart();
            }

            if (_genSetIndex == _generateSettingDataBase.generateSettingList.Count) return;

            // ここからノーツ生成処理
            // 細かいタイミングを考慮する場合の生成処理
            if (_generateSettingDataBase.generateSettingList[_genSetIndex].isUseSubBeat)
            {
                if (_beatCounter.Beat >= _generateSettingDataBase.generateSettingList[_genSetIndex].timing &&
                _beatCounter.SubBeat >= _generateSettingDataBase.generateSettingList[_genSetIndex].subTiming)
                {
#if UNITY_EDITOR
                    Debug.Log("サブタイミングに生成します。");
#endif
                    GenerateNotesByTimming();
                }

                bool subTimigValueCheck = _generateSettingDataBase.generateSettingList[_genSetIndex].subTiming >= 4
                || _generateSettingDataBase.generateSettingList[_genSetIndex].subTiming <= 0;
                if (!subTimigValueCheck)
                {
                    return;
                }
            }

            // 通常の生成処理
            if (_beatCounter.Beat >= _generateSettingDataBase.generateSettingList[_genSetIndex].timing)
            {
                GenerateNotesByTimming();
            }
        }

        #endregion

        #region 関数

        /// <summary>
        /// 初期化処理
        /// </summary>
        private void InisializeMusicManager()
        {
            // キャッシュ
            _beatCounter = GetComponent<BeatCounter>();
            _audioSource = GetComponent<AudioSource>();
            _remainingTimeManager = _remainingTimeManagerObj.GetComponent<RemainingTimeManager>();

            // 初期設定
            SetAudioClip(_audioClip);

            // 楽曲時間の設定
            _remainingTimeManager.SetMusicTime(_musicData.musicTimeMinute, _musicData.musicTimeSecond);

            // スタン時間の設定
            SetStanSecondsToNoteManagers();

            // 終了時テキストを非表示
            _endTextObj.SetActive(false);

            // 難易度がKZであればキッズノーツを生成するよう設定
            if (GetDifficulty(_musicData) == EDifficultyState.Kids)
            {
                _noteManagerP1.SetKidsMode(true);
                _noteManagerP2.SetKidsMode(true);
            }

            // ノーツが取得されたときのコールバックにOnGetNoteを登録
            // 秒数換算に使用します
            _noteManagerP1.getNoteCallBack += OnGetNote;
            _noteManagerP2.getNoteCallBack += OnGetNote;

            // ノーツをミスした時のイベント処理を登録
            // 秒数計上終了時に使用
            _noteManagerP1.getMissNoteCallBack += OnMissNote;
            _noteManagerP2.getMissNoteCallBack += OnMissNote;
        }

        /// <summary>
        /// リズムゲームパートの終了処理
        /// </summary>
        private void EndRhythmGamePart()
        {
            // フラグを終了時の状態にセット
            _isPlaying = false;
            _isEndProcessing = true;

            // 終了のUIを表示
            _endTextObj.SetActive(true);

            // 最終スコアにデータをセット
            // Player1
            _resultDataP1.SetGoodCount(_noteManagerP1.GotGoodCount);
            _resultDataP1.SetMissCount(_noteManagerP1.GotMissCount);
            _resultDataP1.SetInitiativeTime(_initiativeTimeP1);

            // Player2
            _resultDataP2.SetGoodCount(_noteManagerP2.GotGoodCount);
            _resultDataP2.SetMissCount(_noteManagerP2.GotMissCount);
            _resultDataP2.SetInitiativeTime(_initiativeTimeP2);

            // リザルトへ移行
            InGameStateManager.Instance.SetCurrentInGameState(InGameState.End);
        }

        /// <summary>
        /// 難易度に応じてStanTimeをセットする
        /// </summary>
        private void SetStanSecondsToNoteManagers()
        {
            switch (_currentDifficultyState)
            {
                case EDifficultyState.Kids:
                    _noteManagerP1.SetStanTimeSecond(STAN_TIME_KIDS);
                    _noteManagerP2.SetStanTimeSecond(STAN_TIME_KIDS);
                    return;
                case EDifficultyState.Easy:
                    _noteManagerP1.SetStanTimeSecond(STAN_TIME_EASY);
                    _noteManagerP2.SetStanTimeSecond(STAN_TIME_EASY);
                    return;
                case EDifficultyState.Normal:
                    _noteManagerP1.SetStanTimeSecond(STAN_TIME_NORMAL);
                    _noteManagerP2.SetStanTimeSecond(STAN_TIME_NORMAL);
                    return;
                case EDifficultyState.Hard:
                    _noteManagerP1.SetStanTimeSecond(STAN_TIME_HARD);
                    _noteManagerP2.SetStanTimeSecond(STAN_TIME_HARD);
                    return;
                default:
                    Debug.LogError("難易度が想定外の値になっています。");
                    return;
            }
        }

        /// <summary>
        /// 楽曲名の背景2文字から難易度を取得する
        /// </summary>
        /// <param name="musicData">楽曲データ</param>
        /// <returns>識別不可な場合はNoneを返します</returns>
        public EDifficultyState GetDifficulty(MusicDataScriptableObject musicData)
        {
            // 楽曲名を取得
            string musicName = musicData.musicName;

            // 楽曲名の背後2文字を取得
            string difficultyStr = musicName.Substring(musicName.Length - 2, 2);

            // 難易度を返す
            EDifficultyState difficultyState;
            switch (difficultyStr)
            {
                case "KZ":
                    difficultyState = EDifficultyState.Kids;
                    break;
                case "EZ":
                    difficultyState = EDifficultyState.Easy;
                    break;
                case "NL":
                    difficultyState = EDifficultyState.Normal;
                    break;
                case "HD":
                    difficultyState = EDifficultyState.Hard;
                    break;
                default:
                    difficultyState = EDifficultyState.None;
                    break;
            }
            return difficultyState;
        }

        /// <summary>
        /// ノーツ生成をNoteManagerに指示
        /// </summary>
        private void GenerateNotesByTimming()
        {
            // 生成設定がプレイヤー1か2かを判定し、それに対する生成を行う
            // スタン状態処理はNoteManager側で行っているため、こちらで判定を行う必要はないです
            if (_generateSettingDataBase.generateSettingList[_genSetIndex].player == PlayerNumberState.Two)
            {
                GenerateNote(_noteManagerP2);
                return;
            }

            GenerateNote(_noteManagerP1);
        }

        /// <summary>
        /// ノーツ生成の共通処理
        /// </summary>
        /// <param name="noteManager"></param>
        private void GenerateNote(NoteManager noteManager)
        {
            // 生成前に生成番号を渡しておく(重ね順決定に必要なため)
            noteManager.SetGenNum(_genSetIndex);

            // ノーツ生成を指示
            noteManager.GenerateNote(_generateSettingDataBase.generateSettingList[_genSetIndex].generatePos);

            // インデックスをインクリメント
            _genSetIndex++;
        }

        /// <summary>
        /// スコアを表示する
        /// </summary>
        private void ViewScore()
        {
            // TODO スコア表示をしない場合の処理

            // Player1のスコアを表示
            int roundedInitiativeTimeP1 = Mathf.FloorToInt(_initiativeTimeP1);
            int roundedInitiativeTimeMinuteP1 = Mathf.FloorToInt(roundedInitiativeTimeP1 / 60);
            int roundedInitiativeTimeSecondsP1 = Mathf.FloorToInt(roundedInitiativeTimeP1 % 60);
            _p1ScoreTMPro.text = $"{roundedInitiativeTimeMinuteP1}:{roundedInitiativeTimeSecondsP1.ToString("F0").PadLeft(2, '0')}";

            // Player2のスコアを表示
            int roundedInitiativeTimeP2 = Mathf.FloorToInt(_initiativeTimeP2);
            int roundedInitiativeTimeMinuteP2 = Mathf.FloorToInt(roundedInitiativeTimeP2 / 60);
            int roundedInitiativeTimeSecondsP2 = Mathf.FloorToInt(roundedInitiativeTimeP2 % 60);
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
        /// 現在の難易度をセットする
        /// </summary>
        /// <param name="difficulty">難易度</param>
        public void SetCurrentDifficultyState(EDifficultyState difficulty)
        {
            _currentDifficultyState = difficulty;
#if UNITY_EDITOR
            Debug.Log($"難易度を{_currentDifficultyState}にセットしました。");
#endif
        }

        /// <summary>
        /// InitiativePlayerStateのセッター
        /// </summary>
        /// <param name="initiativePlayerState">プレイヤーを指定</param>
        private void SetInitiativePlayerState(EInitiativePlayerState initiativePlayerState)
        {
            _initiativePlayerState = initiativePlayerState;
            _isSetInitiativePlayerState = true;
#if UNITY_EDITOR
            Debug.Log($"主導権を握っているプレイヤーを変更:{_initiativePlayerState}");
#endif
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
        private void AdditionInitiativeTime(EInitiativePlayerState initiativePlayerState)
        {
            if (initiativePlayerState == EInitiativePlayerState.None) return;
            if (initiativePlayerState == EInitiativePlayerState.One)
            {
                _initiativeTimeP1 += Time.deltaTime;
                return;
            }

            _initiativeTimeP2 += Time.deltaTime;
        }

        /// <summary>
        /// ノーツ取得時の処理(NoteManagerのイベントへバインド)
        /// </summary>
        /// <param name="playerNumber">プレイヤーの番号</param>
        public void OnGetNote(PlayerNumberState playerNumber, float correctionValue)
        {
            // 誤差分スコアに反映させる
            if (playerNumber == PlayerNumberState.One)
            {
                _initiativeTimeP1 += correctionValue;
            }
            else if (playerNumber == PlayerNumberState.Two)
            {
                _initiativeTimeP2 += correctionValue;
            }

            // 主導権を握るプレイヤーを変更
            GivePlayerInitiative(playerNumber);

            // チャンネルの変更エフェクトを再生
            PlayChannelChangeEffect(playerNumber);
        }

        /// <summary>
        /// ノーツミス時の処理(NoteManagerのイベントへバインド)
        /// </summary>
        public void OnMissNote(PlayerNumberState playerNumber)
        {
            // 最後のノーツであればだれも主導権を握っていない状態に
            if (_generateSettingDataBase.generateSettingList.Count <= _genSetIndex)
            {
                SetInitiativePlayerState(EInitiativePlayerState.None);
                return;
            }
        }

        /// <summary>
        /// グッド判定を取得したプレイヤーに主導権を付与
        /// </summary>
        /// <param name="playerNumber"></param>
        private void GivePlayerInitiative(PlayerNumberState playerNumber)
        {
            // 最後のノーツであればだれも主導権を握っていない状態に
            if (_generateSettingDataBase.generateSettingList.Count <= _genSetIndex)
            {
                SetInitiativePlayerState(EInitiativePlayerState.None);
                return;
            }

            // 引数のプレイヤーに主導権を変更
            if (playerNumber == PlayerNumberState.One)
            {
                if (_initiativePlayerState == EInitiativePlayerState.One) return;
                // _initiativePlayerState = EInitiativePlayerState.One;
                SetInitiativePlayerState(EInitiativePlayerState.One);
                //StartCoroutine(GiveInitiative(3.0f));
                return;
            }

            if (_initiativePlayerState == EInitiativePlayerState.Two) return;
            // _initiativePlayerState = EInitiativePlayerState.Two;
            SetInitiativePlayerState(EInitiativePlayerState.Two);
            //StartCoroutine(GiveInitiative(3.0f));
        }

        /// <summary>
        /// チャンネル切り替えエフェクトを再生
        /// </summary>
        /// <param name="playerNumber"></param>
        private void PlayChannelChangeEffect(PlayerNumberState playerNumber)
        {
            bool isPlayer1 = playerNumber == PlayerNumberState.One ? true : false;
            _channelChangeAnim.ChangeAnim(isPlayer1);
        }

        /// <summary>
        /// 時間に応じて主導権秒数を付与
        /// </summary>
        /// <param name="earningTime"></param>
        /// <returns></returns>
        private IEnumerator GiveInitiative(float earningTime)
        {
            float time = 0.0f;
            while (time < earningTime)
            {
                time += Time.deltaTime;

                // AdditionInitiativeTime(initiativePlayer);
                // initiativePlayerStateが変更されたらコルーチンを終了する
                if (_isSetInitiativePlayerState)
                {
                    _isSetInitiativePlayerState = false;
                    yield break;
                }
                else
                {
                    yield return null;
                }
            }

            SetInitiativePlayerState(EInitiativePlayerState.None);
        }

        /// <summary>
        /// 楽曲データを設定する
        /// </summary>
        /// <param name="musicData"></param>
        public void SetMusicData(MusicDataScriptableObject musicData)
        {
            _musicData = musicData;
        }

        #endregion
    }
}
