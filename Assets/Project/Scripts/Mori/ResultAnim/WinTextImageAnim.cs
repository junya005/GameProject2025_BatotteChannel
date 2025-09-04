using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

/// <summary>
/// 勝ち表記アニメーション
/// </summary>
public class WinTextImageAnim : MonoBehaviour
{
    //アニメーションの対象を参照　移動、回転
    [Header("アニメーションRectTransform")]
    [SerializeField,Label("0から順に移動アニメーション開始")]
    private List<RectTransform> _moveTImageList = new List<RectTransform>();
    [SerializeField, Label("0から順に回転アニメーション開始")]
    private List<RectTransform> _rotTImageList = new List<RectTransform>();

    [SerializeField, Label("移動アニメーション速さ/秒")]
    private float _moveAnimSpeed = 0.5f;
    [SerializeField,Label("移動アニメーション遅延/秒")]
    private float _moveAnimDelay = 0.2f;
    [SerializeField, Label("移動アニメーション移動幅/スクリーンサイズ")]
    private float _moveAnimAddvalue = 100f;

    [SerializeField, Label("回転アニメーション速さ/秒")]
    private float _rotAnimSpeed = 0.5f;
    [SerializeField, Label("回転アニメーション遅延/秒")]
    private float _rotAnimDelay = 0.2f;
    [SerializeField, Label("回転アニメーション回転量/度")]
    private float _rotAnimAddvalue = 360f;


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
        Sequence winSequence = DOTween.Sequence();

        //W i n ! ! がウェーブ状に順に上に跳ねる
        for (int i = 0; i < _moveTImageList.Count; i++)
        {
            //自身の座標+上がり幅
            //反復させ跳ねるようにし、delayでウェーブに
            winSequence.Join(_moveTImageList[i].DOAnchorPosY(_moveTImageList[i].localPosition.y + _moveAnimAddvalue
                                                            , _moveAnimSpeed)
                                                            .SetEase(Ease.OutCubic).SetLoops(2, LoopType.Yoyo).SetDelay(_moveAnimDelay));
        }
        //同時に順に星がまわる
        for (int i = 0; i < _rotTImageList.Count; i++)
        {
            //Insertで上記の移動の開始と同時に行う
            //勢いをつけるように回転前に助走が入る
            winSequence.Insert(_moveAnimDelay, _rotTImageList[i].DORotate(new Vector3(0,0, _rotAnimAddvalue), _rotAnimSpeed, RotateMode.LocalAxisAdd)
                                                            .SetEase(Ease.InOutBack).SetDelay(_rotAnimDelay));
        }

        //無限ループさせる
        winSequence.SetLoops(-1);

        //実行
        winSequence.Play();
    }
}
