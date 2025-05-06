using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

namespace BatotteChannel.Notes
{
    /// <summary>NoteControllerの全ての権限を持ったinterface</summary>
    public interface INoteController
    {
        bool IsDeletingDistance { get; }
        bool IsDeletingScheduled { get; }
        JudgementState NoteJudgement { get; }

        void JudgementNote(out JudgementState judgement);
        void JudgementNote(int buttonNumber, out JudgementState judgement);
        void DeleteThisNote(float t, bool isTimeElapsed = false);
    }

    /// <summary>判定のステート</summary>
    public enum JudgementState
    {
        Good,
        MISS
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
        private float _goodJudgmentRange = 0.1f;

        /// <summary>ボディとフレームのサイズ比較</summary>
        private float _distance;

        /// <summary>ボタン番号</summary>
        private int _buttonNumber;

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

        [Header("オブジェクト参照")]
        [Tooltip("フレームのオブジェクトを設定してください"), SerializeField]
        private GameObject _outerFrame;

        [Tooltip("ボディのオブジェクトを設定してください"), SerializeField]
        private GameObject _body;

        [Tooltip("判定を表示するためのオブジェクトを設定してください"), SerializeField]
        private GameObject _judgmentText;

        [Tooltip("ボタン番号を表示するためのオブジェクトを設定してください"), SerializeField]
        private GameObject _buttonNumberText;

        /// <summary>判定表示のテキストを格納</summary>
        private TextMesh _tJudgementText;

        /// <summary>判定表示のテキストを格納</summary>
        private TextMesh _tButtonNumberText;

        #endregion

        #region 関数

        /// <summary>
        /// 初期化する
        /// </summary>
        private void SetUp()
        {
            _outerFrame.transform.localScale = _defaltFrameSize;
            _buttonNumberText.gameObject.GetComponent<MeshRenderer>().sortingOrder = 2;
        }

        /// <summary>
        /// キャッシュする
        /// </summary>
        private void DoCache()
        {
            _tJudgementText = _judgmentText.GetComponent<TextMesh>();
            _tButtonNumberText = _buttonNumberText.GetComponent<TextMesh>();
        }

        /// <summary>
        /// ボタン番号を付与する
        /// </summary>
        private void GiveButtonNumber()
        {
            _buttonNumber = Random.Range(1, 9);
            _tButtonNumberText.text = _buttonNumber.ToString();
#if UNITY_EDITOR
            Debug.Log($"ノーツ番号を付与：{_buttonNumber}");
#endif
        }

        /// <summary>
        /// このノートを隠す
        /// </summary>
        private void HideNote()
        {
            _buttonNumberText.SetActive(false);
            _outerFrame.SetActive(false);
            _body.SetActive(false);
        }

        /// <summary>
        /// ノーツを判定する
        /// </summary>
        /// <param name="buttonNumber">ボタン番号</param>
        /// <param name="judgement">判定結果を返す</param>
        public void JudgementNote(int buttonNumber, out JudgementState judgement)
        {
            if (buttonNumber != _buttonNumber)
            {
                HideNote();
                judgement = JudgementState.MISS;
                DisplayJudgementResult(TEXT_JUDGEMENT_MISS);
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
            DisplayJudgementResult(judgementText);
#if UNITY_EDITOR
            Debug.Log($"ノーツを判定しました(判定結果：{judgement}, 距離：{_distance}, GOODの距離：|{_goodJudgmentRange}|)");
#endif
            DeleteThisNote(1.0f);
        }

        /// <summary>
        /// ノーツを判定する(入力期間を過ぎた場合はこちらを使用)
        /// </summary>
        /// <param name="judgement">判定結果を返す</param>
        public void JudgementNote(out JudgementState judgement)
        {
            HideNote();
            judgement = JudgementState.MISS;
            DisplayJudgementResult(TEXT_JUDGEMENT_MISS);
#if UNITY_EDITOR
            Debug.Log("ノーツを判定しました(判定結果：入力期間を過ぎました)");
#endif
            DeleteThisNote(1.0f);
        }

        /// <summary>
        /// 判定結果を表示する
        /// </summary>
        /// <param name="judgementText">判定結果</param>
        private void DisplayJudgementResult(string judgementText)
        {
            _judgmentText.SetActive(true);
            _tJudgementText.text = judgementText;
#if UNITY_EDITOR
            Debug.Log($"次の判定結果を表示しました：{judgementText}");
#endif
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

        #endregion

        #region イベント関数

        private void Start()
        {
            SetUp();
            DoCache();
            GiveButtonNumber();
        }

        void Update()
        {
            _outerFrame.transform.localScale -= Vector3.one * Time.deltaTime;
            CalculateJudgmentDistance();

            if (_isDeletingDistance) return;
            if (_outerFrame.transform.localScale.x < -0.5f)
            {
                _isDeletingDistance = true;
            }
        }

        #endregion

    }
}
