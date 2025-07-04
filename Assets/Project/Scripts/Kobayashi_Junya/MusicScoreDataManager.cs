using System.Collections;
using System.Collections.Generic;
using BatotteChannel.DataAssets;
using UnityEngine;

/// <summary>
/// 楽曲と譜面データの管理を行うシングルトンクラス
/// </summary>
public class MusicScoreDataManager : SceneSingleton<MusicScoreDataManager>
{
    [SerializeField]
    private MusicDataBaseScriptableObject _musicDataBase;

    public static Dictionary<string, MusicDataScriptableObject> musicDataBaseDictionary = new Dictionary<string, MusicDataScriptableObject>();

    void Start()
    {
        musicDataBaseDictionary.Clear();
        // 楽曲データベースから楽曲名で楽曲データのインデックスを生成
        foreach (var musicData in _musicDataBase.musicDataScriptableObjects)
        {
            musicDataBaseDictionary.Add(musicData.musicName, musicData);
        }
    }
}
