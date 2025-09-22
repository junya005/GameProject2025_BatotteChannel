using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BatotteChannel.AudioSystem
{
    /// <summary>
    /// サウンドドライバー
    /// </summary>
    /// <remarks>
    /// ファイル名をstringで引数に指定し、PlayBGM, PlaySE関数で実行することで音を再生できる
    /// </remarks>
    public class SoundManager : Singleton<SoundManager>
    {
        [Header("アセット参照")]
        [Tooltip("BGMとSEを全て設定したScriptableObjectのデータアセットを設定してください"), SerializeField]
        private AudioDataScriptableObject _audioList;

        private Dictionary<string, AudioSource> _bgmAudioSources = new Dictionary<string, AudioSource>();
        private Dictionary<string, AudioSource> _seAudioSources = new Dictionary<string, AudioSource>();

        private List<AudioSource> _audioSources;

        private bool _isInitialized = false;

        // Start is called before the first frame update
        new void Awake()
        {
            var audioSourceMax = _audioList.BGMList.Count + _audioList.SEList.Count;

            if (_isInitialized) return;

            // BGM配列の作成
            foreach (var clip in _audioList.BGMList)
            {
                var audioSource = this.gameObject.AddComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.playOnAwake = false;
                audioSource.loop = true;
                _bgmAudioSources.Add(audioSource.clip.name, audioSource);
            }

            // SE配列の作成
            foreach (var clip in _audioList.SEList)
            {
                var audioSource = this.gameObject.AddComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.playOnAwake = false;
                audioSource.loop = false;
                _seAudioSources.Add(audioSource.clip.name, audioSource);
            }

            _isInitialized = true;
        }

        /// <summary>
        /// BGMを再生する、再生されているBGMがある場合は処理がスキップされます
        /// </summary>
        /// <remarks>
        /// よって、これを実行する前にStopBGMメゾットを実行しておくことを推奨します
        /// </remarks>
        /// <param name="name">効果音ファイル名(拡張子を除く)</param>
        public void PlayBGM(string name)
        {
            foreach (var audioSource in _bgmAudioSources)
            {
                if (_bgmAudioSources[audioSource.Key].isPlaying) return;
            }
            _bgmAudioSources[name].Play();
        }

        /// <summary>
        /// 効果音を再生する
        /// </summary>
        /// <param name="name">効果音ファイル名(拡張子を除く)</param>
        public void PlaySE(string name)
        {
            _seAudioSources[name].Play();
        }

        /// <summary>
        /// すべてのBGMを止める
        /// </summary>
        public void StopBGM()
        {
            foreach (var audioSource in _bgmAudioSources)
            {
                _bgmAudioSources[audioSource.Key].Stop();
            }
        }

        /// <summary>
        /// BGMのボリュームを設定する
        /// </summary>
        /// <param name="volume">ボリューム値</param>
        public void SetBgmVolume(float volume)
        {
            foreach (var audioSource in _bgmAudioSources)
            {
                audioSource.Value.volume = volume;
            }
        }

        /// <summary>
        /// BGMのボリュームを設定する
        /// </summary>
        /// <remarks>
        /// [オーバーロード]BGMを指定して調整できます
        /// </remarks>
        /// <param name="volume">ボリューム値</param>
        /// <param name="bgmName">BGM名</param>
        public void SetBgmVolume(float volume, string bgmName)
        {
            _bgmAudioSources[bgmName].volume = volume;
        }
    }
}
