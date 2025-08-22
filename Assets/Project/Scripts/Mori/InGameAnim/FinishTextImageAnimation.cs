using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class FinishTextImageAnimation : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 offset;
    [Header("出現する")]
    [SerializeField,Label("出現するのにかかる時間/秒")]
    private float widthDuration = 0.3f;
    [Header("ジャンプする")]
    [SerializeField,Label("上に飛ぶ量/スクリーンサイズ")]
    private float jumpUpSize = 50;
    [SerializeField, Label("上に飛ぶのにかかる時間/秒")]
    private float jumpUpDuration = 0.3f;
    [SerializeField, Label("下に落ちるのにかかる時間/秒")]
    private float fallDownDuration = 0.2f;
    [Header("反動でもちっとする")]
    [SerializeField, Label("着地時に反動として変わるサイズ量/スクリーンサイズ")]
    private float changeSize = 30f;
    [SerializeField, Label("着地で反動する片道時間/秒")]
    private float hopDuration = 0.05f;

    [SerializeField,Header("Debug用:ループする")]
    private bool debug = false;

    // Start is called before the first frame update
    void Start()
    {
        debug = false;
        rectTransform =GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(0, 295);
        offset= rectTransform.localPosition;
        FinishAnimation();
    }

    [Button]
    private void FinishAnimation()
    {
        Sequence finSequwnce = DOTween.Sequence();
        //width 0から827へ拡大
        finSequwnce.Append(DOVirtual.Float(0, 827, widthDuration, (tweenValue) => { rectTransform.sizeDelta = new Vector2(tweenValue, 295); }));
        //終わったら上にジャンプ
        finSequwnce.Join(rectTransform.DOAnchorPosY(rectTransform.localPosition.y + jumpUpSize, jumpUpDuration).SetEase(Ease.OutCubic).SetDelay(widthDuration * 2 / 3));
        //下に落ち着地
        finSequwnce.Append(rectTransform.DOAnchorPosY(offset.y, fallDownDuration).SetEase(Ease.InCubic));
        //同時に潰れて戻る
        finSequwnce.Join(DOVirtual.Float(0, changeSize, hopDuration, (tweenValue) => { rectTransform.sizeDelta = new Vector2(827 + tweenValue, 295 - tweenValue); }).SetDelay(fallDownDuration).SetLoops(2,LoopType.Yoyo));

        if(debug)
        {
            DOVirtual.DelayedCall(2f, () => FinishAnimation());
        }

        finSequwnce.Play();
    }
}
