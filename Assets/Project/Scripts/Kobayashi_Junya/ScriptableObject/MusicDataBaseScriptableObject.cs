using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BatotteChannel.DataAssets
{
    /// <summary>
    /// 楽曲データを表すScriptableObjectのデータベース
    /// </summary>
    [CreateAssetMenu(fileName = "newMusicDataBase", menuName = "ScriptableObjects/MusicDataBase")]
    public class MusicDataBaseScriptableObject : ScriptableObject
    {
        public List<MusicDataScriptableObject> musicDataScriptableObjects;
    }
}
