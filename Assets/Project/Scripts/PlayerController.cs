using UnityEngine;
using Notes;

namespace Players
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
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {

            }
        }

        void Start()
        {
            _noteManager = _noteManagerForThisPlayer.GetComponent<NoteManager>();
        }

        void Update()
        {

        }
    }
}
