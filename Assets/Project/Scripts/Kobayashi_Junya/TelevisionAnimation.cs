using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BatotteChannel.InGame.UI
{
    /// <summary>
    /// インゲームのテレビのアニメーションを行うクラス
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class TelevisionAnimation : MonoBehaviour
    {
        /// <summary>チャンネルを指定する際に使用する変数</summary>
        private int _channelIndex;

        /// <summary>チャンネル画像のリスト</summary>
        [SerializeField]
        private List<Sprite> _televisionChannnels = new List<Sprite>();

        /// <summary>画像を表示するオブジェクト</summary>
        [SerializeField] private GameObject _televisionObject;

        void Start()
        {
            _channelIndex = 0;
        }

        /// <summary>
        /// 次のチャンネルに切り替え、チャンネルを表示する
        /// </summary>
        public void NextChannnel()
        {
            if (_channelIndex < _televisionChannnels.Count - 1)
            {
                _channelIndex++;
            }
            else
            {
                _channelIndex = 0;
            }

            _televisionObject.GetComponent<SpriteRenderer>().sprite = _televisionChannnels[_channelIndex];
        }

        /// <summary>
        /// チャンネルを指定して変更する
        /// </summary>
        /// <param name="channelIndex"></param>
        public void ChangeChannel(int channelIndex)
        {
            if (channelIndex < 0 || channelIndex >= _televisionChannnels.Count) return;
            _televisionObject.GetComponent<SpriteRenderer>().sprite = _televisionChannnels[channelIndex];
        }
    }
}
