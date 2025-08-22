using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BatotteChannel.InGame.Notes
{
    public interface INoteController
    {
        /// <summary>削除距離フラグのゲッター</summary>
        bool IsDeletingDistance { get; }
        /// <summary>削除指示フラグのゲッター</summary>
        bool IsDeletingScheduled { get; }
        /// <summary>判定種類のインスタンスのゲッター</summary>
        JudgementState NoteJudgement { get; }
        /// <summary>ダミーノーツか否か</summary>
        bool IsDummyNotes { get; }

        /// <summary>
        /// このノートを判定する
        /// その後、このノートを1秒後に削除する
        /// </summary>
        /// <param name="judgement">判定結果を返す</param>
        void JudgementNote(int buttonNumber, out JudgementState judgement);

        /// <summary>
        /// このノーツを判定する(入力期間を過ぎた場合はこちらを使用)
        /// </summary>
        /// <param name="judgement">判定結果を返す</param>
        void JudgementNote(out JudgementState judgement);

        /// <summary>
        /// このノートを削除する
        /// </summary>
        /// <param name="t">削除までの時間</param>
        void DeleteThisNote(float t, bool isTimeElapsed = false);

        /// <summary>
        /// ボタン入力が有効な範囲かチェックする
        /// </summary>
        /// <returns>ボタン入力が有効化</returns>
        bool CheckButtonEnable();

        /// <summary>
        /// ダミーノーツにセットする
        /// </summary>
        /// <param name="boolean"></param>
        void SetIsDummyNotes(bool boolean);
    }
}
