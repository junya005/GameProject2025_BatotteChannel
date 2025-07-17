using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GoodEffectFireSircleAnim : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _effectslist=new List<GameObject>();
    [SerializeField]
    private List<SpriteRenderer> _spriteslist=new List<SpriteRenderer>();

    private List<Tween> _tweenslist = new List<Tween>();

    [SerializeField]
    private float _delay = 0.5f;
    [SerializeField]
    private float _openTime = 0.5f;
    [SerializeField]
    private float _fadeTime = 0.3f;

    void Start()
    {
        PlayAnim();
    }

    [Button]
    private void PlayAnim()
    {
        //‘S‚Ä‚ðˆêue‚ÌÀ•W‚ÉŒü‚©‚í‚¹‚é
        //‚»‚ÌŒãŽUŠJ‚·‚é
        for (int i = 0; _effectslist.Count > i; i++)
        {
            _tweenslist.Add(_effectslist[i].transform.DOLocalMove(new Vector2(_effectslist[i].transform.localPosition.x * 2, _effectslist[i].transform.localPosition.y * 2), _openTime));
            _tweenslist.Add(_spriteslist[i].DOFade(0, _fadeTime).SetDelay(_delay));
        }
    }

    private void Destroy()
    {
        DOTween.Kill(true);
        Destroy(this.gameObject);
    }
}
