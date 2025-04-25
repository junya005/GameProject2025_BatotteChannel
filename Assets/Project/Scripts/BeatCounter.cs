using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicSystem
{
    public class BeatCounter : MonoBehaviour
    {
        [SerializeField] private float _bpm;
        [SerializeField] private float _spb;
        private float _secCounter;
        [SerializeField] private float _beat;
        public float Beat
        {
            get { return _beat; }
        }

        void Start()
        {
            _spb = 60.0f / _bpm;
        }

        void Update()
        {
            _secCounter += Time.deltaTime;

            if (_secCounter >= _spb)
            {
                _beat++;
                _secCounter = 0;
            }
        }
    }
}
