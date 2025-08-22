using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using BatotteChannel.DataAssets;
using BatotteChannel.AudioSystem;
using System.Collections.Generic;


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

    public enum EWinnerState
    {
        Draw,
        P1,
        P2
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
        private List<TextMeshProUGUI> _scoreTextP1 = new List<TextMeshProUGUI>();

        [Tooltip("Player2のスコアを表示するテキストを設定"), SerializeField]
        private List<TextMeshProUGUI> _scoreTextP2 = new List<TextMeshProUGUI>();

        [SerializeField]
        private GameObject _drawBG;
        [SerializeField]
        private GameObject _winP1;
        [SerializeField]
        private GameObject _winP2;

        private EWinnerState _winnerState = EWinnerState.Draw;

        private void Start()
        {
            // フレームレートの設定
            GameSettingManager.Instance.SetAppFrameRateLimit(GameSettingManager.EnumFrameRateLimitState.Thirty);

            // BGMの再生
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlayBGM("Kira_Kira_Cotton_Candy");

            // Player1のスコアデータを整える
            SRoundedInitiativeTime _roundedInitiativeTimeP1 = new SRoundedInitiativeTime();
            _roundedInitiativeTimeP1 = FormatTime(_resultDataP1);

            // Player2のスコアデータを整える
            SRoundedInitiativeTime _roundedInitiativeTimeP2 = new SRoundedInitiativeTime();
            _roundedInitiativeTimeP2 = FormatTime(_resultDataP2);

            _winnerState = JudgeWinOrLose(_resultDataP1, _resultDataP2);

            // テキスト表示
            _scoreTextP1[0].text = $"{_roundedInitiativeTimeP1.initiativeMinutes}:" +
                                    $"{_roundedInitiativeTimeP1.initiativeSeconds.ToString("F0").PadLeft(2, '0')} ";
            _scoreTextP1[1].text = $"{_resultDataP1.goodCount}";
            _scoreTextP1[2].text = $"{_resultDataP1.missCount}";
            // テキスト表示
            _scoreTextP2[0].text = $"{_roundedInitiativeTimeP2.initiativeMinutes}:" +
                                    $"{_roundedInitiativeTimeP2.initiativeSeconds.ToString("F0").PadLeft(2, '0')}";
            _scoreTextP2[1].text = $"{_resultDataP2.goodCount}";
            _scoreTextP2[2].text = $"{_resultDataP2.missCount}";

            DisplayWinOrLose(_winnerState);
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

        /// <summary>
        /// 整えたスコアを丸める
        /// </summary>
        /// <param name="initiativeTime">主導権を握った時間</param>
        /// <param name="minute">分を出力</param>
        /// <param name="second">秒を出力</param>
        /// <returns>丸めた後の主導権を握った時間</returns>
        private int RoundInitiativeTime(float initiativeTime, out int minute, out int second)
        {
            int roundedInitiativeTime = Mathf.FloorToInt(initiativeTime);
            minute = Mathf.FloorToInt(roundedInitiativeTime / 60.0f);
            second = Mathf.FloorToInt(roundedInitiativeTime % 60.0f);
            return roundedInitiativeTime;
        }

        /// <summary>
        /// 勝敗を判定し、EWinnerState形で返却
        /// </summary>
        /// <param name="resultP1"></param>
        /// <param name="resultP2"></param>
        /// <returns>勝者</returns>
        private EWinnerState JudgeWinOrLose(ResultData resultP1, ResultData resultP2)
        {
            if (Mathf.FloorToInt(resultP1.initiativeTime) == Mathf.FloorToInt(resultP2.initiativeTime))
                return EWinnerState.Draw;

            if (resultP1.initiativeTime >= resultP2.initiativeTime)
                return EWinnerState.P1;

            return EWinnerState.P2;
        }

        /// <summary>
        /// 勝敗を表示する
        /// </summary>
        /// <param name="winnerState"></param>
        private void DisplayWinOrLose(EWinnerState winnerState)
        {
            if (winnerState == EWinnerState.Draw)
            {
                _drawBG.SetActive(true);
                _winP1.SetActive(false);
                _winP2.SetActive(false);
                return;
            }

            if (winnerState == EWinnerState.P1)
            {
                _drawBG.SetActive(false);
                _winP1.SetActive(true);
                _winP2.SetActive(false);
                return;
            }

            _drawBG.SetActive(false);
            _winP1.SetActive(false);
            _winP2.SetActive(true);
        }

        /// <summary>
        /// リザルトデータをスコア(時間)を整え、丸めた後に構造体を戻す
        /// </summary>
        /// <param name="resultData"></param>
        /// <returns></returns>
        private SRoundedInitiativeTime FormatTime(ResultData resultData)
        {
            SRoundedInitiativeTime roundedInitiativeTime = new SRoundedInitiativeTime();
            roundedInitiativeTime.initiativeTime = RoundInitiativeTime(
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
