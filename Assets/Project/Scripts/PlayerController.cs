using UnityEngine;
using BatotteChannel.Notes;

namespace BatotteChannel.Players
{
    /// <summary>プレイヤー入力に関するクラス</summary>
    public class PlayerController : MonoBehaviour
    {
        [Tooltip("プレイヤーの番号を入力(0から)"), SerializeField]
        private int _playerNumber = 0;

        [Tooltip("このプレイヤー用のNoteManagerオブジェクト"), SerializeField]
        private GameObject _noteManagerForThisPlayer;

        /// <summary>_noteManagerForThisPlayerのNoteManagerを格納</summary>
        private NoteManager _noteManager;

        /// <summary>スタン状態フラグ</summary>
        private bool _isStan;

        /// <summary>
        /// プレイヤー番号のプロパティ
        /// </summary>
        public int PlayerNumber
        {
            get { return _playerNumber; }
        }

        public void SetPlayerNumber(int playerNumber)
        {
            _playerNumber = playerNumber;
            Debug.Log($"プレイヤー番号をセット：{_playerNumber} (引数：{playerNumber})");
        }

        private void GetNote(int buttonNumber)
        {
            _noteManager.JudgeMentNoteFromList(buttonNumber);
        }

        private void PlayerHandle()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                _noteManager.JudgeMentNoteFromList(1);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                _noteManager.JudgeMentNoteFromList(2);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                _noteManager.JudgeMentNoteFromList(3);
            }
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                _noteManager.JudgeMentNoteFromList(4);
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                _noteManager.JudgeMentNoteFromList(5);
            }
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                _noteManager.JudgeMentNoteFromList(6);
            }
            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                _noteManager.JudgeMentNoteFromList(7);
            }
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                _noteManager.JudgeMentNoteFromList(8);
            }
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                _noteManager.JudgeMentNoteFromList(9);
            }
        }

        void Start()
        {
            _noteManager = _noteManagerForThisPlayer.GetComponent<NoteManager>();
        }

        void Update()
        {
            PlayerHandle();
        }
    }
}
