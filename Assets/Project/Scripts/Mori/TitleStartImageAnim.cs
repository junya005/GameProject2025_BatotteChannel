using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleStartImageAnim : MonoBehaviour
{
    [SerializeField]
    private float _duration = 0.8f;
    [SerializeField]
    private float _interval = 1.2f;

    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer=GetComponent<SpriteRenderer>();
        TitleImageAnim();
    }

    private void TitleImageAnim()
    {
        var sequence = DOTween.Sequence();
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
        sequence.AppendInterval(_interval);
        sequence.SetLoops(-1);
        Debug.Log("Set DoTween");
        //PlayÇ≈é¿çs
        sequence.Play();
    }
}
