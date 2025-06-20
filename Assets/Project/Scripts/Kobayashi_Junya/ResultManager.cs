using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using BatotteChannel.DataAssets;
using BatotteChannel.AudioSystem;


namespace BatotteChannel.GameManager
{
    /// <summary>
    /// 丸めたプレイヤーの主導権時間と、その分と秒を格納する構造体
    /// </summary>
    public struct SRoundedInitiativeTimeMinutesSeconds
    {
        public float initiativeTime;
        public float initiativeMinutes;
        public float initiativeSeconds;
    }

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
            SRoundedInitiativeTimeMinutesSeconds _roundedInitiativeTimeMinutesSecondsP1 = new SRoundedInitiativeTimeMinutesSeconds();
            _roundedInitiativeTimeMinutesSecondsP1.initiativeTime = RoundInitiativeTime(
                _resultDataP1.initiativeTime,
                out var initiativeMinutesP1,
                out var initiativeSecondsP1
                );
            _roundedInitiativeTimeMinutesSecondsP1.initiativeMinutes = initiativeMinutesP1;
            _roundedInitiativeTimeMinutesSecondsP1.initiativeSeconds = initiativeSecondsP1;

            // Player2のスコアデータを整える
            SRoundedInitiativeTimeMinutesSeconds _roundedInitiativeTimeMinutesSecondsP2 = new SRoundedInitiativeTimeMinutesSeconds();
            _roundedInitiativeTimeMinutesSecondsP2.initiativeTime = RoundInitiativeTime(
                _resultDataP2.initiativeTime,
                out var initiativeMinutesP2,
                out var initiativeSecondsP2
                );
            _roundedInitiativeTimeMinutesSecondsP2.initiativeMinutes = initiativeMinutesP2;
            _roundedInitiativeTimeMinutesSecondsP2.initiativeSeconds = initiativeSecondsP2;

            // テキスト表示
            _scoreTextP1.text = "Player1\n" +
                            $"{_roundedInitiativeTimeMinutesSecondsP1.initiativeMinutes}:{_roundedInitiativeTimeMinutesSecondsP1.initiativeSeconds}\n" +
                            $"Good: {_resultDataP1.goodCount}\n" +
                            $"Miss:{_resultDataP1.missCount}";
            // テキスト表示
            _scoreTextP2.text = "Player2\n" +
                            $"{_roundedInitiativeTimeMinutesSecondsP2.initiativeMinutes}:{_roundedInitiativeTimeMinutesSecondsP2.initiativeSeconds}\n" +
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

        private int RoundInitiativeTime(float initiativeTime, out int minute, out int second)
        {
            int roundedInitiativeTime = Mathf.RoundToInt(initiativeTime);
            minute = Mathf.RoundToInt(roundedInitiativeTime / 60.0f);
            second = Mathf.RoundToInt(roundedInitiativeTime % 60.0f);
            return roundedInitiativeTime;
        }
    }
}
