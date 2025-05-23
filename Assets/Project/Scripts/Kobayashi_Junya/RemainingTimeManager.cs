using UnityEngine;
using TMPro;

namespace BatotteChannel.InGame.MusicSystem
{
    /// <summary>残り時間を表示するクラス</summary>
    public class RemainingTimeManager : MonoBehaviour
    {
        #region 定数
        /// <summary>1分を秒で表したもの</summary>
        private readonly float MINUTE_IN_SECOND = 60.0f;

        /// <summary>0秒を表したもの</summary>
        private readonly float TIME_ZERO = 0.0f;

        #endregion

        #region 変数

        [Header("パラメーター")]
        [Tooltip("楽曲の分数を入れてください"), SerializeField]
        private int _musicTimeMinute;

        /// <summary>楽曲の分数のゲッタープロパティ</summary>
        public int MusicTimeMinute
        {
            get { return _musicTimeMinute; }
        }

        [Tooltip("楽曲の秒数を入れてください"), SerializeField]
        private float _musicTimeSecond;

        /// <summary>楽曲の秒数のゲッタープロパティ</summary>
        public float MusicTimeSecond
        {
            get { return _musicTimeSecond; }
        }

        /// <summary>秒数へ変換済みの楽曲時間を格納</summary>
        private float _musicTime;

        /// <summary>現在の楽曲時間</summary>
        private float _currentRemainingTime;

        [Header("オブジェクト参照")]
        [Tooltip("残り時間を表示するテキストを設定してください"), SerializeField]
        private TextMeshProUGUI _remainingTimeText;

        /// <summary>残り時間をカウントできる状態か</summary>
        private bool _isCount = false;

        /// <summary>残り時間をカウントできる状態かのゲッタープロパティ</summary>
        public bool IsCount
        {
            get { return _isCount; }
        }

        /// <summary>カウントが完了したかどうか</summary>
        private bool _isCompleteCount = false;

        /// <summary>カウントが完了したかどうかのゲッタープロパティ</summary>
        public bool IsCompleteCount
        {
            get { return _isCompleteCount; }
        }

        #endregion

        #region 関数

        /// <summary>
        /// 楽曲時間をセットする
        /// </summary>
        /// <param name="minute">楽曲の分数</param>
        /// <param name="second">楽曲の秒数</param>
        public void SetMusicTime(int minute, float second)
        {
            _musicTimeMinute = minute;
            _musicTimeSecond = second;
            ChangeMusicTimeToSeconds(minute, second);
            ResetRemainingTime();
#if UNITY_EDITOR
            Debug.Log($"楽曲時間を{_musicTimeMinute}:{_musicTimeSecond}に設定し、現在時間をリセットしました。");
#endif
        }

        /// <summary>
        /// 残り時間をカウントできる状態かのセッター関数
        /// </summary>
        /// <param name="value">設定したい値</param>
        public void SetIsCount(bool value)
        {
            _isCount = value;
#if UNITY_EDITOR
            Debug.Log($"_isCountを{value}に設定しました");
#endif
        }

        /// <summary>
        /// カウントが完了したかどうかのセッター関数
        /// </summary>
        /// <param name="value">設定したい値</param>
        public void SetIsCompleteCount(bool value)
        {
            _isCompleteCount = value;
#if UNITY_EDITOR
            Debug.Log($"_isCompleteCountを{value}に設定しました");
#endif
        }

        /// <summary>
        /// 楽曲時間を秒数へ変換
        /// </summary>
        /// <param name="minutes">楽曲分数</param>
        /// <param name="seconds">楽曲秒数</param>
        private void ChangeMusicTimeToSeconds(int minutes, float seconds)
        {
            _musicTime = (float)minutes * MINUTE_IN_SECOND + seconds;
        }

        /// <summary>
        /// 楽曲の残り時間をリセットする
        /// </summary>
        public void ResetRemainingTime()
        {
            _currentRemainingTime = _musicTime;
        }

        /// <summary>
        /// 残り時間をテキストに表示する
        /// </summary>
        private void DisplayRemainingTime()
        {
            // 表示するために値を整える
            int minutes = (int)(Mathf.Round(_currentRemainingTime) / MINUTE_IN_SECOND);
            float seconds = Mathf.Round(_currentRemainingTime) % MINUTE_IN_SECOND;

            if (_currentRemainingTime <= TIME_ZERO)
            {
                _remainingTimeText.text = "End!";
            }
            else
            {
                // 残り楽曲時間の表示
                _remainingTimeText.text = $"{minutes}:{seconds.ToString("F0").PadLeft(2, '0')}";
            }
        }

        #endregion

        #region イベント関数

        void Start()
        {
            ChangeMusicTimeToSeconds(_musicTimeMinute, _musicTimeSecond);
            ResetRemainingTime();
        }

        void Update()
        {
            DisplayRemainingTime();

            if (!_isCount) return;
            _currentRemainingTime -= Time.deltaTime;

            if (_currentRemainingTime <= 0.0f && _isCompleteCount != true)
            {
                _isCompleteCount = true;
#if UNITY_EDITOR
                Debug.Log(_isCompleteCount);
#endif
            }
        }

        #endregion
    }
}
