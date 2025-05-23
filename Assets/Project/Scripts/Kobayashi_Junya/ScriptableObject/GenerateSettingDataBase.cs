using System.Collections.Generic;
using UnityEngine;

namespace BatotteChannel.DataAssets
{
    /// <summary>
    /// ノーツ生成データのデータベースを作成できる
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GenerateSettingDataBase")]
    public class GenerateSettingDataBase : ScriptableObject
    {
        /// <summary>ノーツ生成データのデータベース</summary>
        public List<GenerateSettingScriptableObject> generateSettingList;
    }
}
