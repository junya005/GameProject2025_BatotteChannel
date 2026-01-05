using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrobalUIManager : Singleton<GrobalUIManager>
{
    [SerializeField]
    private ModalController _modalController;

    public bool IsModal
    {
        get
        {
            if (_modalController != null)
            {
                return _modalController.IsModal;
            }

            return false;
        }
    }

    public Button ModalDefaultButton
    {
        get
        {
            if (_modalController != null)
            {
                return _modalController.DefaultButton;
            }

            return null;
        }
    }

    public void ShowModal(string message, Action onYes = null, Action onNo = null)
    {
        _modalController.Show(message, onYes, onNo);
    }
}
