using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ChannelChangeAnim : MonoBehaviour
{
    //画像イメージ
    private Image _image;
    private CanvasGroup _imageCG;
    //差し替え用エフェクトイメージ
    [SerializeField,Label("姉用イメージ")]
    private Sprite _sisChannelSprite;
    [SerializeField, Label("弟用イメージ")]
    private Sprite _broChannelSprite;
    //エフェクトが消えるまでの時間
    [SerializeField]
    private float _duration = 0.5f;

    /// <summary>
    /// debug　プレイヤー1用を発生するどうか、true=Player1 false=Player2
    /// </summary>
    [SerializeField]
    private bool _player1;

    private Tween _tween;

    void Start()
    {
        _image = GetComponent<Image>();
        _imageCG = _image.GetComponent<CanvasGroup>();
        _imageCG.alpha = 0;
    }
    
    //debug
    [Button]
    private void ChangeAnimTest()
    {
        ChangeAnim(_player1);
    }

    /// <summary>
    /// テレビのチャンネル切替エフェクト発生
    /// 引数：true=Sister false=Brother
    /// 連打可能
    /// </summary>
    /// <param name="player1">true=Sister false=Brother</param>
    public void ChangeAnim(bool player1)
    {
        _tween.Kill(true);
        _imageCG.alpha = 0;
        if (player1)
        {
            _image.sprite = _sisChannelSprite;
        }
        else
        {
            _image.sprite= _broChannelSprite;
        }
        _imageCG.alpha = 1;
        _tween = _imageCG.DOFade(0, _duration).SetEase(Ease.InOutCubic);
        _tween.Play();
    }
}
