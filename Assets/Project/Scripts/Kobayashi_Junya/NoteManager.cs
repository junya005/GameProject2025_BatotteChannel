using System;
using System.Collections;
using System.Collections.Generic;
using BatotteChannel.AudioSystem;
using BatotteChannel.InGame.UI;
using UnityEngine;

namespace BatotteChannel.InGame.Notes
{
    /// <summary>ノーツの管理に関するクラス</summary>
    public class NoteManager : MonoBehaviour
    {
        #region 変数

        [SerializeField] private TelevisionAnimation _televisionAnimation;

        /// <summary>生成されたノートを格納</summary>
        [SerializeField] private List<GameObject> _generateNotes;

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

        #endregion

        #region 関数

        /// <summary>
        /// ノーツを生成する
        /// </summary>
        public void GenerateNote(Vector2 generatePos)
        {
            if (_isStopNoteGenerate == true)
            {
                GameObject dummyNote = Instantiate(_dummyNotePrefab, generatePos, Quaternion.identity);
                dummyNote.GetComponent<NoteController>().SetIsDummyNotes(true);
                _generateNotes.Add(dummyNote);
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
        /// <param name="buttonNumber"></param>
        public void JudgeMentNoteFromList(int buttonNumber)
        {
            if (_generateNotes.Count == 0) return;

            INoteController noteController = _generateNotes[0].GetComponent<NoteController>();

            if (noteController.IsDummyNotes) return;

            noteController.JudgementNote(buttonNumber, out var judgement);
            Debug.Log(judgement);

            if (judgement == JudgementState.Good)
            {
                // 効果音の再生
                SoundManager.Instance?.PlaySE("button26");
                // チャンネルの切り替え(現在は次のチャンネルへ移動させる仕様)
                _televisionAnimation?.NextChannnel();

                CountGotNote(judgement);
                RemoveNoteInList(0);
            }
            else if (judgement == JudgementState.MISS)
            {
                // 効果音の再生
                SoundManager.Instance?.PlaySE("cancel_6");

                CountGotNote(judgement);

                // この行がないとMISS判定が表示されない(ミスしたノーツが即削除されてしまうため)
                RemoveNoteInList(0);
                HideNoteFromList(3.0f);
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

            // ダミーノーツであればこのタイミングで即削除
            if (noteController.IsDummyNotes)
            {
                noteController.DeleteThisNote(0);
                RemoveNoteInList(0);
                return;
            }

            noteController.JudgementNote(out var judgement);
            CountGotNote(judgement);

            // この行がないとMISS判定が表示されない(ミスしたノーツが即削除されてしまうため)
            RemoveNoteInList(0);
            HideNoteFromList(3.0f);
        }

        /// <summary>
        /// リストに格納されたノートを番号指定し除外する
        /// 除外されたノートはオブジェクトとして存在したままになり、NoteManagerからの操作が不可能になる
        /// </summary>
        /// <param name="index">除外したいノートの番号</param>
        private void RemoveNoteInList(int index)
        {
            _generateNotes.RemoveAt(index);
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
                    RemoveNoteInList(i);
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

        #region イベント関数

        void Start()
        {
            // キャッシュ
            _generateNotes = new List<GameObject>();
        }

        void Update()
        {
            CheckNoteIsDeletingDistance();
        }

        #endregion
    }
}
