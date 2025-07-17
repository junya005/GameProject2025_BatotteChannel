using System.Collections;
using System.Collections.Generic;
using BatotteChannel.AudioSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class InputModuleActions : MonoBehaviour
{
    private GameObject _currentSelectedGameObject;
    public GameObject CurrentSelectedObject
    {
        get { return _currentSelectedGameObject; }
        set
        {
            if (_titleSelectManager.GameSceneState != GameStatus.GameSceneEnum.Select) return;
            if (_currentSelectedGameObject == value) return;
            PlaySelectSE();
            _currentSelectedGameObject = value;
        }
    }

    [SerializeField]
    private EventSystem _eventSystem;

    [SerializeField]
    private TitleSelectManager _titleSelectManager;

    // Start is called before the first frame update
    void Start()
    {
        _currentSelectedGameObject = _eventSystem.firstSelectedGameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentSelectedObject = _eventSystem.currentSelectedGameObject;
    }

    public void PlaySelectSE()
    {
        SoundManager.Instance.PlaySE("push_determining_button_2");
    }
}
