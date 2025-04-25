using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class NoteController : MonoBehaviour
{
    #region 変数

    [Header("パラメーター")]
    [Tooltip("フレームのデフォルトサイズ"), SerializeField]
    private Vector3 _defaltFrameSize = new Vector3(2, 2, 2);

    [Tooltip("GOOD判定の距離"), SerializeField]
    private float _goodJudgmentRange = 0.1f;

    private float _distance;

    [Header("オブジェクト参照")]
    [Tooltip("フレームのオブジェクトを設定してください"), SerializeField]
    private GameObject _frame;

    [Tooltip("ボディのオブジェクトを設定してください"), SerializeField]
    private GameObject _body;

    [Tooltip("判定を表示するためのオブジェクトを設定してください"), SerializeField]
    private GameObject _judgmentText;

    /// <summary>判定表示のテキストを格納</summary>
    private TextMesh _t;

    #endregion

    #region 関数

    /// <summary>
    /// 判定とその結果を表示する
    /// </summary>
    public void JudgementNote()
    {
        if (Mathf.Abs(_distance) <= _goodJudgmentRange)
        {
            _frame.SetActive(false);
            _body.SetActive(false);
            _t.text = "GOOD!";
            Destroy(this.gameObject, 1.0f);
        }
        else
        {
            _frame.SetActive(false);
            _body.SetActive(false);
            _t.text = "MISS...";
            Destroy(this.gameObject, 1.0f);
        }
        _judgmentText.SetActive(true);
    }

    /// <summary>
    /// このノートを削除する 
    /// </summary>
    private void DeleteThisNote()
    {
        Destroy(this.gameObject);
    }

    #endregion

    #region イベント関数

    // Start is called before the first frame update
    void Start()
    {
        _frame.transform.localScale = _defaltFrameSize;
        _t = _judgmentText.GetComponent<TextMesh>();
    }

    void Update()
    {
        _frame.transform.localScale -= Vector3.one * Time.deltaTime;
        _distance = _body.transform.localScale.x - _frame.transform.localScale.x;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JudgementNote();
        }

        if (_frame.transform.localScale.x < -0.5f)
        {
            DeleteThisNote();
        }
    }

    #endregion
}
