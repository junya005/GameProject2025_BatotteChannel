using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class KeyboardExample : MonoBehaviour
{
    //inputActionの格納
    private PlayerInput _playerInput;
    private InputAction _button0;
    private InputAction _button1;
    private InputAction _button2;
    private InputAction _button3;
    private InputAction _button4;
    private InputAction _button5;
    private InputAction _button6;
    private InputAction _button7;
    private InputAction _button8;
    private InputAction _button9;
    private InputAction _volumePlus;
    private InputAction _volumeMinus;
    private InputAction _buttonEnter;

    private void Start()
    {
        if (_playerInput == null)
        {
            _playerInput = GetComponent<PlayerInput>();
            _button0     = _playerInput.actions["Button0"];
            _button1     = _playerInput.actions["Button1"];
            _button2     = _playerInput.actions["Button2"];
            _button3     = _playerInput.actions["Button3"];
            _button4     = _playerInput.actions["Button4"];
            _button5     = _playerInput.actions["Button5"];
            _button6     = _playerInput.actions["Button6"];
            _button7     = _playerInput.actions["Button7"];
            _button8     = _playerInput.actions["Button8"];
            _button9     = _playerInput.actions["Button9"];
            _volumePlus  = _playerInput.actions["VolumePlus"];
            _volumeMinus = _playerInput.actions["VolumeMinus"];
        }
    }

    private void Update()
    {
        if (_button0.triggered)
        {
            Debug.Log($"{Keyboard.current}:0キーが押された！");
        }

        if (_button1.triggered)
        {
            Debug.Log($"{Keyboard.current}:1キーが押された！");
        }

        if (_button2.triggered)
        {
            Debug.Log($"{Keyboard.current}:2キーが押された！");
        }
        
        if (_button3.triggered)
        {
            Debug.Log($"{Keyboard.current}:3キーが押された！");
        }

        if (_button4.triggered)
        {
            Debug.Log($"{Keyboard.current}:4キーが押された！");
        }

        if (_button5.triggered)
        {
            Debug.Log($"{Keyboard.current}:5キーが押された！");
        }

        if (_button6.triggered)
        {
            Debug.Log($"{Keyboard.current}:6キーが押された！");
        }

        if (_button7.triggered)
        {
            Debug.Log($"{Keyboard.current}:7キーが押された！");
        }

        if (_button8.triggered)
        {
            Debug.Log($"{Keyboard.current}:8キーが押された！");
        }

        if (_button9.triggered)
        {
            Debug.Log($"{Keyboard.current}:9キーが押された！");
        }
    }
}
