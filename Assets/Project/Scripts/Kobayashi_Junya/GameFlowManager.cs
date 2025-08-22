using UnityEngine;

namespace BatotteChannel.GameManager
{
    /// <summary>ゲームの流れを制御するクラス</summary>
    public class GameFlowManager : MonoBehaviour
    {
        /// <summary>インゲームになったかどうかのフラグ</summary>
        private bool _isInGameState;

        [Tooltip("TitleからTutorialのオブジェクトを設定"), SerializeField]
        private GameObject _titleSelectMenuObj;
        [Tooltip("TitleからTutorialの状態管理オブジェクトを設定"), SerializeField]
        private GameObject _titleSelectManagerObj;
        [Tooltip("InGameのオブジェクトを設定"), SerializeField]
        private GameObject _inGameLogicObj;

        /// <summary>タイトルからセレクト画面のマネージャークラス</summary>
        private TitleSelectManager _titleSelectManager;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            CheckGameState();
        }

        /// <summary>
        /// 初期化を行う
        /// </summary>
        private void Initialize()
        {
            _isInGameState = false;
            _titleSelectMenuObj.SetActive(true);
            _inGameLogicObj.SetActive(false);

            _titleSelectManager = _titleSelectManagerObj.GetComponent<TitleSelectManager>();
        }

        /// <summary>
        /// ゲームステートをチェックし、対応する処理を行う関数
        /// </summary>
        private void CheckGameState()
        {
            if (_titleSelectManager.GameSceneState == GameStatus.GameSceneEnum.Game
                                                    && _isInGameState == false)
            {
                ChangeToInGame();
            }
        }

        /// <summary>
        /// インゲームへ切り替える関数
        /// </summary>
        private void ChangeToInGame()
        {
            _titleSelectMenuObj.SetActive(false);
            _inGameLogicObj.SetActive(true);
            _isInGameState = true;
        }
    }
}
