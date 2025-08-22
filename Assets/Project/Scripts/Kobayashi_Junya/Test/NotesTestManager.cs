using System.Collections;
using System.Collections.Generic;
using BatotteChannel.InGame.Notes;
using UnityEngine;

public class NotesTestManager : MonoBehaviour
{
    private float _time = 0.0f;

    private int _buttonNumber = 1;

    [SerializeField]
    private NoteManager _noteManager;

    // Start is called before the first frame update
    void Start()
    {
        _buttonNumber = 1;
        _time = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= 3.0f)
        {
            _time = 0.0f;
            GameObject note = _noteManager.GenerateNote(new Vector2(0, 0));
            note.GetComponent<NoteController>().SetButtonNumber(_buttonNumber);
            if (_buttonNumber > 8)
            {
                _buttonNumber = 1;
            }
            else
            {
                _buttonNumber++;
            }
        }
    }
}
