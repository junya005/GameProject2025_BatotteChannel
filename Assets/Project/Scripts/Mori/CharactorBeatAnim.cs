using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CharactorBeatAnim : MonoBehaviour
{
    [SerializeField, Label("ï–ìπÇÃéûä‘")]
    private float _memeTime = 0.2f;
    [SerializeField, Label("à⁄ìÆïù")]
    private float _memeArea = 0.4f;
    [SerializeField, Label("ÉeÉìÉ|ïù")]
    private float _memeDelay = 0.4f;

    /*
    [Button]
    public async void Animation()
    {
        await this.gameObject.transform.DOMoveY(this.gameObject.transform.position.y - _memeArea, _memeTime).SetEase(Ease.InCubic);
        await this.gameObject.transform.DOMoveY(this.gameObject.transform.position.y + _memeArea, _memeTime).SetEase(Ease.OutSine);
        await UniTask.Delay((int)(_memeDelay * 1000));
        Animation();
    }
    */
    [Button]
    public void AnimationStart()
    {
        AnimationLoop(this.GetCancellationTokenOnDestroy()).Forget();
    }
    public async UniTaskVoid AnimationLoop(CancellationToken cancellationToken)
    {
        while (true)
        {
            await this.gameObject.transform.DOMoveY(this.gameObject.transform.position.y - _memeArea, _memeTime).SetEase(Ease.InCubic);
            await this.gameObject.transform.DOMoveY(this.gameObject.transform.position.y + _memeArea, _memeTime).SetEase(Ease.OutSine);
            await UniTask.Delay((int)(_memeDelay * 1000));
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
        }
    }
}
