using DG.Tweening;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleLogoAnim : MonoBehaviour
{
    [SerializeField,Label("アニメーションインターバル")]
    private float _interval = 1.2f;
    [SerializeField, Label("常に動くものの片道の時間")]
    private float _lowChangeTime = 2f;
    [SerializeField, Label("タイトルが拡大したときの大きさ")]
    private float _titleUpSize = 1.05f;
    [SerializeField, Label("メガホンが拡大したときの大きさ")]
    private float _upSize = 1.3f;
    [SerializeField, Label("メガホンが拡大する時間")]
    private float _upTime = 1.0f;
    [SerializeField, Label("メガホンが縮小する時間")]
    private float _downTime = 0.4f;
    [SerializeField, Label("音量波形が飛ぶ時間")]
    private float _flyTime = 0.5f;
    [SerializeField, Label("音量波形が移動する座標、これを+する")]
    private Vector2 _volOffset = Vector2.zero;

    DG.Tweening.Sequence sequenceMega;
    DG.Tweening.Sequence sequenceL;
    DG.Tweening.Sequence sequenceU;
    DG.Tweening.Sequence sequencePlay;
    DG.Tweening.Sequence sequenceInt;
    DG.Tweening.Sequence sequenceAlw;

    #region オブジェクト変数

    //リモコンから出る電波
    [SerializeField, Foldout("Obj")]
    private RectTransform _volt1Obj;
    [SerializeField, Foldout("Obj")]
    private RectTransform _volt2Obj;

    //音量イメージ：メガホン
    [SerializeField, Foldout("Obj")]
    private RectTransform _volMegahoneObj;

    //音量イメージ：音波
    [SerializeField, Foldout("Obj")]
    private RectTransform _volEffLObj;
    //フェード用
    private CanvasGroup _volEffLObjCG;
    //位置保存用
    private Vector2 _volEffLObjTra;

    [SerializeField, Foldout("Obj")]
    private RectTransform _volEffUObj;
    //フェード用
    private CanvasGroup _volEffUObjCG;
    //位置保存用
    private Vector2 _volEffUObjTra;

    //上のオレンジのワイワイしてるもの
    [SerializeField, Foldout("Obj")]
    private RectTransform _laughRectTra;

    //タイトルテキスト画像
    [SerializeField, Foldout("Obj")]
    private RectTransform _titleRectTra;

    //リモコン
    [SerializeField, Foldout("Obj")]
    private RectTransform _rimoconRectTra;

    #endregion

    void Start()
    {
        //初期位置保存
        _volEffUObjTra = new Vector2(_volEffUObj.localPosition.x, _volEffUObj.localPosition.y);
        _volEffLObjTra = new Vector2(_volEffLObj.localPosition.x, _volEffLObj.localPosition.y);
        //キャンバス取得
        _volEffUObjCG = _volEffUObj.GetComponent<CanvasGroup>();
        _volEffLObjCG = _volEffLObj.GetComponent<CanvasGroup>();

        //アニメーション実行
        PlayTitleLogoImageAnim();
    }

    [Button]
    public void SetTitleLogoImageAnim()
    {
        //シーケンス初期設定
        sequenceMega = DOTween.Sequence();
        sequenceL = DOTween.Sequence();
        sequenceU = DOTween.Sequence();
        sequencePlay = DOTween.Sequence();
        sequenceInt = DOTween.Sequence();
        sequenceAlw = DOTween.Sequence();

        //メガホン
        //メガホン拡大
        sequenceMega.Append(_volMegahoneObj.DOScale(new Vector2(_upSize, _upSize),_upTime).SetEase(Ease.OutQuad));
        //終わったらメガホン縮小
        sequenceMega.Append(_volMegahoneObj.DOScale(new Vector2(1.0f, 1.0f), _downTime).SetEase(Ease.OutElastic));

        //内側の波形
        //終わったら波形が飛ぶ
        sequenceL.Append(_volEffLObj.DOAnchorPos(new Vector2(_volEffLObj.localPosition.x + _volOffset.x, _volEffLObj.localPosition.y + _volOffset.y), _flyTime));
        //少しずらして波形消えていく
        sequenceL.Join(_volEffLObjCG.DOFade(0, _flyTime / 4).SetDelay(_flyTime / 2));
        //消え終わったら位置を戻す
        sequenceL.Append(_volEffLObj.DOAnchorPos(new Vector2(_volEffLObjTra.x, _volEffLObjTra.y), 0.1f));
        //波形出現
        sequenceL.Append(_volEffLObjCG.DOFade(1, 0.2f));

        //外側の波形
        //終わったら波形が飛ぶ
        sequenceU.Append(_volEffUObj.DOAnchorPos(new Vector2(_volEffUObj.localPosition.x + _volOffset.x, _volEffUObj.localPosition.y + _volOffset.y), _flyTime));
        //少しずらして波形消えていく
        sequenceU.Join(_volEffUObjCG.DOFade(0, _flyTime / 4).SetDelay(_flyTime / 2));
        //消え終わったら位置を戻す
        sequenceU.Append(_volEffUObj.DOAnchorPos(new Vector2(_volEffUObjTra.x, _volEffUObjTra.y), 0.1f));
        //波形出現
        sequenceU.Append(_volEffUObjCG.DOFade(1, 0.2f));

        //メガホンのアニメーションを結合する
        //波形が飛ぶアニメーションをずらす
        sequencePlay.Append(sequenceU);
        sequencePlay.Join(sequenceL.SetDelay(0.1f));
        //メガホンが収縮しきる少しだけ前に波形を再生する
        sequencePlay.PrependInterval(_upTime+_downTime-0.15f);
        sequencePlay.Join(sequenceMega);
        //インターバル
        sequencePlay.AppendInterval(_interval/2);
        sequencePlay.SetLoops(-1);

        //たまに動くもの　インターバルを使用するもの
        //電波がピコピコする
        sequenceInt.Join(_volt2Obj.DOAnchorPosY(100, 0.1f).SetEase(Ease.OutCirc).SetLoops(4, LoopType.Yoyo));
        sequenceInt.PrependInterval(0.1f);
        sequenceInt.Join(_volt1Obj.DOAnchorPosY(100, 0.1f).SetEase(Ease.OutCirc).SetLoops(4, LoopType.Yoyo));
        //リモコンが上にはねる
        sequenceInt.Join(_rimoconRectTra.DOAnchorPosY(25, 0.1f).SetEase(Ease.OutCirc).SetLoops(2, LoopType.Yoyo));
        //インターバル
        sequenceInt.AppendInterval(_interval);
        sequenceInt.SetLoops(-1);

        //常に動くもの
        sequenceAlw.Append(_laughRectTra.DOLocalRotate(new Vector3(0, 0, 0), _lowChangeTime).SetEase(Ease.InOutCubic).SetLoops(2, LoopType.Yoyo));
        sequenceAlw.Join(_titleRectTra.DOScale(new Vector2(_titleUpSize, _titleUpSize), _lowChangeTime).SetEase(Ease.InOutQuint).SetLoops(2, LoopType.Yoyo));
        sequenceAlw.SetLoops(-1);
    }

    [Button]
    public void PlayTitleLogoImageAnim()
    {
        //Playで実行
        SetTitleLogoImageAnim();
        sequenceInt.Play();
        sequencePlay.Play();
        sequenceAlw.Play();
    }

    [Button]
    public void StopTitleLogoImageAnim()
    {
        //Killで終了
        sequenceInt.Kill(true);
        sequencePlay.Kill(true);
        sequenceAlw.Kill(true);
    }
}
