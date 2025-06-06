using UnityEngine;
using TMPro;

using BatotteChannel.InGame.Notes;
using BatotteChannel.GameState;
using BatotteChannel.DataAssets;

namespace BatotteChannel.InGame.MusicSystem
{
    #region 変数

    /// <summary>リズムゲームパートを管理するクラス AudioSourceとBeatCounterとの併用を想定</summary>
    [RequireComponent(typeof(AudioSource)), RequireComponent(typeof(BeatCounter))]
    public class MusicManager : MonoBehaviour
    {
        /// <summary>
        /// ビート計上クラスを格納
        /// Startで取得するので設定の必要なし
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

        /// <summary>ノーツ生成がされる状態か</summary>
        private bool _isPlaying;

        /// <summary>ノーツ生成がされる状態かのゲッタープロパティ</summary>
        public bool IsPlaying
        {
            get { return _isPlaying; }
        }

        /// <summary>リズムゲームパートの終了処理が開始されたか</summary>
        private bool _isEndProcessing = false;

        /// <summary>ノーツ生成インデックス番号を格納</summary>
        int genSetIndex = 0;

        #endregion

        #region 関数

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

        #endregion

        private void Start()
        {
            // キャッシュ
            _beatCounter = GetComponent<BeatCounter>();
            _audioSource = GetComponent<AudioSource>();
            _remainingTimeManager = _remainingTimeManagerObj.GetComponent<RemainingTimeManager>();

            // 初期設定
            SetAudioClip(_audioClip);
            // 一旦仮で実数値で設定してます
            _remainingTimeManager.SetMusicTime(0, 48);
        }

        private void Update()
        {
            if (!_isPlaying) return;
            // スコアの表示
            _p1ScoreTMPro.text = $"Good:{_noteManagerP1.GotGoodCount}\nMiss:{_noteManagerP1.GotMissCount}";
            _p2ScoreTMPro.text = $"Good:{_noteManagerP2.GotGoodCount}\nMiss:{_noteManagerP2.GotMissCount}";

            // リズムゲームパートの終了処理
            if (_remainingTimeManager.IsCompleteCount == true && _isEndProcessing == false)
            {
                _isEndProcessing = true;
                _resultDataP1.SetGoodCount(_noteManagerP1.GotGoodCount);
                _resultDataP1.SetMissCount(_noteManagerP1.GotMissCount);
                _resultDataP2.SetGoodCount(_noteManagerP2.GotGoodCount);
                _resultDataP2.SetMissCount(_noteManagerP2.GotMissCount);
                InGameStateManager.Instance.SetCurrentInGameState(InGameState.End);
            }

            // ノーツの生成処理
            if (genSetIndex == _generateSettingDataBase.generateSettingList.Count) return;
            if (_beatCounter.Beat >= _generateSettingDataBase.generateSettingList[genSetIndex].timing)
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
        }
    }
}
