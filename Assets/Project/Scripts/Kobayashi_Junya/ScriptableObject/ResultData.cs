using UnityEngine;

namespace BatotteChannel.DataAssets
{
    /// <summary>スコアの結果データを作成できる</summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ResultData")]
    public class ResultData : ScriptableObject
    {
        /// <summary>good判定の数</summary>
        public int goodCount;
        /// <summary>miss判定の数</summary>
        public int missCount;

        /// <summary>
        /// GoodCountを設定する
        /// </summary>
        /// <param name="value">設定したい値</param>
        public void SetGoodCount(int value)
        {
            goodCount = value;
        }

        /// <summary>
        /// missCountを設定する
        /// </summary>
        /// <param name="value">設定したい値</param>
        public void SetMissCount(int value)
        {
            missCount = value;
        }
    }
}
