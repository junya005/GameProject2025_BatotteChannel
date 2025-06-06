using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using Cysharp.Threading.Tasks;

public class JudgeEffectControll : MonoBehaviour
{
    [SerializeField, Label("Debugモード")]
    private bool _isDebugMode = false;
    [SerializeField, Label("正誤判定の画像")]
    private GameObject _uiJudgeSprite;
    private SpriteRenderer _uiJudgeSpriteRenderer;

    [SerializeField, Label("PeaticleEffect")]
    private GameObject _effect;


    [SerializeField, Label("文字のアニメーション時間/秒")]
    private float _animTime = 0.15f;
    [SerializeField, Label("Goodを表示する時間/秒")]
    private float _waitTime = 0.4f;
    [SerializeField, Label("文字が消えるのにかかる時間/秒")]
    private float _fadeOutTime = 0.35f;

    void Start()
    {
        //エフェクト発生(関数呼び出し)
        JudgeEffectAnim();
    }

    [Button]
    public async void JudgeEffectAnim()
    {
        //サイズを0.25に初期設定
        _uiJudgeSprite.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        //画像取得(透明用)
        _uiJudgeSpriteRenderer = _uiJudgeSprite.GetComponent<SpriteRenderer>();
        //画像透明度初期化
        _uiJudgeSpriteRenderer.color = new Color(1, 1, 1, 1);

        //エフェクトパーティクル発生
        if( _effect != null)
        {
            Instantiate(_effect, transform.position, Quaternion.identity);
        }
        //サイズを1.2まで拡大したのち1に収束
        await _uiJudgeSprite.transform.DOScale(new Vector3(1, 1, 1), _animTime).SetEase(Ease.OutBack);
        //n秒待つ
        await UniTask.Delay((int)(_waitTime * 1000));
        //m秒でフェードアウト
        await DOVirtual.Float(
          from: 1f,
          to: 0f,
          duration: _fadeOutTime,
          onVirtualUpdate: (tweenValue) => {
              _uiJudgeSpriteRenderer.color = new Color(1, 1, 1, tweenValue);
          }
        );
        if (_isDebugMode == false)
        {
            //ゲームオブジェクトを消去
            Destroy(this.gameObject);
        }
    }
}
