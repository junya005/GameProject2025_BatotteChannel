using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Drawing;
using UnityEditor.ShaderKeywordFilter;

public class TitleSelectManager : MonoBehaviour
{
    #region 変数定義
    /// <summary>
    /// タイトル状態かのBool
    /// </summary>
    private bool _isStart = false;
    /// <summary>
    /// ボタンを押せる状態かのBool
    /// </summary>
    private bool _canPushButton = false;
    /// <summary>
    /// メインカメラオブジェクト
    /// </summary>
    private Camera _mainCamera;

    [SerializeField, Label("タイトル画面UICanvas")]
    private Canvas _titleCanvas;
    private CanvasGroup _titleCanvasGroup;
    [SerializeField, Label("選択画面UICanvas")]
    private Canvas _selectCanvas;
    private CanvasGroup _selectCanvasGroup;

    /* カメラサイズ使用時
    /// <summary>
    /// タイトル画面カメラサイズ
    /// </summary>
    private readonly float _titleCameraSize = 2.5f;
    /// <summary>
    /// 選択画面カメラサイズ
    /// </summary>
    private readonly float _selectCameraSize = 2.5f;

        [SerializeField,Label("カメラ縮小速度 n秒")]
    private float _camSizeSpeed = 0.3f;
    */
    [SerializeField,Label("Canvasフェード速度 n秒")]
    private float _canvasFadeSpeed = 0.2f;

    #endregion

    void Start()
    {
        _mainCamera = Camera.main;
        _titleCanvasGroup = _titleCanvas.GetComponent<CanvasGroup>();
        _selectCanvasGroup = _selectCanvas.GetComponent<CanvasGroup>();
        _canPushButton = true;
        _isStart = true;
        //タイトルからスタート
        _isStart = true;
        _titleCanvasGroup.alpha = 1.0f;
        _selectCanvasGroup.alpha = 0f;
    }

    void Update()
    {
        //ボタンを押せない状態なら返す
        if (!_canPushButton) return;
        //Esc押されたらタイトルへ戻る
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //スタート状態なら返す
            if (_isStart) return;
            ToTitle();
        }
        //何かキーを押されたら遷移
        else if (Input.anyKeyDown)
        {
            ToSelect();
        }
    }

    /// <summary>
    /// タイトルから選択画面遷移関数
    /// </summary>
    public async void ToSelect()
    {
        //ボタンを押せなくする
        _canPushButton = false;
        //タイトル状態では無くす
        _isStart = false;
        //タイトル画面UIを非表示　フェード
        await Fade(_titleCanvasGroup,0, _canvasFadeSpeed);
        //終わったら選択画面UIを表示　フェード
        await Fade(_selectCanvasGroup,1, _canvasFadeSpeed);
        //ボタンを押せるようにする
        _canPushButton = true;
        Debug.Log("Compreat:ToSelect");
    }

    /// <summary>
    /// 選択画面からタイトル遷移関数
    /// </summary>
    public async void ToTitle()
    {
        //ボタンを押せなくする
        _canPushButton = false;
        //タイトル状態にする
        _isStart = true;
        //選択画面UIを非表示　フェード
        await Fade(_selectCanvasGroup,0, _canvasFadeSpeed);
        //終わったらタイトルUIを表示　フェード
        await Fade(_titleCanvasGroup, 1, _canvasFadeSpeed);
        //ボタンを押せるようにする
        _canPushButton = true;
        Debug.Log("Compreat:ToTitle");
    }

    /// <summary>
    /// カメラサイズ変更
    /// </summary>
    /// <param name="size">変化後のサイズ</param>
    /// <returns></returns>
    async UniTask CamSize(float size,float duration)
    {
        await DOVirtual.Float(from: _mainCamera.orthographicSize,
                        to: size,
                        duration: duration,
                        onVirtualUpdate: (tweenValue) => { _mainCamera.orthographicSize = tweenValue; });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="canvasGroup">フェードさせるCanvasGroup</param>
    /// <param name="alpha">フェード後のalphaの値</param>
    /// <returns></returns>
    async UniTask Fade(CanvasGroup canvasGroup, float alpha,float duration)
    {
        await DOVirtual.Float(from: canvasGroup.alpha,
                to: alpha,
                duration: duration,
                onVirtualUpdate: (tweenValue) => { canvasGroup.alpha = tweenValue; });
    }
}
