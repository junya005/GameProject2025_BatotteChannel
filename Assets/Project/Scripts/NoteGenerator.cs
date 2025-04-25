using System;
using System.Collections.Generic;
using System.Diagnostics;
using MusicSystem;
using UnityEngine;

public class NoteGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _players = new List<GameObject>();
    [SerializeField] private GenerateSettingDataBase _generateSettingDataBase;
    [SerializeField] private GameObject _notePrefab;
    [SerializeField] private BeatCounter _beatCounter;
    [SerializeField] private List<GameObject> _generatedNotesForPlayerOne = new List<GameObject>();
    [SerializeField] private List<GameObject> _generatedNotesForPlayerTwo = new List<GameObject>();
    private int _currentNoteIndex = 0;

    private void Start()
    {

    }

    private void Update()
    {
        GenerateNote();
        DeleteNotesByNoteList(_generatedNotesForPlayerOne);
        DeleteNotesByNoteList(_generatedNotesForPlayerTwo);
    }

    private void GenerateNote()
    {
        if (_beatCounter.Beat >= _generateSettingDataBase.generateSettingList[_currentNoteIndex].timing)
        {
            GameObject generatedNote = Instantiate(_notePrefab, _generateSettingDataBase.generateSettingList[_currentNoteIndex].generatePos, Quaternion.identity);
            switch (_generateSettingDataBase.generateSettingList[_currentNoteIndex].playerNum)
            {
                case 0:
                    _generatedNotesForPlayerOne.Add(generatedNote);
                    break;
                case 1:
                    _generatedNotesForPlayerTwo.Add(generatedNote);
                    break;
            }
            _currentNoteIndex++;
        }
    }

    private void DeleteNotesByNoteList(List<GameObject> noteList)
    {
        try
        {
            if (noteList != null && !noteList[0].GetComponent<NoteController>().IsEnableThisNotes)
            {
                noteList.RemoveAt(0);
            }
        }
        catch (MissingReferenceException)
        {
            noteList.RemoveAt(0);
        }
        catch (ArgumentOutOfRangeException)
        {
            return;
        }
    }
}
