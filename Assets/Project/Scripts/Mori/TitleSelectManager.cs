using UnityEngine;
using NaughtyAttributes;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using BatotteChannel.AudioSystem;
using BatotteChannel.InGame.MusicSystem;
using BatotteChannel.Tutorial;
using BatotteChannel.DataAssets;

public class TitleSelectManager : MonoBehaviour
{
    #region
    /// <summary>
    /// 現在の画面状況、モード
    /// </summary>
    private GameStatus.GameSceneEnum _gameScene = GameStatus.GameSceneEnum.Title;

    /// <summary>
    /// 現在の画面状況、モードのゲッタープロパティ
    /// </summary>
    public GameStatus.GameSceneEnum GameSceneState
    {
        get { return _gameScene; }
    }

    /// <summary>
    /// ボタンを押せる状態かのBool
    /// </summary>
    private bool _canPushButton = false;

    /// <summary>
    /// チュートリアルを再生するかどうか
    /// </summary>
    [SerializeField]
    private bool _isPlayTutorial = false;
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
    /// チュートリアル画面Canvas
    /// </summary>
    [SerializeField, Label("選択画面UICanvas")]
    private Canvas _tutorialCanvas;
    /// <summary>
    /// チュートリアル画面CanvasGroup
    /// </summary>
    private CanvasGroup _tutorialCanvasGroup;
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
    private readonly float _inCameraSize = 2.7f;
    /// <summary>
    /// ズームアウト画面カメラサイズ
    /// </summary>
    private readonly float _outCameraSize = 5.4f;

    [SerializeField, Label("カメラ縮小速度 n秒")]
    private float _camSizeSpeed = 0.3f;

    /// <summary>
    /// CanvasGroupのフェード速度
    /// </summary>
    [SerializeField, Label("Canvasフェード速度 n秒")]
    private float _canvasFadeSpeed = 0.2f;

    #endregion

    void Start()
    {
        //変数初期設定
        _mainCamera = Camera.main;
        _titleCanvasGroup = _titleCanvas.GetComponent<CanvasGroup>();
        _selectCanvasGroup = _selectCanvas.GetComponent<CanvasGroup>();
        _tutorialCanvasGroup = _tutorialCanvas.GetComponent<CanvasGroup>();
        _ingameCanvasGroup = _ingameCanvas.GetComponent<CanvasGroup>();
        _canPushButton = true;
        //タイトル表示からスタート
        _gameScene = GameStatus.GameSceneEnum.Title;
        _mainCamera.orthographicSize = _inCameraSize;
        _titleCanvasGroup.alpha = 1.0f;
        _selectCanvasGroup.alpha = 0f;
        _tutorialCanvasGroup.alpha = 0.0f;
        _ingameCanvasGroup.alpha = 0f;
        // BGM再生
        SoundManager.Instance.SetBgmVolume(0.5f);
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM("be_efficient");
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
            if (_gameScene == GameStatus.GameSceneEnum.Title) return;
            TransitionCanvas(GameStatus.GameSceneEnum.Select, GameStatus.GameSceneEnum.Title);
        }
        //Enterを押されたら遷移
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (_gameScene != GameStatus.GameSceneEnum.Title) return;
            if (_isPlayTutorial)
            {
                TransitionCanvas(GameStatus.GameSceneEnum.Title, GameStatus.GameSceneEnum.Tutorial);
                return;
            }

            TransitionCanvas(GameStatus.GameSceneEnum.Title, GameStatus.GameSceneEnum.Select);
        }
    }

    /// <summary>
    /// タイトルから選択画面遷移関数
    /// </summary>
    public async void ToSelect()
    {
        if (_gameScene != GameStatus.GameSceneEnum.Title) return;
        // 効果音を再生
        SoundManager.Instance.PlaySE("push_determining_button_53");
        //ボタンを押せなくする
        _canPushButton = false;
        //タイトル画面UIを非表示　フェード
        await Fade(_titleCanvasGroup, 0, _canvasFadeSpeed);
        //カメラ動作を入れるならここ
        // 小林：セレクト画面に行く際にカメラ動作を追記
        // await CamSize(_outCameraSize, _camSizeSpeed);
        //終わったら選択画面UIを表示　フェード
        await Fade(_selectCanvasGroup, 1, _canvasFadeSpeed);
        //ボタンを押せるようにする
        _canPushButton = true;
        //ゲームモードを変更
        _gameScene = GameStatus.GameSceneEnum.Select;
        Debug.Log("Compreat:ToSelect");
    }

    /// <summary>
    /// 選択画面からタイトル遷移関数
    /// </summary>
    public async void ToTitle()
    {
        if (_gameScene != GameStatus.GameSceneEnum.Select) return;
        //ボタンを押せなくする
        _canPushButton = false;
        //選択画面UIを非表示　フェード
        await Fade(_selectCanvasGroup, 0, _canvasFadeSpeed);
        //カメラ動作を入れるならここ
        //終わったらタイトルUIを表示　フェード
        await Fade(_titleCanvasGroup, 1, _canvasFadeSpeed);
        //ボタンを押せるようにする
        _canPushButton = true;
        //ゲームモードを変更
        _gameScene = GameStatus.GameSceneEnum.Title;
        Debug.Log("Compreat:ToTitle");
    }

    public async void ToTutorial()
    {
        if (_gameScene != GameStatus.GameSceneEnum.Select) return;
        // 効果音を再生
        SoundManager.Instance.PlaySE("push_determining_button_53");
        //ボタンを押せなくする
        _canPushButton = false;
        //選択画面UIを非表示　フェード
        await Fade(_selectCanvasGroup, 0, _canvasFadeSpeed);
        //カメラ動作を入れるならここ
        // 小林：カメラ動作をズームインに変更
        // await CamSize(_inCameraSize, _camSizeSpeed);
        //終わったらタイトルUIを表示　フェード
        await Fade(_tutorialCanvasGroup, 1, _canvasFadeSpeed);
        //ボタンを押せるようにする
        _canPushButton = true;

        //ゲームモードを変更
        _gameScene = GameStatus.GameSceneEnum.Tutorial;
        Debug.Log("Compreat:ToTutorial");
    }

    //debug
    /// <summary>
    /// ゲーム画面へ遷移
    /// </summary>
    public async void ToGame()
    {
        if (_gameScene != GameStatus.GameSceneEnum.Select) return;
        // 効果音を再生
        SoundManager.Instance.PlaySE("push_determining_button_53");
        //ボタンを押せなくする
        _canPushButton = false;
        //選択画面UIを非表示　フェード
        await Fade(_selectCanvasGroup, 0, _canvasFadeSpeed);
        //カメラ動作を入れるならここ
        // 小林：カメラ動作をズームインに変更
        // await CamSize(_inCameraSize, _camSizeSpeed);
        //終わったらタイトルUIを表示　フェード
        await Fade(_ingameCanvasGroup, 1, _canvasFadeSpeed);
        //ボタンを押せるようにする
        _canPushButton = true;

        //ゲームモードを変更
        _gameScene = GameStatus.GameSceneEnum.Game;
        Debug.Log("Compreat:ToGame");
    }

    public async void ToGameFromTutorial()
    {
        if (_gameScene != GameStatus.GameSceneEnum.Tutorial) return;
        // 効果音を再生
        SoundManager.Instance.PlaySE("push_determining_button_53");
        //ボタンを押せなくする
        _canPushButton = false;
        //選択画面UIを非表示　フェード
        await Fade(_tutorialCanvasGroup, 0, _canvasFadeSpeed);
        //カメラ動作を入れるならここ
        //終わったらタイトルUIを表示　フェード
        await Fade(_ingameCanvasGroup, 1, _canvasFadeSpeed);
        //ボタンを押せるようにする
        _canPushButton = true;

        //ゲームモードを変更
        _gameScene = GameStatus.GameSceneEnum.Game;
        Debug.Log("Compreat:ToGame");
    }

    /// <summary>
    /// カメラサイズを変更する関数
    /// </summary>
    /// <param name="size">変化後のサイズ</param>
    /// <param name="duration">フェード後にかかる時間、秒数</param>
    /// <returns></returns>
    async UniTask CamSize(float size, float duration)
    {
        await DOVirtual.Float(
                from: _mainCamera.orthographicSize,
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
    async UniTask Fade(CanvasGroup canvasGroup, float alpha, float duration)
    {
        await DOVirtual.Float(
                from: canvasGroup.alpha,
                to: alpha,
                duration: duration,
                onVirtualUpdate: (tweenValue) => { canvasGroup.alpha = tweenValue; });
    }

    #region Kobayashi

    /// <summary>楽曲名</summary>
    [SerializeField]
    private string _musicName;

    /// <summary>楽曲管理オブジェクト</summary>
    [SerializeField]
    private MusicManager _musicManager;

    /// <summary>チュートリアル管理オブジェクト</summary>
    [SerializeField]
    private TutorialFlowManager _tutorialManager;

    /// <summary>ストーリーのキャンバス</summary>
    [SerializeField]
    private CanvasGroup _storyCanvas;

    /// <summary>
    /// 画面を遷移させる
    /// </summary>
    /// <param name="fromGameStatus">元(現在)のゲームステート</param>
    /// <param name="toGameStatus">次のゲームステート</param>
    public async void TransitionCanvas(GameStatus.GameSceneEnum fromGameStatus, GameStatus.GameSceneEnum toGameStatus)
    {
        if (_gameScene != fromGameStatus) return;

        // 遷移元と遷移先が同じであればエラーを出力し処理を中断
        if (fromGameStatus == toGameStatus)
        {
#if UNITY_EDITOR
            Debug.LogError("遷移元と遷移先のゲームステートが同じです");
#endif
            return;
        }

        // ゲームステータスが追加されたら、それぞれのSwitch文を追記してください
        // 元(現在)の画面のキャンバスを取得
        CanvasGroup fromCanvas = null;
        switch (fromGameStatus)
        {
            case GameStatus.GameSceneEnum.Title:
                fromCanvas = _titleCanvasGroup;
                break;
            case GameStatus.GameSceneEnum.Select:
                fromCanvas = _selectCanvasGroup;
                SoundManager.Instance?.StopBGM();
                SoundManager.Instance?.PlayBGM("be_efficient");
                break;
            case GameStatus.GameSceneEnum.Tutorial:
                fromCanvas = _tutorialCanvasGroup;
                _tutorialManager.SetCanPlayTutorial(false);
                break;
            case GameStatus.GameSceneEnum.Game:
                fromCanvas = _ingameCanvasGroup;
                break;
            case GameStatus.GameSceneEnum.Story:
                fromCanvas = _storyCanvas;
                break;
            default:
                // ステートが無効な範囲であればエラーを出力し処理を中断
#if UNITY_EDITOR
                Debug.LogError("指定したゲームステートが無効です");
#endif
                return;
        }

        // 次の画面のキャンバスを取得
        CanvasGroup toCanvas = null;
        switch (toGameStatus)
        {
            case GameStatus.GameSceneEnum.Title:
                toCanvas = _titleCanvasGroup;
                break;
            case GameStatus.GameSceneEnum.Select:
                toCanvas = _selectCanvasGroup;
                SoundManager.Instance?.StopBGM();
                SoundManager.Instance?.PlayBGM("Chearful_Fight");
                break;
            case GameStatus.GameSceneEnum.Tutorial:
                toCanvas = _tutorialCanvasGroup;
                _tutorialManager.SetCanPlayTutorial(true);
                break;
            case GameStatus.GameSceneEnum.Game:
                toCanvas = _ingameCanvasGroup;
                break;
            case GameStatus.GameSceneEnum.Story:
                toCanvas = _storyCanvas;
                break;
            default:
                // ステートが無効な範囲であればエラーを出力し処理を中断
#if UNITY_EDITOR
                Debug.LogError("指定したゲームステートが無効です");
#endif
                return;
        }

        // 効果音を再生
        SoundManager.Instance.PlaySE("push_determining_button_53");
        //ボタンを押せなくする
        _canPushButton = false;
        //選択画面UIを非表示　フェード
        await Fade(fromCanvas, 0, _canvasFadeSpeed);
        //カメラ動作を入れるならここ
        //終わったらタイトルUIを表示　フェード
        await Fade(toCanvas, 1, _canvasFadeSpeed);
        //ボタンを押せるようにする
        _canPushButton = true;

        //ゲームモードを変更
        _gameScene = toGameStatus;
        Debug.Log("Compreat:ToGame");
    }

    /// <summary>
    /// 難易度を Kidsにセットしたうえでインゲームへ移行、ボタンへのバインドを想定
    /// </summary>
    public void ToGameKids()
    {
        string musicDataIndex = _musicName + "_KZ";
        MusicScoreDataManager.musicDataBaseDictionary.TryGetValue(musicDataIndex, out var musicData);
        SetUpMusicManager(musicData);

        ToNextSceneFromSelect();
    }

    /// <summary>
    /// 難易度をEasyにセットしたうえでインゲームへ移行、ボタンへのバインドを想定
    /// </summary>
    public void ToGameEasy()
    {
        string musicDataIndex = _musicName + "_EZ";
        MusicScoreDataManager.musicDataBaseDictionary.TryGetValue(musicDataIndex, out var musicData);
        SetUpMusicManager(musicData);

        ToNextSceneFromSelect();
    }

    /// <summary>
    /// 難易度をNomalにセットしたうえでインゲームへ移行、ボタンへのバインドを想定
    /// </summary>
    public void ToGameNomal()
    {
        string musicDataIndex = _musicName + "_NL";
        MusicScoreDataManager.musicDataBaseDictionary.TryGetValue(musicDataIndex, out var musicData);
        SetUpMusicManager(musicData);

        ToNextSceneFromSelect();
    }

    /// <summary>
    /// 難易度をHardにセットしたうえでインゲームへ移行、ボタンへのバインドを想定
    /// </summary>
    public void ToGameHard()
    {
        string musicDataIndex = _musicName + "_HD";
        MusicScoreDataManager.musicDataBaseDictionary.TryGetValue(musicDataIndex, out var musicData);
        SetUpMusicManager(musicData);

        ToNextSceneFromSelect();
    }

    /// <summary>
    /// 楽曲管理オブジェクトをセットアップする
    /// </summary>
    /// <param name="musicData">楽曲データ</param>
    private void SetUpMusicManager(MusicDataScriptableObject musicData)
    {
        _musicManager.SetMusicData(musicData);
        _musicManager.SetGenerateSettingsDB(musicData.musicGenerateSettingDataBase);
        _musicManager.SetCurrentDifficultyState(_musicManager.GetDifficulty(musicData));
    }

    /// <summary>
    /// 選択画面の次の画面へ移行する
    /// </summary>
    /// <remarks>
    /// インゲーム画面の前に何か画面を挟むならこれを編集する
    /// </remarks>
    private void ToNextSceneFromSelect()
    {
        // ストーリー画面へ遷移
        TransitionCanvas(_gameScene, GameStatus.GameSceneEnum.Story);

        //ToGame();
    }

    #endregion
}
