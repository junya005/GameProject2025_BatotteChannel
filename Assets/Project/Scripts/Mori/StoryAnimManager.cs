using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoryAnimManager : MonoBehaviour
{
    #region Obj変数
    [Header("シーン親オブジェクト")]
    [SerializeField, Foldout("1st")]
    private CanvasGroup scene1;
    [Header("シーン親オブジェクト")]
    [SerializeField, Foldout("2nd")]
    private CanvasGroup scene2;
    [Header("シーン親オブジェクト")]
    [SerializeField, Foldout("3rd")]
    private CanvasGroup scene3;
    [Header("シーン親オブジェクト")]
    [SerializeField, Foldout("4th")]
    private CanvasGroup scene4;

    [Header("アニメーションオブジェクト")]
    [SerializeField, Foldout("1st")]
    private RectTransform sister_1st;
    [SerializeField, Foldout("1st")]
    private RectTransform tv_1st;
    [SerializeField, Foldout("1st")]
    private RectTransform pi_1st;

    [Header("アニメーションオブジェクト")]
    [SerializeField, Foldout("2nd")]
    private RectTransform brother_2nd;
    [SerializeField, Foldout("2nd")]
    private RectTransform tv_2nd;
    [SerializeField, Foldout("2nd")]
    private RectTransform pi_2nd;

    [Header("アニメーションオブジェクト")]
    [SerializeField, Foldout("3rd")]
    private RectTransform sister_3rd;
    [SerializeField, Foldout("3rd")]
    private RectTransform brother_3rd;
    [SerializeField, Foldout("3rd")]
    private RectTransform tv_3rd;
    [SerializeField, Foldout("3rd")]
    private List<RectTransform> piList = new List<RectTransform>();

    [Header("アニメーションオブジェクト")]
    [SerializeField, Foldout("4th")]
    private RectTransform sisbro_4th;

    private AudioSource audioSource => this.GetComponent<AudioSource>();
    [SerializeField, Foldout("SE")]
    [Header("効果音")]
    private AudioClip _sePi;
    [SerializeField, Foldout("SE")]
    private AudioClip _seSpark;
    #endregion

    #region パラメータ変数
    [Header("パラメータ")]
    [SerializeField, Foldout("General"), Label("スライドインにかかる時間/秒")]
    private float slideInTime = 0.5f;
    [SerializeField, Foldout("General"), Label("ピがでるのにかかる時間/秒")]
    private float PiScaleUpTime = 0.2f;
    [SerializeField, Foldout("General"), Label("フェードインにかかる時間/秒")]
    private float objFadeTime = 0.2f;
    [SerializeField, Foldout("General"), Label("次シーンにフェードするまでの待機時間/秒")]
    private float fadeDelay = 0.5f;
    [SerializeField, Foldout("General"), Label("アニメーション終わりの待機時間/秒")]
    private float finishDelay = 1.5f;
    [SerializeField, Foldout("General"), Label("スライドインの動き方")]
    private Ease ease = Ease.OutCubic;

    #endregion

    #region 内部変数
    DG.Tweening.Sequence Allsequence;
    DG.Tweening.Sequence sequence1;
    DG.Tweening.Sequence sequence2;
    DG.Tweening.Sequence sequence3;
    DG.Tweening.Sequence sequence4;

    //1番目
    private float sisY1;
    private float tvY1;
    //2番目
    private float broY2;
    private float tvY2;
    //3番目
    private float sisY3;
    private float broY3;
    private float tvY3;
    //4番目
    private float sisbroY4;
    #endregion

    void Start()
    {
        //TweenAnimation();
    }

    void Update()
    {

    }

    /// <summary>
    /// Tweenリセット
    /// </summary>
    [Button]
    public void TweenReset()
    {
        scene1.alpha = 0f;
        scene2.alpha = 0;
        scene3.alpha = 0;
        scene4.alpha = 0;
        audioSource.Stop();
    }

    /// <summary>
    /// Tweenスキップ
    /// </summary>
    [Button]
    public void TweenSkip()
    {
        //Tweenスキップ(兄弟喧嘩図まで)
        Allsequence.Complete();
        Allsequence.Append(DOVirtual.DelayedCall(finishDelay, CompleteAnimation));
        audioSource.Stop();
    }

    [Button]
    /// <summary>
    /// 全体tween実行(リセット、設定、実行)
    /// </summary>
    public void TweenAnimation()
    {
        TweenReset();
        //設定
        SetFirstAnim();
        SetSecondAnim();
        SetThirdAnim();
        SetFourthAnim();

        //1番目から実行
        Allsequence = DOTween.Sequence();
        Allsequence.Append(sequence1);
        Allsequence.Append(sequence2);
        Allsequence.Append(sequence3);
        Allsequence.Append(sequence4);
        Allsequence.Append(DOVirtual.DelayedCall(finishDelay, CompleteAnimation));
        Allsequence.Play();
    }

    private void CompleteAnimation()
    {
        scene4.DOFade(0, objFadeTime);
        //終了時に呼ぶ処理


    }

    /// <summary>
    /// 1st設定
    /// </summary>
    private void SetFirstAnim()
    {
        //position保存
        sisY1 = sister_1st.localPosition.y;
        tvY1 = tv_1st.localPosition.y;
        //初期座標、サイズ設定
        sister_1st.localPosition = new Vector2(sister_1st.localPosition.x, -1000);
        tv_1st.localPosition = new Vector2(tv_1st.localPosition.x, 1000);
        pi_1st.localScale = new Vector2(0.0f, 0.0f);


        sequence1 = DOTween.Sequence();

        //一瞬でフェードイン
        sequence1.Append(scene1.DOFade(1, objFadeTime).SetDelay(fadeDelay));
        //姉くる
        sequence1.Append(sister_1st.DOAnchorPosY(sisY1, slideInTime).SetEase(ease));
        //テレビくる
        sequence1.Append(tv_1st.DOAnchorPosY(tvY1, slideInTime).SetEase(ease));
        //ピ
        sequence1.Append(pi_1st.DOScale(Vector2.one, PiScaleUpTime).SetEase(Ease.OutBack));
        sequence1.JoinCallback(() => { audioSource.PlayOneShot(_sePi); });
        //一瞬でフェードアウト
        sequence1.Append(scene1.DOFade(0, objFadeTime).SetDelay(fadeDelay));
    }

    /// <summary>
    /// 2nd設定
    /// </summary>
    private void SetSecondAnim()
    {
        //position保存
        broY2 = brother_2nd.localPosition.y;
        tvY2 = tv_2nd.localPosition.y;
        //初期座標、サイズ設定
        brother_2nd.localPosition = new Vector2(brother_2nd.localPosition.x, 1200);
        tv_2nd.localPosition = new Vector2(tv_2nd.localPosition.x, -1200);
        pi_2nd.localScale = new Vector2(0.0f, 0.0f);

        sequence2 = DOTween.Sequence();

        //一瞬でフェードイン
        sequence2.Append(scene2.DOFade(1, objFadeTime).SetDelay(fadeDelay));
        //弟くる
        sequence2.Append(brother_2nd.DOAnchorPosY(broY2, slideInTime).SetEase(ease));
        //テレビくる
        sequence2.Append(tv_2nd.DOAnchorPosY(tvY2, slideInTime).SetEase(ease));
        //ピ
        sequence2.Append(pi_2nd.DOScale(Vector2.one, PiScaleUpTime).SetEase(Ease.OutBack));
        sequence2.JoinCallback(() => { audioSource.PlayOneShot(_sePi); });
        //一瞬でフェードアウト
        sequence2.Append(scene2.DOFade(0, objFadeTime).SetDelay(fadeDelay));
    }

    /// <summary>
    /// 3rd設定
    /// </summary>
    private void SetThirdAnim()
    {
        //position保存
        broY3 = brother_3rd.localPosition.y;
        sisY3 = sister_3rd.localPosition.y;
        tvY3 = tv_3rd.localPosition.y;
        //初期座標、サイズ設定
        sister_3rd.localPosition = new Vector2(sister_3rd.localPosition.x, -1000);
        brother_3rd.localPosition = new Vector2(brother_3rd.localPosition.x, -1000);
        tv_3rd.localPosition = new Vector2(tv_3rd.localPosition.x, 1000);
        for(int i = 0; i < piList.Count; i++)
        {
            piList[i].localScale = new Vector2(0.0f, 0.0f);
        }

        sequence3 = DOTween.Sequence();

        //一瞬でフェードイン
        sequence3.Append(scene3.DOFade(1, objFadeTime).SetDelay(fadeDelay));
        //全部同時
        //弟くる
        sequence3.Append(brother_3rd.DOAnchorPosY(broY2, slideInTime).SetEase(ease));
        //姉くる
        sequence3.Join(sister_3rd.DOAnchorPosY(broY2, slideInTime).SetEase(ease));
        //テレビくる
        sequence3.Join(tv_3rd.DOAnchorPosY(tvY2, slideInTime).SetEase(ease));

        //ピピピピ
        for (int i = 0; i < piList.Count; i++)
        {
            sequence3.Append(piList[i].DOScale(Vector2.one, PiScaleUpTime).SetEase(Ease.OutBack));
            sequence3.JoinCallback(() => { audioSource.PlayOneShot(_sePi); });
        }

        //一瞬でフェードアウト
        sequence3.Append(scene3.DOFade(0, objFadeTime).SetDelay(fadeDelay));
    }

    /// <summary>
    /// 4th設定
    /// </summary>
    private void SetFourthAnim()
    {
        //position保存
        sisbroY4 = sisbro_4th.localPosition.y;
        //初期座標、サイズ設定
        sisbro_4th.localPosition = new Vector2(sisbro_4th.localPosition.x, 1500);

        sequence4 = DOTween.Sequence();

        //一瞬でフェードイン
        sequence4.Append(scene4.DOFade(1, objFadeTime).SetDelay(fadeDelay));
        //上から喧嘩
        sequence4.Append(sisbro_4th.DOAnchorPosY(sisbroY4, slideInTime).SetEase(ease));
        sequence4.AppendCallback(() => { audioSource.PlayOneShot(_seSpark); });
    }
}
