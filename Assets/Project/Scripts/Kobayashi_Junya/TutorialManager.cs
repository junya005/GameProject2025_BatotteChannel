using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BatotteChannel.InGame.Animation;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// チュートリアルの管理クラス
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private AnimationZoomCamera _animationZoomCamera;

    private ETutorialState tutorialState;

    // Start is called before the first frame update
    void Start()
    {
        Inisialize();
        StartProcess();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 初期化する
    /// </summary>
    private void Inisialize()
    {
        GameSettingManager.Instance.SetAppFrameRateLimit(GameSettingManager.EnumFrameRateLimitState.Sixty);
    }

    /// <summary>
    /// 開始処理
    /// </summary>
    private async void StartProcess()
    {
        await StartCameraAnimation();
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    private async void EndProcess()
    {
        await EndCameraAnimation();
    }

    /// <summary>
    /// 開始時のカメラアニメーション
    /// </summary>
    /// <returns></returns>
    [Button]
    private async UniTask StartCameraAnimation()
    {
        // カメラ演出
        await UniTask.Delay(500);
        await _animationZoomCamera.ZoomCamera(new Vector3(0.0f, 0.75f, -10.0f), new Vector3(0.0f, 0.0f, -10.0f),
                                        2.7f, 5.4f,
                                        1.0f);
        await UniTask.Delay(100);
    }

    /// <summary>
    /// 終了時のカメラアニメーション
    /// </summary>
    /// <returns></returns>
    [Button]
    private async UniTask EndCameraAnimation()
    {
        // カメラ演出
        await UniTask.Delay(500);
        await _animationZoomCamera.ZoomCamera(new Vector3(0.0f, 0.0f, -10.0f), new Vector3(0.0f, 0.75f, -10.0f),
                                        5.4f, 2.7f,
                                        1.0f);
        await UniTask.Delay(100);
    }
}
