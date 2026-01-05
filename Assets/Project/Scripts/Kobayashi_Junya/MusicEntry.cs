using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Project.DataAsset.Music;

namespace Project.InGame.MusicSystem
{
    public class MusicEntry : MonoBehaviour
    {
        [Header("データの設定")]
        [SerializeField, Label("楽曲データ")]
        private SOMusicData _musicDatum;


    }
}
