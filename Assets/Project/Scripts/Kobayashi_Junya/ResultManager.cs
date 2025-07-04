using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using BatotteChannel.DataAssets;
using BatotteChannel.AudioSystem;


namespace BatotteChannel.GameManager
{
    /// <summary>
    /// 丸めたプレイヤーの主導権時間を格納する構造体
    /// </summary>
    public struct SRoundedInitiativeTime
    {
        public float initiativeTime;
        public float initiativeMinutes;
        public float initiativeSeconds;
    }

    /// <summary>
    /// リザルトシーンを管理するクラス
    /// </summary>
    public class ResultManager : MonoBehaviour
    {
        [Tooltip("Player1のスコアが格納されているデータを設定"), SerializeField]
        private ResultData _resultDataP1;

        [Tooltip("Player2のスコアが格納されているデータを設定"), SerializeField]
        private ResultData _resultDataP2;

        [Tooltip("Player1のスコアを表示するテキストを設定"), SerializeField]
        private TextMeshProUGUI _scoreTextP1;

        [Tooltip("Player2のスコアを表示するテキストを設定"), SerializeField]
        private TextMeshProUGUI _scoreTextP2;

        private void Start()
        {
            // フレームレートの設定
            GameSettingManager.Instance.SetAppFrameRateLimit(GameSettingManager.EnumFrameRateLimitState.Thirty);

            // Player1のスコアデータを整える
            SRoundedInitiativeTime _roundedInitiativeTimeMinutesSecondsP1 = new SRoundedInitiativeTime();
            _roundedInitiativeTimeMinutesSecondsP1.initiativeTime = RoundInitiativedTime(
                _resultDataP1.initiativeTime,
                out var initiativeMinutesP1,
                out var initiativeSecondsP1
                );
            _roundedInitiativeTimeMinutesSecondsP1.initiativeMinutes = initiativeMinutesP1;
            _roundedInitiativeTimeMinutesSecondsP1.initiativeSeconds = initiativeSecondsP1;

            // Player2のスコアデータを整える
            SRoundedInitiativeTime _roundedInitiativeTimeMinutesSecondsP2 = new SRoundedInitiativeTime();
            _roundedInitiativeTimeMinutesSecondsP2.initiativeTime = RoundInitiativedTime(
                _resultDataP2.initiativeTime,
                out var initiativeMinutesP2,
                out var initiativeSecondsP2
                );
            _roundedInitiativeTimeMinutesSecondsP2.initiativeMinutes = initiativeMinutesP2;
            _roundedInitiativeTimeMinutesSecondsP2.initiativeSeconds = initiativeSecondsP2;

            // テキスト表示
            _scoreTextP1.text = "Player1\n" +
                            $"{_roundedInitiativeTimeMinutesSecondsP1.initiativeMinutes}:" +
                            $"{_roundedInitiativeTimeMinutesSecondsP1.initiativeSeconds.ToString("F0").PadLeft(2, '0')}\n" +
                            $"Good: {_resultDataP1.goodCount}\n" +
                            $"Miss:{_resultDataP1.missCount}";
            // テキスト表示
            _scoreTextP2.text = "Player2\n" +
                            $"{_roundedInitiativeTimeMinutesSecondsP2.initiativeMinutes}:" +
                            $"{_roundedInitiativeTimeMinutesSecondsP2.initiativeSeconds.ToString("F0").PadLeft(2, '0')}\n" +
                            $"Good:{_resultDataP2.goodCount}\n" +
                            $"Miss: {_resultDataP2.missCount} ";
        }

        /// <summary>
        /// タイトル画面へ遷移する
        /// </summary>
        public void ToTitle(string sceneName)
        {
            Debug.Assert(sceneName == "Main");

            SoundManager.Instance?.PlaySE("push_determining_button_53");

            SceneManager.LoadScene(sceneName);
        }

        private int RoundInitiativedTime(float initiativeTime, out int minute, out int second)
        {
            int roundedInitiativeTime = Mathf.FloorToInt(initiativeTime);
            minute = Mathf.FloorToInt(roundedInitiativeTime / 60.0f);
            second = Mathf.FloorToInt(roundedInitiativeTime % 60.0f);
            return roundedInitiativeTime;
        }

        /// <summary>
        /// スコア(時間)を整え、分と秒に分けた数値で返却する
        /// </summary>
        /// <param name="resultData"></param>
        /// <returns></returns>
        private SRoundedInitiativeTime InitiativeTime(ResultData resultData)
        {
            SRoundedInitiativeTime roundedInitiativeTime = new SRoundedInitiativeTime();
            roundedInitiativeTime.initiativeTime = RoundInitiativedTime(
                resultData.initiativeTime,
                out var initiativeMinutes,
                out var initiativeSeconds
                );
            roundedInitiativeTime.initiativeMinutes = initiativeMinutes;
            roundedInitiativeTime.initiativeSeconds = initiativeSeconds;
            return roundedInitiativeTime;
        }
    }
}
