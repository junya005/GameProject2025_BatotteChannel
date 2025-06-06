using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class CharactorBeatAnim : MonoBehaviour
{
    [SerializeField, Label("片道の時間")]
    private float _memeTime = 0.2f;
    [SerializeField, Label("移動幅")]
    private float _memeArea = 0.4f;
    [SerializeField, Label("テンポ幅")]
    private float _memeDelay = 0.19f;

    [SerializeField,Label("キャラクターがリズムにのるか")]
    public bool _rideTheBeat = true;

    /// <summary>
    /// テスト用　アニメーションをスタートする関数
    /// </summary>
    [Button]
    public void AnimationStart()
    {
        //アニメーションスタート
        _rideTheBeat = true;
        AnimationLoop(this.GetCancellationTokenOnDestroy()).Forget();
    }
    public async UniTaskVoid AnimationLoop(CancellationToken cancellationToken)
    {
        //ループ
        while (_rideTheBeat)
        {
            //上下にキャラが動くアニメーション　参考：head bop meme
            await this.gameObject.transform.DOMoveY(this.gameObject.transform.position.y - _memeArea, _memeTime).SetEase(Ease.InCubic);
            await this.gameObject.transform.DOMoveY(this.gameObject.transform.position.y + _memeArea, _memeTime).SetEase(Ease.OutSine);
            //間隔を開ける
            await UniTask.Delay((int)(_memeDelay * 1000));
            //オブジェクトが削除されていたらタスクを削除
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
        }
    }
}
