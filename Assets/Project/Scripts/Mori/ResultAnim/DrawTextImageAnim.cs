using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTextImageAnim : MonoBehaviour
{
    //アニメーションの対象を参照　移動、回転
    [Header("アニメーションRectTransform")]
    [SerializeField, Label("0から順に移動アニメーション開始")]
    private List<RectTransform> _moveTImageList = new List<RectTransform>();

    [SerializeField, Label("移動アニメーション速さ/秒")]
    private float _moveAnimSpeed = 0.5f;
    [SerializeField, Label("移動アニメーション遅延/秒")]
    private float _moveAnimDelay = 0.2f;
    [SerializeField, Label("移動アニメーション移動幅/スクリーンサイズ")]
    private float _moveAnimAddvalue = 100f;


    void Start()
    {
        //実行
        TextImageAnimation();
    }

    /// <summary>
    /// DOTWEENシーケンスの設定と実行
    /// </summary>
    private void TextImageAnimation()
    {
        //シーケンス設定
        Sequence drawSequence = DOTween.Sequence();

        //D r a w がウェーブ状に順に上に跳ねる
        for (int i = 0; i < _moveTImageList.Count; i++)
        {
            //自身の座標+上がり幅
            //反復させ跳ねるようにし、delayでウェーブに
            drawSequence.Join(_moveTImageList[i].DOAnchorPosY(_moveTImageList[i].localPosition.y + _moveAnimAddvalue
                                                            , _moveAnimSpeed)
                                                            .SetEase(Ease.OutCubic).SetLoops(2, LoopType.Yoyo).SetDelay(_moveAnimDelay));
        }
        //無限ループさせる
        drawSequence.SetLoops(-1);

        //実行
        drawSequence.Play();
    }
}