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

    [SerializeField]
    private float _upTime = 1.0f;
    [SerializeField]
    private float _downTime = 0.4f;
    [SerializeField]
    private float _flyTime = 0.5f;
    [SerializeField]
    private Vector2 _volOffset = Vector2.zero;

    //リモコンから出る電波
    [SerializeField]
    private RectTransform _volt1Obj;
    [SerializeField]
    private RectTransform _volt2Obj;
    //音量イメージ：メガホン
    [SerializeField]
    private RectTransform _volMegahoneObj;
    //音量イメージ：音波
    [SerializeField]
    private RectTransform _volEffLObj;
    private CanvasGroup _volEffLObjCG;
    //位置保存用
    private Vector2 _volEffLObjTra;
    [SerializeField]
    private RectTransform _volEffUObj;
    private CanvasGroup _volEffUObjCG;
    //位置保存用
    private Vector2 _volEffUObjTra;
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
        //初期位置保存
        _volEffUObjTra = new Vector2(_volEffUObj.localPosition.x, _volEffUObj.localPosition.y);
        _volEffLObjTra = new Vector2(_volEffLObj.localPosition.x, _volEffLObj.localPosition.y);
        //キャンバス取得
        _volEffUObjCG = _volEffUObj.GetComponent<CanvasGroup>();
        _volEffLObjCG = _volEffLObj.GetComponent<CanvasGroup>();

        TitleLogoImageAnim();
    }

    [Button]
    private void TitleLogoImageAnim()
    {
        //メガホン
        var sequenceMega = DOTween.Sequence();
        //メガホン拡大
        sequenceMega.Append(_volMegahoneObj.DOScale(new Vector2(1.3f,1.3f),_upTime).SetEase(Ease.OutQuad));
        //終わったらメガホン縮小
        sequenceMega.Append(_volMegahoneObj.DOScale(new Vector2(1.0f, 1.0f), _downTime).SetEase(Ease.OutElastic));

        var sequenceL = DOTween.Sequence();
        //終わったら波形が飛ぶ
        sequenceL.Append(_volEffLObj.DOAnchorPos(new Vector2(_volEffLObj.localPosition.x + _volOffset.x, _volEffLObj.localPosition.y + _volOffset.y), _flyTime));
        //少しずらして波形消えていく
        sequenceL.Join(_volEffLObjCG.DOFade(0, _flyTime / 4).SetDelay(_flyTime / 2));
        //消え終わったら位置を戻す
        sequenceL.Append(_volEffLObj.DOAnchorPos(new Vector2(_volEffLObjTra.x, _volEffLObjTra.y), 0.1f));
        //波形出現
        sequenceL.Append(_volEffLObjCG.DOFade(1, 0.2f));

        var sequenceU = DOTween.Sequence();
        //終わったら波形が飛ぶ
        sequenceU.Append(_volEffUObj.DOAnchorPos(new Vector2(_volEffUObj.localPosition.x + _volOffset.x, _volEffUObj.localPosition.y + _volOffset.y), _flyTime));
        //少しずらして波形消えていく
        sequenceU.Join(_volEffUObjCG.DOFade(0, _flyTime / 4).SetDelay(_flyTime / 2));
        //消え終わったら位置を戻す
        sequenceU.Append(_volEffUObj.DOAnchorPos(new Vector2(_volEffUObjTra.x, _volEffUObjTra.y), 0.1f));
        //波形出現
        sequenceU.Append(_volEffUObjCG.DOFade(1, 0.2f));

        var sequencePlay = DOTween.Sequence();
        sequencePlay.Append(sequenceU);
        sequencePlay.Join(sequenceL.SetDelay(0.1f));
        sequencePlay.PrependInterval(_upTime+_downTime-0.15f);
        sequencePlay.Join(sequenceMega);
        sequencePlay.AppendInterval(_interval/2);
        sequencePlay.SetLoops(-1);

        //たまに動くもの　インターバルを使用するもの
        var sequenceInt = DOTween.Sequence();
        sequenceInt.Join(_volt2Obj.DOAnchorPosY(100, 0.1f).SetEase(Ease.OutCirc).SetLoops(4, LoopType.Yoyo));
        sequenceInt.PrependInterval(0.1f);
        sequenceInt.Join(_volt1Obj.DOAnchorPosY(100, 0.1f).SetEase(Ease.OutCirc).SetLoops(4, LoopType.Yoyo));
        sequenceInt.Join(_rimoconRectTra.DOAnchorPosY(25, 0.1f).SetEase(Ease.OutCirc).SetLoops(2, LoopType.Yoyo));
        sequenceInt.AppendInterval(_interval);
        sequenceInt.SetLoops(-1);

        //Playで実行
        sequenceInt.Play();
        sequencePlay.Play();
        


        //常に動くもの
        _laughRectTra.DOLocalRotate(new Vector3(0, 0, 0), _lowChangeTime).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        _titleRectTra.DOScale(new Vector2(1.05f, 1.05f), _lowChangeTime).SetEase(Ease.InOutQuint).SetLoops(-1, LoopType.Yoyo);
    } 

    private void LAnimation()
    {
        var sequenceL = DOTween.Sequence();
        //終わったら波形が飛ぶ
        sequenceL.Append(_volEffLObj.DOAnchorPos(new Vector2(_volEffLObj.localPosition.x + 200, _volEffLObj.localPosition.y - 100), _flyTime));
        //少しずらして波形消えていく
        sequenceL.Join(_volEffLObjCG.DOFade(0, _flyTime / 4).SetDelay(_flyTime / 2));
        //消え終わったら位置を戻す
        sequenceL.Append(_volEffLObj.DOAnchorPos(new Vector2(_volEffLObjTra.x, _volEffLObjTra.y), 0.1f));
        //波形出現
        sequenceL.Append(_volEffLObjCG.DOFade(1, 0.2f));
        sequenceL.Play();
    }
    private void UAnimation()
    {
        var sequenceL = DOTween.Sequence();
        //終わったら波形が飛ぶ
        sequenceL.Append(_volEffLObj.DOAnchorPos(new Vector2(_volEffLObj.localPosition.x + 200, _volEffLObj.localPosition.y - 100), _flyTime));
        //少しずらして波形消えていく
        sequenceL.Join(_volEffLObjCG.DOFade(0, _flyTime / 4).SetDelay(_flyTime / 2));
        //消え終わったら位置を戻す
        sequenceL.Append(_volEffLObj.DOAnchorPos(new Vector2(_volEffLObjTra.x, _volEffLObjTra.y), 0.1f));
        //波形出現
        sequenceL.Append(_volEffLObjCG.DOFade(1, 0.2f));
        sequenceL.Play();
    }
}
