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

        public void Initialize()
        {
            _player1._enterPressedEventHandler += OnPlayerEnterPressed;
            _player2._enterPressedEventHandler += OnPlayerEnterPressed;

            _player1PushImage.gameObject.SetActive(true);
            _player1OkImage.gameObject.SetActive(false);
            _player2PushImage.gameObject.SetActive(true);
            _player2OkImage.gameObject.SetActive(false);
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
