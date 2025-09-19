using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryPressUI : MonoBehaviour
{
    /// <summary>Player1がEnterを押したかを表示するImage</summary>
    [SerializeField]
    private GameObject _pressReadyUIP1;

    /// <summary>Player2がEnterを押したかを表示するImage</summary>
    [SerializeField]
    private GameObject _pressReadyUIP2;

    /// <summary>押されたUI(Player1)</summary>
    [SerializeField]
    private GameObject _pressedUIP1;

    /// <summary>押されたUI(Player1)</summary>
    [SerializeField]
    private GameObject _pressedUIP2;

    void Start()
    {
        ResetPressUI();
    }

    /// <summary>
    /// 押下された状態を表示する
    /// </summary>
    /// <param name="playerNumber"></param>
    public void DisplayPress(PlayerNumberState playerNumber)
    {
        if (playerNumber == PlayerNumberState.One)
        {
            _pressReadyUIP1.SetActive(false);
            _pressedUIP1.SetActive(true);
            return;
        }

        _pressReadyUIP2.SetActive(false);
        _pressedUIP2.SetActive(true);
    }

    /// <summary>
    /// 押下状態UIを初期状態に設定
    /// </summary>
    public void ResetPressUI()
    {
        _pressReadyUIP1.SetActive(true);
        _pressReadyUIP2.SetActive(true);
        _pressedUIP1.SetActive(false);
        _pressedUIP2.SetActive(false);
    }
}
