using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

/// <summary>チュートリアルの流れを制御するクラス</summary>
public class TutorialFlowManager : MonoBehaviour
{
    [SerializeField, Label("タイトル&セレクト画面のマネージャー")]
    private TitleSelectManager _titleSelectManager;

    /// <summary>
    /// ゲームステートがTutorialになったかのフラグ
    /// </summary>
    private bool _isGameStateTutorial;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTutrorialState();
    }

    /// <summary>
    /// 初期化する
    /// </summary>
    private void Initialize()
    {
        _isGameStateTutorial = false;
    }

    /// <summary>
    /// ゲーム状態がチュートリアルになったかチェックする
    /// </summary>
    private async void CheckTutrorialState()
    {
        if (_isGameStateTutorial == true) return;
        if (_titleSelectManager.GameSceneState != GameStatus.GameSceneEnum.Tutorial) return;

        _isGameStateTutorial = true;
        await PlayTutorialAnimation();
    }

    /// <summary>
    /// チュートリアルを再生する
    /// </summary>
    public async UniTask PlayTutorialAnimation()
    {
        Debug.Log("チュートリアルアニメーションを開始");
        // 仮置き
        await UniTask.WaitForSeconds(2.0f);

        _titleSelectManager.ToGameFromTutorial();
    }
}
