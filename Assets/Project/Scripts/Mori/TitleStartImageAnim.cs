using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleStartImageAnim : MonoBehaviour
{
    //Tweenにかかる時間
    [SerializeField]
    private float _duration = 0.8f;
    //フェードのインターバル
    [SerializeField]
    private float _interval = 1.2f;

    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        //Renderer設定とアニメーションスタート
        _spriteRenderer=GetComponent<SpriteRenderer>();
        TitleImageAnim();
    }

    /// <summary>
    /// タイトルの文のフェードアニメーション
    /// </summary>
    private void TitleImageAnim()
    {
        //シーケンスでもなんでも
        //Enterを押す画像切替のアニメーション

        //シーケンス作成
        var sequence = DOTween.Sequence();
        //フェードインアウト
        sequence.Append(
            DOVirtual.Float(
                from: 1.0f,
                to: 0f,
                duration: _duration,
                onVirtualUpdate: (tweenValue) => {
                    _spriteRenderer.color = new Color(1, 1, 1, tweenValue);
                }
                ).SetEase(Ease.InOutCubic).SetLoops(2, LoopType.Yoyo)
        );
        //インターバル設定
        sequence.AppendInterval(_interval);
        //無限ループ
        sequence.SetLoops(-1);
        //シーケンス実行
        sequence.Play();
    }
}
