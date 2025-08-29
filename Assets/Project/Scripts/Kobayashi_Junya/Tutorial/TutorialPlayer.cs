using UnityEngine;
using UnityEngine.InputSystem;
using BatotteChannel.InGame.Notes;
using System.Collections;
using BatotteChannel.InGame.MusicSystem;
using BatotteChannel.Player;
using NaughtyAttributes;

namespace BatotteChannel.Tutorial
{
    /// <summary>チュートリアルのプレイヤー入力に関するクラス</summary>
    public class Player : MonoBehaviour
    {
        #region 変数

        public delegate void EnterPressedEvent(PlayerNumberState playerNumber);
        public event EnterPressedEvent _enterPressedEventHandler;

        //inputActionの格納
        private PlayerInput _playerInput;
        private InputAction _button0;
        private InputAction _button1;
        private InputAction _button2;
        private InputAction _button3;
        private InputAction _button4;
        private InputAction _button5;
        private InputAction _button6;
        private InputAction _button7;
        private InputAction _button8;
        private InputAction _button9;
        private InputAction _volumePlus;
        private InputAction _volumeMinus;
        private InputAction _buttonEnter;

        [Tooltip("プレイヤーを選択"), SerializeField]
        private PlayerNumberState _playerNumber = PlayerNumberState.One;

        [Tooltip("このプレイヤー用のNoteManagerオブジェクト"), SerializeField]
        private NoteManager _noteManager;

        /// <summary>
        /// プレイヤー番号のプロパティ
        /// </summary>
        public PlayerNumberState PlayerNumber
        {
            get { return _playerNumber; }
        }

        private bool _canPlayerInput = false;

        #endregion

        #region イベント関数

        void Start()
        {
            // 初期設定
            RegisterButton();
        }

        void Update()
        {
            PlayerHandle();
        }

        #endregion

        #region 関数

        /// <summary>
        /// InputActionのボタンを登録する
        /// </summary>
        private void RegisterButton()
        {
            if (_playerInput == null) _playerInput = GetComponent<PlayerInput>();

            _button0 = _playerInput.actions["Button0"];
            _button1 = _playerInput.actions["Button1"];
            _button2 = _playerInput.actions["Button2"];
            _button3 = _playerInput.actions["Button3"];
            _button4 = _playerInput.actions["Button4"];
            _button5 = _playerInput.actions["Button5"];
            _button6 = _playerInput.actions["Button6"];
            _button7 = _playerInput.actions["Button7"];
            _button8 = _playerInput.actions["Button8"];
            _button9 = _playerInput.actions["Button9"];
            _volumePlus = _playerInput.actions["VolumePlus"];
            _volumeMinus = _playerInput.actions["VolumeMinus"];
            _buttonEnter = _playerInput.actions["ButtonEnter"];
        }

        /// <summary>
        /// プレイヤー番号を設定する
        /// </summary>
        /// <param name="playerNumber"></param>
        public void SetPlayerNumber(PlayerNumberState playerNumber)
        {
            _playerNumber = playerNumber;
            Debug.Log($"プレイヤー番号をセット：{_playerNumber} (引数：{playerNumber})");
        }

        /// <summary>
        /// 入力されたボタンに応じてNoteManagerにノーツ判定を指示させる
        /// </summary>
        /// <param name="buttonNumber">ボタン番号</param>
        private void GetNote(int buttonNumber)
        {
            if (buttonNumber == 0) return;
            _noteManager.JudgeMentNoteFromList(buttonNumber);
        }

        /// <summary>プレイヤー操作の取得</summary>
        private void PlayerHandle()
        {
            if (!_canPlayerInput) return;

            if (_button1.triggered)
            {
                GetNote(1);
            }
            if (_button2.triggered)
            {
                GetNote(2);
            }
            if (_button3.triggered)
            {
                GetNote(3);
            }
            if (_button4.triggered)
            {
                GetNote(4);
            }
            if (_button5.triggered)
            {
                GetNote(5);
            }
            if (_button6.triggered)
            {
                GetNote(6);
            }
            if (_button7.triggered)
            {
                GetNote(7);
            }
            if (_button8.triggered)
            {
                GetNote(8);
            }
            if (_button9.triggered)
            {
                GetNote(9);
            }

            if (_buttonEnter.triggered)
            {
                _enterPressedEventHandler?.Invoke(_playerNumber);
            }
        }

        public void SetCanPlayerInput(bool value)
        {
            _canPlayerInput = value;
#if UNITY_EDITOR
            Debug.Log($"プレイヤー{_playerNumber}の入力を{_canPlayerInput}に設定");
#endif
        }

        #endregion
    }
}

