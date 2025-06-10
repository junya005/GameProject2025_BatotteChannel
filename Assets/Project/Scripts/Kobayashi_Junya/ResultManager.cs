using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using BatotteChannel.DataAssets;
using BatotteChannel.AudioSystem;


namespace BatotteChannel.GameManager
{
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

            // テキスト表示
            _scoreTextP1.text = $"Player1\nGood:{_resultDataP1.goodCount}\nMiss:{_resultDataP1.missCount}";
            // テキスト表示
            _scoreTextP2.text = $"Player2\nGood:{_resultDataP2.goodCount}\nMiss:{_resultDataP2.missCount}";
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
    }
}
