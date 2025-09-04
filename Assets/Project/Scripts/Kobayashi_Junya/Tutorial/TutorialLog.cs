using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading;
using UnityEngine.Rendering.Universal;

namespace BatotteChannel.Tutorial
{
    [RequireComponent(typeof(Image), typeof(CanvasGroup))]
    public class TutorialLog : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> _logSprites = new List<Sprite>();

        private Image _image;

        private CanvasGroup _canvasGroup;

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        // Start is called before the first frame update
        void Start()
        {
            _image = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetLogSprite(int index)
        {
            if (index < 0 || index >= _logSprites.Count) return;

            _image.sprite = _logSprites[index];
        }

        public void SetLogSpriteWithAnimation(int index)
        {
            if (index < 0 || index >= _logSprites.Count) return;

            var token = _cancellationTokenSource.Token;
            ChangeLogAnime(index, 1.0f, token);
        }

        public async UniTask ChangeLogAnime(int index, float duration, CancellationToken token)
        {
            if (index < 0 || index >= _logSprites.Count) return;

            await DOVirtual.Float(
                from: _canvasGroup.alpha,
                to: 0.0f,
                duration: duration,
                onVirtualUpdate: (tweenValue) => { _canvasGroup.alpha = tweenValue; }
                );

            _image.sprite = _logSprites[index];

            await DOVirtual.Float(
                from: _canvasGroup.alpha,
                to: 1.0f,
                duration: duration,
                onVirtualUpdate: (tweenValue) => { _canvasGroup.alpha = tweenValue; }
                );
        }
    }
}
