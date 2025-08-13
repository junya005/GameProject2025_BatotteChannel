using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleStartImageAnim : MonoBehaviour
{
    //Tweenにかかる時間
    [SerializeField]
    private float _duration = 0.8f;
    //フェードのインターバル
    [SerializeField]
    private float _interval = 1.2f;

    private Image _image;
    private CanvasGroup _imageCG;

    [SerializeField]
    private float _switchImageTime = 1;
    private float _switchImageValTime;
    [SerializeField, Label("エンター押されていないスタートテキスト画像")]
    private Sprite _enterTextImage;
    [SerializeField, Label("エンター押されているスタートテキスト画像")]
    private Sprite _enterPushTextImage;

    void Start()
    {
        //Renderer設定とアニメーションスタート
        _image = GetComponent<Image>();
        _imageCG=_image.GetComponent<CanvasGroup>();
        _image.sprite=_enterTextImage;
        _switchImageValTime = _switchImageTime;
        TitleTextImageAnim();
    }

    private void Update()
    {
        //画像のエンター押下状態の切替
        if (_switchImageValTime > 0)
        {
            _switchImageValTime -= Time.deltaTime;
        }
        else
        {
            if (_image.sprite == _enterTextImage)
            {
                _image.sprite = _enterPushTextImage;
            }
            else
            {
                _image.sprite = _enterTextImage;
            }
            _switchImageValTime = _switchImageTime;
        }
    }

    /// <summary>
    /// タイトルの文のフェードアニメーション
    /// </summary>
    public void TitleTextImageAnim()
    {
        //Enterを押す画像切替のアニメーション

        //シーケンス作成
        var sequence = DOTween.Sequence();
        //フェードインアウト
        sequence.Append(_imageCG.DOFade(0,_duration).SetEase(Ease.InOutCubic).SetLoops(2, LoopType.Yoyo));
        //インターバル設定
        sequence.AppendInterval(_interval);
        //無限ループ
        sequence.SetLoops(-1);
        //シーケンス実行
        sequence.Play();
    }
}
