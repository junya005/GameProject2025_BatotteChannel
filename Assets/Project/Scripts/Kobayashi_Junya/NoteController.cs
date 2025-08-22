using System.Collections.Generic;
using UnityEngine;

namespace BatotteChannel.InGame.Notes
{
    /// <summary>判定の種類</summary>
    public enum JudgementState
    {
        None,
        Good,
        MISS
    }

    public enum ENotePlayerState
    {
        Player1,
        Player2
    }

    /// <summary>ノーツを動きに関するクラス</summary>
    public class NoteController : MonoBehaviour, INoteController
    {
        #region 定数

        /// <summary>グッド判定時のテキスト</summary>
        private const string TEXT_JUDGEMENT_GOOD = "GOOD!!";

        /// <summary>ミス判定時のテキスト</summary>
        private const string TEXT_JUDGEMENT_MISS = "MISS...";

        #endregion

        #region 変数

        [Header("パラメーター")]
        [Tooltip("フレームのデフォルトサイズ"), SerializeField]
        private Vector3 _defaltFrameSize = new Vector3(2, 2, 2);

        [Tooltip("GOOD判定の距離"), SerializeField]
        private float _goodJudgmentRange = 0.2f;

        /// <summary>ボディとフレームのサイズ比較</summary>
        private float _distance = 1.0f;

        /// <summary>現在のフレーム比較のゲッタープロパティ</summary>
        public float Distance { get { return _distance; } }

        /// <summary>ボタン番号</summary>
        private int _buttonNumber;

        /// <summary>ボタン番号のゲッタープロパティ</summary>
        public int ButtonNumber { get { return _buttonNumber; } }

        /// <summary>判定のステートのインスタンス</summary>
        private JudgementState _noteJudgement;

        /// <summary>判定ステートのインスタンスのプロパティ</summary>
        public JudgementState NoteJudgement
        {
            get { return _noteJudgement; }
        }

        /// <summary>削除距離に到達したか</summary>
        private bool _isDeletingDistance = false;

        /// <summary>削除距離到達フラグのプロパティ</summary>
        public bool IsDeletingDistance { get { return _isDeletingDistance; } }

        /// <summary>削除予定かどうか</summary>
        private bool _isDeletingScheduled = false;

        /// <summary>削除予定かどうかのプロパティ</summary>
        public bool IsDeletingScheduled { get { return _isDeletingScheduled; } }

        /// <summary>ダミーノートかどうかのフラグ</summary>
        private bool _isDummyNotes = false;

        /// <summary>ダミーノートかどうかのゲッタープロパティ</summary>
        public bool IsDummyNotes { get { return _isDummyNotes; } }

        /// <summary>ボタン番号が付与されたか</summary>
        private bool _isGaveButton = false;

        /// <summary>
        /// 前回のボタン番号
        /// staticに設定することで、オブジェクトの生成を跨いで値を共有しています。
        /// </summary>
        private static int _lastButtonNumForPlayerOne;
        private static int _lastButtonNumForPlayerTwo;

        [Header("オブジェクト参照")]
        [Tooltip("フレームのオブジェクトを設定してください"), SerializeField]
        private GameObject _outerFrame;

        [Tooltip("ボディのオブジェクトを設定してください"), SerializeField]
        private GameObject _body;

        [Tooltip("ボタン番号を表示するためのオブジェクトを設定してください"), SerializeField]
        private GameObject _buttonNumberDisplay;

        [Tooltip("判定表示のためのオブジェクトを格納"), SerializeField]
        private GameObject _judgementDisplay;

        private SpriteRenderer _judgementDisplaySpriteRenderer;

        /// <summary>判定表示のSpriteRendererを格納</summary>
        private SpriteRenderer _buttonNumberSpriteRenderer;

        /// <summary>ボタンが有効になる距離</summary>
        [Tooltip("ボタン入力が有効になる距離"), SerializeField]
        private float _buttonEnableDistance = 0.4f;

        [Header("アセット参照")]
        [Tooltip("ボタン番号の画像を設定"), SerializeField]
        private List<Sprite> _buttonNumImages = new List<Sprite>();

        [Tooltip("Good判定を表示する画像を設定"), SerializeField]
        private Sprite _goodJudgementImage;

        [Tooltip("Miss判定を表示する画像を設定"), SerializeField]
        private Sprite _missJudgementImage;

        [Tooltip("Good判定エフェクトのオブジェクトを格納"), SerializeField]
        private GameObject _goodEffectPrefab;

        [Tooltip("Miss判定エフェクトのオブジェクトを格納"), SerializeField]
        private GameObject _missEffectPrefab;

        [SerializeField]
        private ENotePlayerState _notePlayerState = ENotePlayerState.Player1;

        #endregion

        #region イベント関数

        private void Awake()
        {
            // キャッシュ
            _buttonNumberSpriteRenderer = _buttonNumberDisplay.GetComponent<SpriteRenderer>();
            _judgementDisplaySpriteRenderer = _judgementDisplay.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            // 初期設定
            _outerFrame.transform.localScale = _defaltFrameSize;
            _judgementDisplaySpriteRenderer.sortingOrder = 51;
            GiveButtonNumber();
        }

        void Update()
        {
            _outerFrame.transform.localScale -= Vector3.one * Time.deltaTime;
            CalculateJudgmentDistance();

            if (_isDeletingDistance) return;
            if (_outerFrame.transform.localScale.x < 0.0f)
            {
                _isDeletingDistance = true;
            }
        }

        #endregion

        #region 関数

        /// <summary>
        /// 前回のボタン番号を設定
        /// </summary>
        /// <param name="value"></param>
        public void SetLastButtonNum(int value, ENotePlayerState playerState)
        {
            if (playerState == ENotePlayerState.Player1)
                _lastButtonNumForPlayerOne = value;
            else
                _lastButtonNumForPlayerTwo = value;
        }

        /// <summary>
        /// ダミーノーツに設定する
        /// </summary>
        /// <param name="boolean">ダミーノーツにするかどうか</param>
        public void SetIsDummyNotes(bool boolean)
        {
#if UNITY_EDITOR
            if (boolean) Debug.Log("このノートはダミーノートに設定されました。ダミーノートは入力を受け付けず、判定結果にも影響しません。");
#endif
            _isDummyNotes = boolean;
            if (_isDummyNotes)
            {
                SettingDummyNote();
            }
        }

        /// <summary>
        /// ダミーノーツの設定
        /// </summary>
        private void SettingDummyNote()
        {
            Color halfColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            _body.GetComponent<SpriteRenderer>().color = halfColor;
            _outerFrame.GetComponent<SpriteRenderer>().color = halfColor;
            _buttonNumberDisplay.GetComponent<SpriteRenderer>().color = halfColor;
        }

        /// <summary>
        /// ボタン番号を付与する
        /// </summary>
        private void GiveButtonNumber()
        {
            if (_isGaveButton) return;

            _buttonNumber = Random.Range(1, 10);
            int lastButtonNum = _notePlayerState == ENotePlayerState.Player1 ? _lastButtonNumForPlayerOne : _lastButtonNumForPlayerTwo;
            // ボタン番号が前回生成したノーツと被っていれば再帰する。
            if (_buttonNumber == lastButtonNum)
            {
                GiveButtonNumber();
                return;
            }
            int buttonNumImageIndex = _buttonNumber - 1;
            _buttonNumberSpriteRenderer.sprite = _buttonNumImages[buttonNumImageIndex];
            SetLastButtonNum(_buttonNumber, _notePlayerState);
#if UNITY_EDITOR
            Debug.Log($"ノーツ番号を付与：{_buttonNumber}");
#endif
            _isGaveButton = true;
        }

        /// <summary>
        /// このノートを隠す
        /// </summary>
        private void HideNote()
        {
            _buttonNumberDisplay.SetActive(false);
            _outerFrame.SetActive(false);
            _body.SetActive(false);
        }

        /// <summary>
        /// このノートを判定する
        /// その後、このノートを1秒後に削除する
        /// </summary>
        /// <param name="buttonNumber">ボタン番号</param>
        /// <param name="judgement">判定結果を返す</param>
        public void JudgementNote(int buttonNumber, out JudgementState judgement)
        {
            if (buttonNumber != _buttonNumber)
            {
                HideNote();
                judgement = JudgementState.MISS;
                DisplayJudgementResult(judgement);
#if UNITY_EDITOR
                Debug.Log("ノーツを判定しました(判定結果：ボタンの不一致)");
#endif
                DeleteThisNote(1.0f);
                return;
            }

            HideNote();
            float distance = Mathf.Abs(_distance);
            judgement = distance <= _goodJudgmentRange ? JudgementState.Good : JudgementState.MISS;
            string judgementText = distance <= _goodJudgmentRange ? TEXT_JUDGEMENT_GOOD : TEXT_JUDGEMENT_MISS;
            DisplayJudgementResult(judgement);
#if UNITY_EDITOR
            Debug.Log($"ノーツを判定しました(判定結果：{judgement}, 距離：{_distance}, GOODの距離：{_goodJudgmentRange}[絶対値])");
#endif
            DeleteThisNote(1.0f);
        }

        /// <summary>
        /// このノーツを判定する(入力期間を過ぎた場合はこちらを使用)
        /// </summary>
        /// <param name="judgement">判定結果を返す</param>
        public void JudgementNote(out JudgementState judgement)
        {
            HideNote();
            judgement = JudgementState.MISS;
            DisplayJudgementResult(judgement);
#if UNITY_EDITOR
            Debug.Log("ノーツを判定しました(判定結果：入力期間を過ぎました)");
#endif
            DeleteThisNote(1.0f);
        }

        /// <summary>
        /// 判定結果を表示する
        /// </summary>
        /// <param name="judgementText">判定結果</param>
        private void DisplayJudgementResult(JudgementState judgementState)
        {
            _judgementDisplay.SetActive(true);

#if UNITY_EDITOR
            Debug.Log($"次の判定結果を表示します：{judgementState}");
#endif

            if (judgementState == JudgementState.Good)
            {
                Instantiate(_goodEffectPrefab, transform.position, Quaternion.identity);
                return;
            }

            Instantiate(_missEffectPrefab, transform.position, Quaternion.identity);
        }

        /// <summary>
        /// このノートを削除する
        /// </summary>
        /// <param name="t">削除までの時間</param>
        public void DeleteThisNote(float t, bool isTimeElapsed = false)
        {
            _isDeletingScheduled = true;
            Destroy(this.gameObject, t);
#if UNITY_EDITOR
            Debug.Log($"このノートは{t}秒後に削除されます");
#endif
            return;
        }

        /// <summary>
        /// 判定距離を計算する
        /// </summary>
        private void CalculateJudgmentDistance()
        {
            _distance = _body.transform.localScale.x - _outerFrame.transform.localScale.x;
        }

        /// <summary>
        /// ボタン入力が有効かチェックする、NoteManagerから判定する際は事前にこちらを使用してボタン入力範囲内かチェックを行う。
        /// </summary>
        /// <returns>ボタン入力が有効かどうか</returns>
        public bool CheckButtonEnable()
        {
            if (Mathf.Abs(_distance) >= Mathf.Abs(_buttonEnableDistance))
            {
#if UNITY_EDITOR
                Debug.Log($"ボタン入力が無効な範囲です。距離:{_distance}");
#endif
                return false;
            }
#if UNITY_EDITOR
            Debug.Log($"ボタン入力が有効な範囲です距離:{_distance}");
#endif
            return true;
        }

        public void SetButtonNumber(int buttonNumber)
        {
            _buttonNumber = buttonNumber;
            _buttonNumberSpriteRenderer.sprite = _buttonNumImages[_buttonNumber - 1];
            _isGaveButton = true;
        }

        #endregion

    }
}
