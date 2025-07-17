using UnityEngine;

namespace BatotteChannel.DataAssets
{
    /// <summary>
    /// 楽曲データを表すScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "newMusicData", menuName = "ScriptableObjects/MusicData")]
    public class MusicDataScriptableObject : ScriptableObject
    {
        /// <summary>
        /// 楽曲名
        /// </summary>
        public string musicName;

        /// <summary>
        /// 楽曲の分数
        /// </summary>
        public int musicTimeMinute;

        /// <summary>
        /// 楽曲の秒数
        /// </summary>
        public int musicTimeSecond;

        /// <summary>
        /// 楽曲に対応する譜面データ
        /// </summary>
        public GenerateSettingDataBase musicGenerateSettingDataBase;
    }
}
