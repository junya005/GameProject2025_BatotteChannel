using UnityEngine;

namespace BatotteChannel.InGame.MusicSystem
{
    /// <summary>
    /// ビート計上に関するクラス
    /// ノーツの生成時に参照する
    /// </summary>
    public class BeatCounter : MonoBehaviour
    {
        /// <summary>楽曲のBPMを設定</summary>
        [SerializeField] private float _bpm;

        /// <summary>楽曲のSPB(Second Per Beat)を格納</summary>
        private float _spb;

        /// <summary>Beat計上のためのカウンター</summary>
        private float _secCounter;

        /// <summary>現在のビート</summary>
        [SerializeField] private int _beat;

        /// <summary>現在ビートのゲッタープロパティ</summary>
        public int Beat
        {
            get { return _beat; }
        }

        /// <summary>
        /// 現在ビートのセッター関数
        /// </summary>
        /// <param name="beat">設定したい数</param>
        public void SetBeat(int beat)
        {
            _beat = beat;
            _secCounter = 0;
#if UNITY_EDITOR
            Debug.Log($"Beatを{_beat}に設定しました。");
#endif
        }

        void Start()
        {
            // SPBを計算
            _spb = 60.0f / _bpm;
        }

        void Update()
        {
            _secCounter += Time.deltaTime;

            // ビートをカウント
            if (_secCounter >= _spb)
            {
                _beat++;
                _secCounter = 0;
            }
        }
    }
}
