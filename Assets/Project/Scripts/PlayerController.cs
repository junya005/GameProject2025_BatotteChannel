using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("プレイヤーの番号を入力(0から)"), SerializeField]
    private int _playerNumber = 0;

    /// <summary>
    /// プレイヤー番号のプロパティ
    /// </summary>
    public int PlayerNumber
    {
        get { return _playerNumber; }
    }

    /// <summary>このプレイヤー用のノーツを格納</summary>
    private List<GameObject> _notes;

    public void SetNote(GameObject gameObject)
    {
        _notes.Add(gameObject);
    }

    private void GetNote()
    {
        if (_notes[0] == null) return;
        _notes[0].GetComponent<NoteController>().JudgementNote();
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetNote();
        }
    }


}
