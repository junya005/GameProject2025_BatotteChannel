using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BatotteChannel.Tutorial
{
    /// <summary>チュートリアル用のプレイヤーマネージャー</summary>
    public class TutorialPlayerManager : MonoBehaviour
    {
        public delegate void SkipTutorialEvent();
        public event SkipTutorialEvent _skipTutorialEventHandler;

        private bool _canPlayerPressEnter = false;

        public bool CanPlayerPressEnter { get { return _canPlayerPressEnter; } }

        [SerializeField]
        private Player _player1;

        [SerializeField]
        private Player _player2;

        bool _player1EnterPressed = false;
        bool _player2EnterPressed = false;

        [SerializeField]
        private Image _player1PushImage;
        [SerializeField]
        private Image _player1OkImage;

        [SerializeField]
        private Image _player2PushImage;
        [SerializeField]
        private Image _player2OkImage;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Initialize()
        {
            _player1._enterPressedEventHandler += OnPlayerEnterPressed;
            _player2._enterPressedEventHandler += OnPlayerEnterPressed;

            ResetImages();
        }

        public void ResetImages()
        {
            Debug.Log("イメージをリセットします。");

            _player1PushImage.gameObject.SetActive(true);
            _player1OkImage.gameObject.SetActive(false);
            _player2PushImage.gameObject.SetActive(true);
            _player2OkImage.gameObject.SetActive(false);
        }

        public void SetCanPlayerPressEnter(bool value)
        {
            _canPlayerPressEnter = value;
        }

        void OnDisable()
        {
            _player1._enterPressedEventHandler -= OnPlayerEnterPressed;
            _player2._enterPressedEventHandler -= OnPlayerEnterPressed;
        }

        public void SetCanPlayersInput(bool value)
        {
            _player1.SetCanPlayerInput(value);
            _player2.SetCanPlayerInput(value);
        }

        public void ResetCanPlayersEnterPressed()
        {
            _player1EnterPressed = false;
            _player2EnterPressed = false;
        }

        private void OnPlayerEnterPressed(PlayerNumberState playerNumber)
        {
            if (_canPlayerPressEnter == false) return;

            if (playerNumber == PlayerNumberState.One)
            {
                _player1EnterPressed = true;
                _player1PushImage.gameObject.SetActive(false);
                _player1OkImage.gameObject.SetActive(true);
            }
            else
            {
                _player2EnterPressed = true;
                _player2PushImage.gameObject.SetActive(false);
                _player2OkImage.gameObject.SetActive(true);
            }

            if (_player1EnterPressed == true && _player2EnterPressed == true)
            {
                _skipTutorialEventHandler?.Invoke();
            }
        }
    }
}
