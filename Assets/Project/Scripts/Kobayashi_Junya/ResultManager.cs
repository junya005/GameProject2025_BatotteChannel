using UnityEngine;
using TMPro;

using BatotteChannel.DataAssets;

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
            // テキスト表示
            _scoreTextP1.text = $"Player1\nGood:{_resultDataP1.goodCount}\nMiss:{_resultDataP1.missCount}";
        }
    }
}
