using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace BatotteChannel.InGame
{
    /// <summary>プレイヤーミス時のアニメーション</summary>
    public class PlayerMissedAnimation : MonoBehaviour
    {
        /// <summary>プレイヤーのオブジェクト</summary>
        [SerializeField]
        private GameObject playerObject;

        /// <summary>座標移動量</summary>
        [SerializeField]
        private float _moveDistance = 1.0f;

        /// <summary>回転移動量</summary>
        [SerializeField]
        private float _rotateDistance = 15.0f;

        /// <summary>アニメーション時間</summary>
        [SerializeField]
        private float _startDuration = 0.2f;

        /// <summary>アニメーション時間</summary>
        [SerializeField]
        private float _endDuration = 0.2f;

        /// <summary>
        /// アニメーションを開始する
        /// </summary>
        public void StartAnimation()
        {
            StartCoroutine(PlayMissedAnim(_moveDistance, _rotateDistance, _startDuration, _endDuration));
        }

        /// <summary>
        /// アニメーションを開始する(数値指定)
        /// </summary>
        /// <param name="moveDistance">座標移動量</param>
        /// <param name="rotateDistance">回転移動量</param>
        /// <param name="startDuration">アニメーション時間</param>
        public void StartAnimation(float moveDistance, float rotateDistance, float startDuration, float endDuration)
        {
            StartCoroutine(PlayMissedAnim(moveDistance, rotateDistance, startDuration, endDuration));
        }

        /// <summary>
        /// アニメーション処理
        /// </summary>
        /// <param name="moveDistance">座標移動量</param>
        /// <param name="rotateDistance">回転移動量</param>
        /// <param name="startDuration">アニメーション時間</param>
        /// <returns>yield</returns>
        private IEnumerator PlayMissedAnim(float moveDistance, float rotateDistance, float startDuration, float endDuration)
        {
            // 時間変数を定義
            float timeCount = 0.0f;

            // Lerp用変数を定義
            float t;

            // 座標、回転の開始と終了を定義
            Vector2 startPos = playerObject.transform.position;
            Vector2 endPos = new Vector2(startPos.x, startPos.y + moveDistance);

            Vector3 startRot = playerObject.transform.rotation.eulerAngles;
            Vector3 endRot = new Vector3(startRot.x, startRot.y, startRot.z + rotateDistance);

            // 回転系は使用しないので一旦コメントに
            // 現在の回転値を入れる変数を定義
            // Vector3 currentRot;

            // プレイヤーを上げる
            while (timeCount < startDuration)
            {
                // 時間を加算
                timeCount += Time.deltaTime;

                // アニメーション時間と現在時間の割合を計算
                t = timeCount / startDuration;

                playerObject.transform.position = Vector2.Lerp(startPos, endPos, t);
                //currentRot = Vector3.Lerp(startRot, endRot, t);
                //playerObject.transform.rotation = Quaternion.Euler(currentRot);

                yield return null;
            }

            // 時間をリセット
            timeCount = 0.0f;

            // プレイヤーを下げる
            while (timeCount < endDuration)
            {
                // 時間を加算
                timeCount += Time.deltaTime;

                // アニメーション時間と現在時間の割合を計算
                t = timeCount / endDuration;

                playerObject.transform.position = Vector2.Lerp(endPos, startPos, t);
                //currentRot = Vector3.Lerp(endRot, startRot, t);
                //playerObject.transform.rotation = Quaternion.Euler(currentRot);

                yield return null;
            }
        }
    }
}
