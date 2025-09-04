using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BatotteChannel.Tutorial
{
    public class PlayerHandler : MonoBehaviour
    {
        //public delegate void

        #region 変数

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

        #endregion

        #region イベント関数

        void Start()
        {
            // 初期設定
            RegisterButton();
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

        }

        /// <summary>プレイヤー操作の取得</summary>
        public void GetPushedButton()
        {
            if (_button1.triggered)
            {

            }
            if (_button2.triggered)
            {

            }
            if (_button3.triggered)
            {

            }
            if (_button4.triggered)
            {

            }
            if (_button5.triggered)
            {

            }
            if (_button6.triggered)
            {

            }
            if (_button7.triggered)
            {

            }
            if (_button8.triggered)
            {

            }
            if (_button9.triggered)
            {

            }
        }

        #endregion
    }
}
