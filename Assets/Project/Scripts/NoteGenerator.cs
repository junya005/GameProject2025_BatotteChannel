using System.Collections.Generic;
using MusicSystem;
using UnityEngine;

public class NoteGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _players;
    [SerializeField] private GenerateSettingDataBase _generateSettingDataBase;
    [SerializeField] private GameObject _notePrefab;
    [SerializeField] private BeatCounter _beatCounter;

    private void Start()
    {
        GenerateNote();
    }

    private void GenerateNote()
    {
        Instantiate(_notePrefab);
    }
}
