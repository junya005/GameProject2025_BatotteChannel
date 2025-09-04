using UnityEngine;
using UnityEngine.InputSystem;
using BatotteChannel.InGame.Notes;
using System.Collections;
using BatotteChannel.InGame.MusicSystem;
using BatotteChannel.Player;

namespace BatotteChannel.InGame.Players
{
    /// <summary>プレイヤー入力に関するクラス</summary>
    public class PlayerController : MonoBehaviour
    {
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

        [Tooltip("プレイヤーを選択"), SerializeField]
        private PlayerNumberState _playerNumber = PlayerNumberState.One;

        [Tooltip("このプレイヤー用のNoteManagerオブジェクト"), SerializeField]
        private GameObject _noteManagerForThisPlayer;

        /// <summary>_noteManagerForThisPlayerのNoteManagerを格納</summary>
        private NoteManager _noteManager;

        /// <summary>スタン状態フラグ</summary>
        private bool _isStan;

        /// <summary>アニメーションが開始されたか</summary>
        private bool _isAnimationStart;

        /// <summary>
        /// ビートアニメーションを開始するかのフラグ
        /// </summary>
        [SerializeField]
        private bool _isBeatAnimation;

        /// <summary>プレイヤーのグラフィックを格納</summary>
        [SerializeField]
        private SpriteRenderer _playerGraphyic;

        [SerializeField]
        private Sprite _idleSprite;

        [SerializeField]
        private Sprite _remoconSprite;

        [SerializeField]
        private MusicManager _musicManager;

        /// <summary>
        /// アニメーションクラスを設定する
        /// </summary>
        [SerializeField]
        private AnimatorParamSetter _playerAnimatorParamSetter;

        /// <summary>
        /// プレイヤー番号のプロパティ
        /// </summary>
        public PlayerNumberState PlayerNumber
        {
            get { return _playerNumber; }
        }

        [SerializeField]
        private PlayerMissedAnimation _playerMissedAnim;

        #endregion

        #region イベント関数

        void Start()
        {
            // キャッシュ
            _noteManager = _noteManagerForThisPlayer.GetComponent<NoteManager>();

            // 初期設定
            RegisterButton();

            _noteManager.getMissNoteCallBack += PlayMissAnimation;
        }

        void Update()
        {
            PlayerHandle();

            bool isMusicPlaying = (bool)_musicManager?.GetComponent<AudioSource>().isPlaying;
            if (isMusicPlaying && !_isAnimationStart)
            {
                if (_isBeatAnimation)
                {
                    StartBeatAnimation();
                }
                _isAnimationStart = true;
                // リズムに合わせて揺れるアニメーションの開始
                if (_playerAnimatorParamSetter != null)
                    _playerAnimatorParamSetter.SetAnimationBool("isPlaying", true);
            }
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
            // StartCoroutine(RemoconPushAnimation());
            if (_playerAnimatorParamSetter != null)
                _playerAnimatorParamSetter.SetAnimationTrigger("Click");
            _noteManager.JudgeMentNoteFromList(buttonNumber);
        }

        /// <summary>
        /// miss時のアニメーションの再生
        /// </summary>
        private void PlayMissAnimation(PlayerNumberState playerNumber)
        {
            // if (_playerAnimatorParamSetter != null)
            //     _playerAnimatorParamSetter.SetAnimationTrigger("Missed");

            _playerMissedAnim?.StartAnimation();
        }

        /// <summary>プレイヤー操作の取得</summary>
        private void PlayerHandle()
        {
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
        }

        /// <summary>
        /// リモコンを押すアニメーションを実行
        /// </summary>
        /// <returns></returns>
        private IEnumerator RemoconPushAnimation()
        {
            _playerGraphyic.sprite = _remoconSprite;
            yield return new WaitForSeconds(0.5f);
            _playerGraphyic.sprite = _idleSprite;
        }

        /// <summary>
        /// ビートアニメーションを開始
        /// </summary>
        public void StartBeatAnimation()
        {
            CharactorBeatAnim playerCharactorBeatAnim = _playerGraphyic.gameObject.GetComponent<CharactorBeatAnim>();

            if (playerCharactorBeatAnim != null)
                playerCharactorBeatAnim.AnimationStart();
        }

        #endregion
    }
}
