using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BatotteChannel.Player
{
    /// <summary>
    /// プレイヤーのアニメーションを管理するクラス
    /// </summary>
    public class AnimatorParamSetter : MonoBehaviour
    {
        /// <summary>プレイヤーのAnimator</summary>
        [SerializeField]
        private Animator _playerAnim;

        /// <summary>
        /// アニメーションのトリガーパラメータをセットする
        /// </summary>
        /// <param name="pramName">パラメータ名</param>
        public void SetAnimationTrigger(string pramName)
        {
            // 指定パラメータが存在するか調べる
            var animPrameter = Array.Find(
                    _playerAnim.parameters,
                    p => p.name == pramName
                    );

            if (_playerAnim != null || animPrameter != null)
                // 指定されたパラメータのアニメーションをセット
                _playerAnim.SetTrigger(pramName);
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("指定されたパラメータが存在しませんでした。");
#endif
            }
        }

        /// <summary>
        /// アニメーションのトリガーパラメータをセットする
        /// </summary>
        /// <param name="pramName">パラメータ名</param>
        /// <param name="value">入れたい値</param>
        public void SetAnimationBool(string pramName, bool value)
        {
            // 指定パラメータが存在するか調べる
            var animPrameter = Array.Find(
                    _playerAnim.parameters,
                    p => p.name == pramName
                    );

            if (_playerAnim != null || animPrameter != null)
                // 指定されたパラメータのアニメーションをセット
                _playerAnim.SetBool(pramName, value);
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("指定されたパラメータが存在しませんでした。");
#endif
            }
        }
    }
}
