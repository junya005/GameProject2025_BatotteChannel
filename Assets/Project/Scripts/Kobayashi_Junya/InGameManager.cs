using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using BatotteChannel.InGame.MusicSystem;
using BatotteChannel.InGame.Animation;
using BatotteChannel.GameState;
using BatotteChannel.AudioSystem;
using System;
using UnityEngine.EventSystems;

namespace BatotteChannel.GameManager
{
    /// <summary>InGame部分の管理を行うクラス</summary>
    public class InGameManager : MonoBehaviour
    {
        #region 変数

        [Header("オブジェクト参照")]
        [Tooltip("メインカメラのAnimationZoomCameraを設定"), SerializeField]
        private AnimationZoomCamera _zoomCamera;

        [Tooltip("MusicManagerコンポーネントがついたオブジェクトを設定"), SerializeField]
        private MusicManager _musicManager;

        /// <summary>InGameの終了処理が開始されたか</summary>
        private bool _isEndInGameProcessing = false;

        #endregion

        #region async関数

        /// <summary>
        /// ゲーム開始時のアニメーション処理
        /// </summary>
        private async void StartInGameAnimation()
        {
            await InGameAnimation();
        }

        private async UniTask InGameAnimation()
        {
            // カメラ演出
            await UniTask.Delay(500);
            await _zoomCamera.ZoomCamera(new Vector3(0.0f, 0.75f, -10.0f), new Vector3(0.0f, 0.0f, -10.0f),
                                            2.7f, 5.4f,
                                            1.0f);
            await UniTask.Delay(100);

            // リズムゲームパートの開始
            InGameStateManager.Instance.SetCurrentInGameState(InGameState.PlayMusic);

            // ノーツ生成
            _musicManager.SetIsPlaying(true);
            _musicManager.BeatCounterInstance.SetBeat(0);

            // 遅れて音楽を再生することで、ノーツのタイミングを合わせる
            await UniTask.Delay(1300);

            // 音楽再生
            _musicManager.StartMusic();

        }

        /// <summary>
        /// ゲーム終了時のアニメーション処理
        /// </summary>
        async void EndInGameAnimation()
        {
            // カメラ演出
            await _zoomCamera.ZoomCamera(new Vector3(0.0f, 0.0f, -10.0f), new Vector3(0.0f, 0.75f, -10.0f),
                                            5.4f, 2.7f,
                                            2.0f);
            await UniTask.Delay(100);

            // シーンをリザルトへ遷移
            SceneManager.LoadScene("Result");
        }

        #endregion

        #region イベント関数

        private void Start()
        {
            GameSettingManager.Instance.SetAppFrameRateLimit(GameSettingManager.EnumFrameRateLimitState.Sixty);
            SoundManager.Instance.StopBGM();
            StartInGameAnimation();
        }

        void Update()
        {
            if (_isEndInGameProcessing) return;
            if (InGameStateManager.Instance.CurrentInGameState == InGameState.End)
            {
                _isEndInGameProcessing = true;
                EndInGameAnimation();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GrobalUIManager.Instance.IsModal) return;

                // ポーズ状態に変更
                Time.timeScale = 0f;
                _musicManager.PauseMusic();

                string message = "ゲームを終了しますか？";

                Action onYes = () =>
                {
                    Time.timeScale = 1.0f;
                    SceneManager.LoadScene("Main");
                };

                Action onNo = () =>
                {
                    _musicManager.UnPauseMusic();
                    Time.timeScale = 1.0f;
                };

                GrobalUIManager.Instance.ShowModal(message, onYes, onNo);

                Debug.Log(GrobalUIManager.Instance.ModalDefaultButton.gameObject);
                EventSystem.current.SetSelectedGameObject(GrobalUIManager.Instance.ModalDefaultButton.gameObject);
            }
        }

        #endregion

    }
}
