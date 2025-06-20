using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleLogoAnim : MonoBehaviour
{
    [SerializeField]
    private float _interval = 1.2f;

    //リモコンから出る電波
    [SerializeField]
    private RectTransform _volt1Obj;
    [SerializeField]
    private RectTransform _volt2Obj;
    //音量イメージ
    [SerializeField]
    private RectTransform _volumeObj;
    //上のオレンジのワイワイしてるもの
    [SerializeField]
    private RectTransform _laughRectTra;
    //タイトルテキスト
    [SerializeField]
    private RectTransform _titleRectTra;
    //リモコン
    [SerializeField]
    private RectTransform _rimoconRectTra;

    [SerializeField]
    private float _lowChangeTime=2f;

    void Start()
    {
        TitleLogoImageAnim();
    }

    [Button]
    private void TitleLogoImageAnim()
    {
        //常に動くもの
        /*
        var sequence2 = DOTween.Sequence();
        sequence2.Join(_laughRectTra.DOLocalRotate(new Vector3(0,0,-10f) , _lowChangeTime).SetEase(Ease.InOutCubic).SetLoops(2,LoopType.Yoyo));
        sequence2.Join(_titleRectTra.DOScale(new Vector2(1.05f,1.05f) , _lowChangeTime).SetEase(Ease.InOutQuint).SetLoops(2,LoopType.Yoyo));
        sequence2.AppendInterval(_interval);
        sequence2.SetLoops(-1);
        sequence2.Play();
        */
        _laughRectTra.DOLocalRotate(new Vector3(0, 0, -10f), _lowChangeTime).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        _titleRectTra.DOScale(new Vector2(1.05f, 1.05f), _lowChangeTime).SetEase(Ease.InOutQuint).SetLoops(-1, LoopType.Yoyo);

        //たまに動くもの　インターバルを使用するもの
        var sequence = DOTween.Sequence();
        //後に追加　メガホン
        sequence.Join(_volt2Obj.DOAnchorPosY(100, 0.1f).SetEase(Ease.OutCirc).SetLoops(4, LoopType.Yoyo));
        sequence.PrependInterval(0.1f);
        sequence.Join(_volt1Obj.DOAnchorPosY(100, 0.1f).SetEase(Ease.OutCirc).SetLoops(4, LoopType.Yoyo));
        sequence.Join(_rimoconRectTra.DOAnchorPosY(25, 0.1f).SetEase(Ease.OutCirc).SetLoops(2, LoopType.Yoyo));
        sequence.AppendInterval(_interval);
        sequence.SetLoops(-1);
        //Playで実行
        sequence.Play();
    } 
}
