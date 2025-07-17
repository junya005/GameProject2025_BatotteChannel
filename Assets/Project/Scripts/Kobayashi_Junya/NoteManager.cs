using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BatotteChannel.AudioSystem;
using BatotteChannel.InGame.Players;
using BatotteChannel.InGame.UI;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

namespace BatotteChannel.InGame.Notes
{
    /// <summary>
    /// ノーツが取得されたときのコールバック
    /// </summary>
    /// <param name="playerNumber">プレイヤー番号を取得できる</param>
    /// <returns>判定結果</returns>
    public delegate void GetGoodNoteCallBack(PlayerNumberState playerNumber);

    /// <summary>
    /// ノーツの管理に関するクラス
    /// PlayerControllがついたオブジェクトを親にする必要がある
    /// </summary>
    public class NoteManager : MonoBehaviour
    {
        #region 変数

        public GetGoodNoteCallBack getNoteCallBack;

        [SerializeField]
        private bool isAutoPlayMode = false;

        [SerializeField] private TelevisionAnimation _televisionAnimation;

        /// <summary>生成されたノートを格納</summary>
        [SerializeField] private List<GameObject> _generateNotes;

        /// <summary>生成されたダミーノーツを格納</summary>
        [SerializeField] private List<GameObject> _generatedDummyNotes;

        /// <summary>クローン元となるノートのプレファブ</summary>
        [SerializeField] private GameObject _notePrefab;

        /// <summary>クローン元となるダミーノートのプレファブ</summary>
        [SerializeField] private GameObject _dummyNotePrefab;

        /// <summary>ノーツ生成停止フラグ</summary>
        [SerializeField] private bool _isStopNoteGenerate = false;

        /// <summary>ノーツ生成停止フラグのプロパティ</summary>
        public bool IsStopNoteGenerate
        {
            get { return _isStopNoteGenerate; }
        }

        /// <summary>判定したノーツ数</summary>
        [SerializeField] private int _gotNoteCount;

        /// <summary>判定したノーツ数のプロパティ</summary>
        public int GotNoteCount
        {
            get { return _gotNoteCount; }
        }

        /// <summary>グッド判定のノーツ数</summary>
        [SerializeField] private int _gotGoodCount;

        /// <summary>グッド判定のノーツ数のプロパティ</summary>
        public int GotGoodCount
        {
            get { return _gotGoodCount; }
        }

        /// <summary>バッド判定のノーツ数</summary>
        [SerializeField] private int _gotMissCount;

        /// <summary>バッド判定のノーツ数のプロパティ</summary>
        public int GotMissCount
        {
            get { return _gotMissCount; }
        }

        /// <summary>スタン秒数</summary>
        private float _stanTimeSecond;

        /// <summary>スタン秒数のプロパティ</summary>
        public float StanTimeSecond { get { return _stanTimeSecond; } }

        #endregion

        #region イベント関数

        void Start()
        {
            // 初期化
            _generateNotes = new List<GameObject>();
            _generatedDummyNotes = new List<GameObject>();
        }

        void Update()
        {
            if (isAutoPlayMode)
            {
                AutoPlay();
            }
            CheckNoteIsDeletingDistance();
            CheckDummyNoteIsDeletingDistance();
        }

        #endregion

        #region 関数

        /// <summary>
        /// スタン秒数のセッター
        /// </summary>
        /// <param name="value">セットしたい値</param>
        public void SetStanTimeSecond(float value)
        {
            _stanTimeSecond = value;
#if UNITY_EDITOR
            Debug.Log($"スタン時間を{_stanTimeSecond}にセットしました。入力値:{value}");
#endif
        }

        /// <summary>
        /// ノーツを生成する
        /// </summary>
        public void GenerateNote(Vector2 generatePos)
        {
            if (_isStopNoteGenerate == true)
            {
                GameObject dummyNote = Instantiate(_dummyNotePrefab, generatePos, Quaternion.identity);
                dummyNote.GetComponent<NoteController>().SetIsDummyNotes(true);
                _generatedDummyNotes.Add(dummyNote);
                Debug.Log("スタン中のため、ダミーノーツを生成しました。");
                return;
            }
            GameObject note = Instantiate(_notePrefab, generatePos, Quaternion.identity);
            _generateNotes.Add(note);
#if UNITY_EDITOR
            Debug.Log($"ノーツの生成が完了しました(生成座標：{generatePos})");
#endif
        }

        /// <summary>
        /// ノーツを判定する
        /// </summary>
        /// <param name="buttonNumber">ボタン番号</param>
        public void JudgeMentNoteFromList(int buttonNumber)
        {
            if (_generateNotes.Count == 0) return;

            INoteController noteController = _generateNotes[0].GetComponent<NoteController>();

            // ダミーノーツ用のリストを作成したので必要ない
            // if (noteController.IsDummyNotes) return;

            if (!noteController.CheckButtonEnable()) return;
            int channelIndex = buttonNumber - 1;

            noteController.JudgementNote(buttonNumber, out var judgement);
            Debug.Log(judgement);

            if (judgement == JudgementState.Good)
            {
                // 効果音の再生
                SoundManager.Instance?.PlaySE("button26");
                // チャンネルの切り替え
                _televisionAnimation?.ChangeChannel(channelIndex);

                CountGotNote(judgement);
                RemoveNoteInGenerateList(0);
#if UNITY_EDITOR
                Debug.Log("主導権者の変更を指示します。");
#endif
                getNoteCallBack.Invoke(transform.parent.gameObject.GetComponent<PlayerController>().PlayerNumber);
            }
            else if (judgement == JudgementState.MISS)
            {
                // 効果音の再生
                SoundManager.Instance?.PlaySE("cancel_6");

                CountGotNote(judgement);

                // この行がないとMISS判定が表示されない(ミスしたノーツが即削除されてしまうため)
                RemoveNoteInGenerateList(0);
                SetDummyNoteFromList(_stanTimeSecond);
            }

        }

        /// <summary>
        /// ノートを削除する距離になったかチェックし、達していればミス判定に
        /// </summary>
        private void CheckNoteIsDeletingDistance()
        {
            if (_generateNotes.Count == 0) return;

            INoteController noteController = _generateNotes[0].GetComponent<NoteController>();

            if (noteController.IsDeletingScheduled == true) return;
            if (noteController.IsDeletingDistance == false) return;

            noteController.JudgementNote(out var judgement);
            CountGotNote(judgement);

            // 効果音の再生
            SoundManager.Instance?.PlaySE("cancel_6");

            // この行がないとMISS判定が表示されない(ミスしたノーツが即削除されてしまうため)
            RemoveNoteInGenerateList(0);
            SetDummyNoteFromList(_stanTimeSecond);
        }

        /// <summary>
        /// ダミーノーツを削除する距離になっていたら削除する
        /// </summary>
        private void CheckDummyNoteIsDeletingDistance()
        {
            if (_generatedDummyNotes.Count == 0) return;
            INoteController dummyNoteController = _generatedDummyNotes[0].GetComponent<NoteController>();

            if (dummyNoteController.IsDeletingScheduled == true) return;
            if (dummyNoteController.IsDeletingDistance == false) return;

            dummyNoteController.DeleteThisNote(0);
            RemoveNoteInGenerateList(0, true);
        }

        /// <summary>
        /// 生成リストに格納されたノートを番号指定し除外する
        /// 除外されたノートはオブジェクトとして存在したままになり、NoteManagerからの操作が不可能になる
        /// </summary>
        /// <param name="index">除外したいノートの番号</param>
        /// <param name="isDummyNotes">ダミーノーツかどうか</param>
        private void RemoveNoteInGenerateList(int index, bool isDummyNotes = false)
        {
            if (isDummyNotes)
            {
                _generatedDummyNotes.RemoveAt(index);
                return;
            }
            _generateNotes.RemoveAt(index);
        }

        /// <summary>
        /// 生成リストに格納されたノーツをダミーノーツのリストに追加する
        /// </summary>
        /// <param name="index"></param>
        private void AddToDummyNoteListFromGenerateList(int index)
        {
            _generatedDummyNotes.Add(_generateNotes[index]);
        }

        /// <summary>
        /// リストのノートを全てDestroyした後、hideTime秒間ダミーノーツ生成モードにする
        /// </summary>
        /// <param name="hideTime">非表示時間</param>
        public void HideNoteFromList(float hideTime)
        {
            if (_generateNotes.Count != 0)
            {
                for (int i = 0; i < _generateNotes.Count; i++)
                {
                    Debug.Log($"リストから{i}番目のノートを削除します。");
                    INoteController noteController = _generateNotes[i].GetComponent<NoteController>();
                    noteController.DeleteThisNote(0);
                    RemoveNoteInGenerateList(i);
                }

            }

            StartCoroutine(StopGenerateNote(hideTime));
        }

        /// <summary>
        /// リストのノートを全てダミーノーツに設定後、指定秒間ダミーノーツ生成モードにする
        /// </summary>
        /// <param name="hideTime"></param>
        private void SetDummyNoteFromList(float hideTime)
        {
            if (_generateNotes.Count != 0)
            {
                for (int i = 0; i < _generateNotes.Count; i++)
                {
                    Debug.Log($"リストから{i}番目のノートをダミーノーツにします。");
                    INoteController noteController = _generateNotes[i].GetComponent<NoteController>();
                    noteController.SetIsDummyNotes(true);
                    AddToDummyNoteListFromGenerateList(i);
                    RemoveNoteInGenerateList(i);
                }

            }

            StartCoroutine(StopGenerateNote(hideTime));
        }

        /// <summary>
        /// 取得したノーツをカウントする
        /// </summary>
        /// <param name="judgement">判定結果</param>
        private void CountGotNote(JudgementState judgement)
        {
            _gotNoteCount++;
            if (judgement == JudgementState.Good)
            {
                _gotGoodCount++;
            }
            else
            {
                _gotMissCount++;
            }
        }

        /// <summary>
        /// オートプレイモード時にUpdate内で実行する関数
        /// </summary>
        private void AutoPlay()
        {
            if (_generateNotes.Count == 0) return;

            NoteController noteController = _generateNotes[0].GetComponent<NoteController>();

            if (noteController.Distance >= 0.0f)
            {
                JudgeMentNoteFromList(noteController.ButtonNumber);
            }
        }

        #endregion

        #region コルーチン

        /// <summary>
        /// ノーツの生成を停止する
        /// </summary>
        /// <param name="hideTime">停止時間</param>
        private IEnumerator StopGenerateNote(float hideTime)
        {
            Debug.Log($"ノーツの生成を{hideTime}秒停止します。");
            float time = 0.0f;
            _isStopNoteGenerate = true;

            while (time < hideTime)
            {
                time += Time.deltaTime;
                yield return null;
            }

            _isStopNoteGenerate = false;
        }

        #endregion
    }
}
