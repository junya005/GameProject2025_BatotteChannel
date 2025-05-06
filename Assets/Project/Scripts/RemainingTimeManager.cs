using TMPro;
using UnityEngine;

namespace BatotteChannel.TimeSystem
{
    /// <summary>残り時間を表示するクラス</summary>
    public class RemainingTimeManager : MonoBehaviour
    {
        #region 定数

        private readonly float MINUTE_IN_SECOND = 60.0f;
        private readonly float TIME_ZERO = 0.0f;

        #endregion

        #region 変数

        [Header("パラメーター")]
        [Tooltip("楽曲の分数を入れてください"), SerializeField]
        private int _musicTimeMinute;

        public int MusicTimeMinute
        {
            get { return _musicTimeMinute; }
        }

        [Tooltip("楽曲の秒数を入れてください"), SerializeField]
        private float _musicTimeSecond;

        public float MusicTimeSecond
        {
            get { return _musicTimeSecond; }
        }

        /// <summary>秒数へ変換済みの楽曲時間を格納</summary>
        private float _musicTime;
        /// <summary>現在の楽曲時間</summary>
        private float _currentRemainingTime;
        /// <summary>残り分数</summary>
        private int _remainingTimeMinutes;
        /// <summary>残り秒数</summary>
        private float _remainingTimeSeconds;

        [Header("オブジェクト参照")]
        [Tooltip("残り時間を表示するテキストを設定してください"), SerializeField]
        private TextMeshProUGUI _remainingTimeText;

        #endregion

        #region 関数

        public void SetMusicTime(int minute, float second)
        {
            _musicTimeMinute = minute;
            _musicTimeSecond = second;
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
        private void ResetRemainingTime()
        {
            _currentRemainingTime = _musicTime;
        }

        private void DisplayRemainingTime()
        {
            // 表示するために値を整える
            _currentRemainingTime -= Time.deltaTime;
            _remainingTimeMinutes = (int)(Mathf.Round(_currentRemainingTime) / MINUTE_IN_SECOND);
            _remainingTimeSeconds = Mathf.Round(_currentRemainingTime) % MINUTE_IN_SECOND;

            if (_currentRemainingTime <= TIME_ZERO)
            {
                _remainingTimeText.text = "Complete!";
            }
            else
            {
                // 残り楽曲時間の表示
                _remainingTimeText.text = $"{_remainingTimeMinutes}:{_remainingTimeSeconds.ToString("F0").PadLeft(2, '0')}";
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

        }

        #endregion
    }
}
