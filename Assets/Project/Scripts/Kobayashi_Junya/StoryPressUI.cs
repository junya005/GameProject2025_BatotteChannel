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

    [SerializeField]
    private GameObject _pressedUIP1;

    [SerializeField]
    private GameObject _pressedUIP2;

    void Start()
    {
        _pressReadyUIP1.SetActive(true);
        _pressReadyUIP2.SetActive(true);
        _pressedUIP1.SetActive(false);
        _pressedUIP2.SetActive(false);
    }

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
}
