using System;
using System.Collections.Generic;
using UnityEngine;

namespace BatotteChannel.InGame.UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TelevisionAnimation : MonoBehaviour
    {
        private int _channelIndex;

        [SerializeField]
        private List<Sprite> _televisionChannnels = new List<Sprite>();

        [SerializeField] private GameObject _televisionObject;

        void Start()
        {
            _channelIndex = 0;
        }

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
    }
}
