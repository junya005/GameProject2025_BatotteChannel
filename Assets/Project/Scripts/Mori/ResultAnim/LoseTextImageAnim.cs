using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 負け表記アニメーション
/// </summary>
public class LoseTextImageAnim : MonoBehaviour
{
    //アニメーションの対象を参照　移動、回転
    [Header("アニメーションRectTransform")]
    [SerializeField, Label("0から順に移動アニメーション開始")]
    private List<RectTransform> _moveTImageList = new List<RectTransform>();
    [SerializeField, Label("0から順に付属アニメーション開始")]
    private List<RectTransform> _moveAcceImageList = new List<RectTransform>();

    [SerializeField, Label("移動アニメーション速さ/秒")]
    private float _moveAnimSpeed = 0.5f;
    [SerializeField, Label("移動アニメーション遅延/秒")]
    private float _moveAnimDelay = 0.2f;
    [SerializeField, Label("移動アニメーション移動幅/スクリーンサイズ")]
    private float _moveAnimAddvalue = 100f;

    [SerializeField, Label("付属アニメーション速さ/秒")]
    private float _moveAcceAnimSpeed = 0.5f;
    [SerializeField, Label("付属アニメーション遅延/秒")]
    private float _moveAcceAnimDelay = 0.2f;
    [SerializeField, Label("付属アニメーション移動幅/スクリーンサイズ")]
    private Vector2 _moveAcceAnimAddvalueOffset;

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
        Sequence loseSequence = DOTween.Sequence();

        //L o s e がウェーブ状に順に波打つ

        loseSequence.AppendCallback(WaveTextImage());

        //汗アニメーション
        //左右に移動する？　ななめも噛む？
        //透明度は変更する？移動だけ？
        //同時移動させる？　差異を付ける？
        for (int i = 0; i < _moveAcceImageList.Count; i++)
        {
            loseSequence.Insert(0 + (i * _moveAcceAnimDelay)
                , _moveAcceImageList[i].DOAnchorPos(new Vector2(_moveAcceImageList[i].localPosition.x + _moveAcceAnimAddvalueOffset.x
                                                                ,_moveAcceImageList[i].localPosition.y + _moveAcceAnimAddvalueOffset.y)
                                                                , _moveAcceAnimSpeed)
                                                                .SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo));
        }

        //無限ループさせる
        loseSequence.SetLoops(-1);

        //実行
        loseSequence.Play();
    }

    private TweenCallback WaveTextImage()
    {
        for (int i = 0; i < _moveTImageList.Count; i++)
        {
            //自身の座標+上がり幅
            //ゆるやかに上下し、Insert + delayでウェーブに
             _moveTImageList[i].DOAnchorPosY(_moveTImageList[i].localPosition.y + _moveAnimAddvalue
                                                            , _moveAnimSpeed)
                                                            .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetDelay(0 + (i * _moveAnimDelay));
        }
        return null;
    }
}
