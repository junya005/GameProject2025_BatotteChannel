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
    /// 現在の画面状況、モード
    /// </summary>
    public enum GameSceneEnum
    {
        Title,
        Select,
        Tutorial,
        Game,
        //Result 別シーン
    }
    private GameSceneEnum _gameScene = GameSceneEnum.Title;

    /// <summary>
    /// ボタンを押せる状態かのBool
    /// </summary>
    private bool _canPushButton = false;
    /// <summary>
    /// メインカメラオブジェクト
    /// </summary>
    private Camera _mainCamera;

    /// <summary>
    /// タイトル画面Canvas
    /// </summary>
    [SerializeField, Label("タイトル画面UICanvas")]
    private Canvas _titleCanvas;
    /// <summary>
    /// タイトル画面CanvasGroup
    /// </summary>
    private CanvasGroup _titleCanvasGroup;
    /// <summary>
    /// 難易度選択画面Canvas
    /// </summary>
    [SerializeField, Label("選択画面UICanvas")]
    private Canvas _selectCanvas;
    /// <summary>
    /// 難易度選択画面CanvasGroup
    /// </summary>
    private CanvasGroup _selectCanvasGroup;
    /// <summary>
    /// ゲーム画面UICanvas
    /// </summary>
    [SerializeField, Label("ゲーム画面UICanvas")]
    private Canvas _ingameCanvas;
    /// <summary>
    /// ゲーム画面UICanvasGroup
    /// </summary>
    private CanvasGroup _ingameCanvasGroup;

    /// <summary>
    /// ズームイン画面カメラサイズ
    /// </summary>
    private readonly float _inCameraSize = 2.5f;
    /// <summary>
    /// ズームアウト画面カメラサイズ
    /// </summary>
    private readonly float _outCameraSize = 5f;

    [SerializeField, Label("カメラ縮小速度 n秒")]
    private float _camSizeSpeed = 0.3f;

    /// <summary>
    /// CanvasGroupのフェード速度
    /// </summary>
    [SerializeField,Label("Canvasフェード速度 n秒")]
    private float _canvasFadeSpeed = 0.2f;

    #endregion

    void Start()
    {
        //変数初期設定
        _mainCamera = Camera.main;
        _titleCanvasGroup = _titleCanvas.GetComponent<CanvasGroup>();
        _selectCanvasGroup = _selectCanvas.GetComponent<CanvasGroup>();
        _ingameCanvasGroup = _ingameCanvas.GetComponent<CanvasGroup>();
        _canPushButton = true;
        //タイトル表示からスタート
        _gameScene = GameSceneEnum.Title;
        _mainCamera.orthographicSize = _inCameraSize;
        _titleCanvasGroup.alpha = 1.0f;
        _selectCanvasGroup.alpha = 0f;
        _ingameCanvasGroup.alpha = 0f;
    }

    /// <summary>
    /// 主に入力受付とその条件
    /// </summary>
    void Update()
    {
        //ボタンを押せない状態なら返す
        if (!_canPushButton) return;
        //Esc押されたらタイトルへ戻る
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //スタート状態なら返す
            if (_gameScene==GameSceneEnum.Title) return;
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
        if (_gameScene != GameSceneEnum.Title) return;
        //ボタンを押せなくする
        _canPushButton = false;
        //タイトル画面UIを非表示　フェード
        await Fade(_titleCanvasGroup,0, _canvasFadeSpeed);
        //カメラ動作を入れるならここ
        //終わったら選択画面UIを表示　フェード
        await Fade(_selectCanvasGroup,1, _canvasFadeSpeed);
        //ボタンを押せるようにする
        _canPushButton = true;
        //ゲームモードを変更
        _gameScene = GameSceneEnum.Select;
        Debug.Log("Compreat:ToSelect");
    }

    /// <summary>
    /// 選択画面からタイトル遷移関数
    /// </summary>
    public async void ToTitle()
    {
        if(_gameScene!=GameSceneEnum.Select) return;
        //ボタンを押せなくする
        _canPushButton = false;
        //選択画面UIを非表示　フェード
        await Fade(_selectCanvasGroup,0, _canvasFadeSpeed);
        //カメラ動作を入れるならここ
        //終わったらタイトルUIを表示　フェード
        await Fade(_titleCanvasGroup, 1, _canvasFadeSpeed);
        //ボタンを押せるようにする
        _canPushButton = true;
        //ゲームモードを変更
        _gameScene = GameSceneEnum.Title;
        Debug.Log("Compreat:ToTitle");
    }

    //debug
    /// <summary>
    /// ゲーム画面へ遷移
    /// </summary>
    public async void ToGame()
    {
        if (_gameScene!=GameSceneEnum.Select) return;
        //ボタンを押せなくする
        _canPushButton = false;
        //選択画面UIを非表示　フェード
        await Fade(_selectCanvasGroup, 0, _canvasFadeSpeed);
        //カメラ動作を入れるならここ
        await CamSize(_outCameraSize, _camSizeSpeed);
        //終わったらタイトルUIを表示　フェード
        await Fade(_ingameCanvasGroup, 1, _canvasFadeSpeed);
        //ボタンを押せるようにする
        _canPushButton = true;
        //ゲームモードを変更
        _gameScene = GameSceneEnum.Game;
        Debug.Log("Compreat:ToGame");
    }

    /// <summary>
    /// カメラサイズを変更する関数
    /// </summary>
    /// <param name="size">変化後のサイズ</param>
    /// <param name="duration">フェード後にかかる時間、秒数</param>
    /// <returns></returns>
    async UniTask CamSize(float size,float duration)
    {
        await DOVirtual.Float(from: _mainCamera.orthographicSize,
                        to: size,
                        duration: duration,
                        onVirtualUpdate: (tweenValue) => { _mainCamera.orthographicSize = tweenValue; });
    }

    /// <summary>
    /// Canvasをまとめてフェードさせる関数
    /// </summary>
    /// <param name="canvasGroup">フェードさせるCanvasGroup</param>
    /// <param name="alpha">フェード後のalphaの値</param>
    /// <param name="duration">フェード後にかかる時間、秒数</param>
    /// <returns></returns>
    async UniTask Fade(CanvasGroup canvasGroup, float alpha,float duration)
    {
        await DOVirtual.Float(from: canvasGroup.alpha,
                to: alpha,
                duration: duration,
                onVirtualUpdate: (tweenValue) => { canvasGroup.alpha = tweenValue; });
    }
}
