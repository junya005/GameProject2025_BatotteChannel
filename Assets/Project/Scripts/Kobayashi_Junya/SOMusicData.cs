using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Project.DataAsset.Notes;
using UnityEngine;

namespace Project.DataAsset.Music
{
    [SerializeField, CreateAssetMenu(fileName = "NewMusicData",
                                    menuName = "DataAssets/MusicData")]
    public class SOMusicData : ScriptableObject
    {
        [Label("楽曲名")]
        public string musicName;
        [Label("楽曲の長さ")]
        public float timeMax;
        [Label("譜面データのリスト")]
        public List<SONoteTimingList> maps;
    }
}
