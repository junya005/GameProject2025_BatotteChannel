using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalController : MonoBehaviour
{
    public bool IsModal { get; private set; }

    public Button DefaultButton { get; private set; }

    [SerializeField]
    private GameObject _modalWindow;

    [SerializeField]
    private TextMeshProUGUI _messageText;

    [SerializeField]
    private Button _yesButton;

    [SerializeField]
    private Button _noButton;

    private Action _onYesButtonClicked;
    private Action _onNoButtonClicked;

    private void Awake()
    {
        DefaultButton = _noButton;

        _yesButton.onClick.AddListener(() => Hide(true));
        _noButton.onClick.AddListener(() => Hide(false));
    }

    public void Show(string message, Action onYes = null, Action onNo = null)
    {
        _messageText.text = message;
        _onYesButtonClicked = onYes;
        _onNoButtonClicked = onNo;

        IsModal = true;
        _modalWindow.SetActive(true);
    }

    private void Hide(bool result)
    {
        if (result)
        {
            _onYesButtonClicked?.Invoke();
            _onYesButtonClicked = null;
        }
        else
        {
            _onNoButtonClicked?.Invoke();
            _onNoButtonClicked = null;
        }

        IsModal = false;
        _modalWindow.SetActive(false);
    }
}
