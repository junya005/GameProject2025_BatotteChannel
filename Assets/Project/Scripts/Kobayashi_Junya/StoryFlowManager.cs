using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>ストーリー画面の流れを管理するクラス</summary>
public class StoryFlowManager : MonoBehaviour
{
    /// <summary>インゲームまでの画面マネージャー</summary>
    [SerializeField]
    private TitleSelectManager _titleSelectManager;

    /// <summary>ストーリーのアニメーションマネージャー</summary>
    [SerializeField]
    private StoryAnimManager _storyAnimManager;

    /// <summary>ボタンが押下状態UIマネージャー</summary>
    [SerializeField]
    private StoryPressUI _storyPressUI;

    /// <summary>アニメーションが再生されたかのフラグ</summary>
    private bool _isPlayAnim = false;

    /// <summary>アニメーションがスキップされたかのフラグ</summary>
    private bool _isSkipAnim = false;

    // PlayerがEnterを押したかのフラグを定義
    private bool _isPressedEnterP1 = false;
    private bool _isPressedEnterP2 = false;

    void Start()
    {
        _storyAnimManager.TweenReset();
        _storyAnimManager.completeAnimEventHandler += OnCompleteAnimation;
    }

    void Update()
    {
        // 条件に当てはまらなければ処理しない
        if (_titleSelectManager.GameSceneState != GameStatus.GameSceneEnum.Story) return;

        if (_isPlayAnim)
        {
            // 一旦Input.GetKeyで記述
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                _isPressedEnterP1 = true;
                _storyPressUI.DisplayPress(PlayerNumberState.One);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _isPressedEnterP2 = true;
                _storyPressUI.DisplayPress(PlayerNumberState.Two);
            }

            if (_isSkipAnim) return;
            if (_isPressedEnterP1 && _isPressedEnterP2)
            {
                _storyAnimManager.TweenSkip();
                _isSkipAnim = true;
            }

            return;
        }

        // アニメーション処理を開始
        _storyAnimManager.TweenAnimation();
        _isPlayAnim = true;
    }

    void OnDestroy()
    {
        _storyAnimManager.completeAnimEventHandler -= OnCompleteAnimation;
    }

    /// <summary>
    /// アニメーション終了時イベントにバインド
    /// </summary>
    private void OnCompleteAnimation()
    {
        // 画面遷移を実行
        _titleSelectManager.TransitionCanvas(GameStatus.GameSceneEnum.Story, GameStatus.GameSceneEnum.Game);
    }
}
