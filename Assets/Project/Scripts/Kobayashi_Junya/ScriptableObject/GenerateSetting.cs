using UnityEngine;

namespace BatotteChannel.DataAssets
{
    /// <summary>
    /// 譜面生成タイミングと生成座標のデータアセットを作成できる
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GenerateSettingScriptableObject")]
    public class GenerateSettingScriptableObject : ScriptableObject
    {
        /// <summary>生成タイミング</summary>
        public float timing;

        /// <summary>生成座標</summary>
        public Vector3 generatePos;
    }
}
