using UnityEngine;

namespace BatotteChannel.GameManager
{
    public class GameFlowManager : MonoBehaviour
    {
        private bool _isInGameState;

        [Tooltip("TitleからTutorialのオブジェクトを設定"), SerializeField]
        private GameObject _titleSelectMenuObj;
        [Tooltip("TitleからTutorialの状態管理オブジェクトを設定"), SerializeField]
        private GameObject _titleSelectManagerObj;
        [Tooltip("InGameのオブジェクトを設定"), SerializeField]
        private GameObject _inGameLogicObj;

        private TitleSelectManager _titleSelectManager;

        // Start is called before the first frame update
        void Start()
        {
            _isInGameState = false;
            _titleSelectMenuObj.SetActive(true);
            _inGameLogicObj.SetActive(false);

            _titleSelectManager = _titleSelectManagerObj.GetComponent<TitleSelectManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_titleSelectManager.GameSceneState == GameStatus.GameSceneEnum.Game
                                                    && _isInGameState == false)
            {
                _titleSelectMenuObj.SetActive(false);
                _inGameLogicObj.SetActive(true);
                _isInGameState = true;
            }
        }
    }
}
