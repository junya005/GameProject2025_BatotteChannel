using System.Collections;
using System.Collections.Generic;
using System.Threading;
using BatotteChannel.InGame.Notes;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TutorialNotesManager : MonoBehaviour
{
    private float _time = 0.0f;

    private int _buttonNumber = 1;

    [SerializeField]
    private NoteManager _noteManagerPlayer1;

    [SerializeField]
    private NoteManager _noteManagerPlayer2;

    [SerializeField]
    private Vector2 _generatePosP1 = new Vector2(0, 0);

    [SerializeField]
    private Vector2 _generatePosP2 = new Vector2(0, 0);

    [SerializeField]
    private bool _canGenerate = false;

    public bool CanGenerate { get { return _canGenerate; } }

    // Start is called before the first frame update
    void Start()
    {
        _buttonNumber = 0;
        _time = 3.0f;
    }

    // Update is called once per frame
    // void Update()
    // {
    // if (_canGenerate == false) return;

    // _time += Time.deltaTime;

    // if (_time >= 3.0f)
    // {
    //     _time = 0.0f;
    //     GenerateNoteOrder();
    // }
    // }

    void OnEnable()
    {
        _noteManagerPlayer1.gameObject.SetActive(true);
        _noteManagerPlayer2.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _noteManagerPlayer1.gameObject.SetActive(false);
        _noteManagerPlayer2.gameObject.SetActive(false);
    }

    /// <summary>
    /// ノーツ生成の可否を設定する
    /// </summary>
    /// <param name="value"></param>
    public void SetCanGenerate(bool value)
    {
        _canGenerate = value;
#if UNITY_EDITOR
        Debug.Log($"ノーツ生成状態を{_canGenerate}に設定");
#endif
    }

    public void DeleteAllNotes()
    {
        _noteManagerPlayer1.DeleteAllNotes();
        _noteManagerPlayer2.DeleteAllNotes();
    }

    /// <summary>
    /// 現在番号から順番にノーツを生成する
    /// </summary>
    private void GenerateNoteOrder()
    {
        GameObject notePlayer1 = _noteManagerPlayer1.GenerateNote(_generatePosP1);
        GameObject notePlayer2 = _noteManagerPlayer2.GenerateNote(_generatePosP2);
        notePlayer1.GetComponent<NoteController>().SetButtonNumber(_buttonNumber);
        notePlayer2.GetComponent<NoteController>().SetButtonNumber(_buttonNumber);
        if (_buttonNumber > 8)
        {
            _buttonNumber = 1;
        }
        else
        {
            _buttonNumber++;
        }
    }

    /// <summary>
    /// 指定した回数ノーツを生成する(非同期)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public async UniTask GenerateNoteByCount(int buttonNumber, int endCount, bool isGetNotPossible = false, float stanTime = 0.0f, CancellationToken token = default)
    {
        if (_canGenerate == false) return;

        _buttonNumber = buttonNumber;

        int count = 0;

        while (count <= endCount)
        {
            _time += Time.deltaTime;

            if (_time >= 3.0f)
            {
                _time = 0.0f;
                GameObject notePlayer1 = _noteManagerPlayer1.GenerateNote(_generatePosP1);
                NoteController noteControllerPlayer1 = notePlayer1.GetComponent<NoteController>();
                _noteManagerPlayer1.SetStanTimeSecond(stanTime);
                noteControllerPlayer1.SetButtonNumber(_buttonNumber);
                noteControllerPlayer1.SetIsGetNotPossible(isGetNotPossible);

                GameObject notePlayer2 = _noteManagerPlayer2.GenerateNote(_generatePosP2);
                NoteController noteControllerPlayer2 = notePlayer2.GetComponent<NoteController>();
                _noteManagerPlayer2.SetStanTimeSecond(stanTime);
                noteControllerPlayer2.GetComponent<NoteController>().SetButtonNumber(_buttonNumber);
                noteControllerPlayer2.GetComponent<NoteController>().SetIsGetNotPossible(isGetNotPossible);
                count++;
            }
            await UniTask.Yield(token);
        }
    }

    /// <summary>
    /// 指定した番号まで順番にノーツを生成する(非同期)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public async UniTask GenerateNoteOrderBySpecified(int startValue, int endValue, bool isGetNotPossible = false, float stanTime = 0.0f, CancellationToken token = default)
    {
        if (_canGenerate == false) return;
        _buttonNumber = startValue;
        _time = 3.0f;

        while (_buttonNumber <= endValue)
        {
            _time += Time.deltaTime;

            if (_time >= 3.0f)
            {
                _time = 0.0f;
                GameObject notePlayer1 = _noteManagerPlayer1.GenerateNote(_generatePosP1);
                NoteController noteControllerPlayer1 = notePlayer1.GetComponent<NoteController>();
                _noteManagerPlayer1.SetStanTimeSecond(stanTime);
                noteControllerPlayer1.SetButtonNumber(_buttonNumber);
                noteControllerPlayer1.SetIsGetNotPossible(isGetNotPossible);

                GameObject notePlayer2 = _noteManagerPlayer2.GenerateNote(_generatePosP2);
                NoteController noteControllerPlayer2 = notePlayer2.GetComponent<NoteController>();
                _noteManagerPlayer2.SetStanTimeSecond(stanTime);
                noteControllerPlayer2.GetComponent<NoteController>().SetButtonNumber(_buttonNumber);
                noteControllerPlayer2.GetComponent<NoteController>().SetIsGetNotPossible(isGetNotPossible);
                _buttonNumber++;
            }
            await UniTask.Yield(token);
        }
    }
}
