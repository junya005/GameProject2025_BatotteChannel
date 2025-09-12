using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCharacterAnimation : MonoBehaviour
{
    // プレイヤーの定義
    [SerializeField]
    private Animator _animatorP1;

    [SerializeField]
    private Animator _animatorP2;

    public void StartSpeakAnimation(bool isPlayerOne)
    {
        if (isPlayerOne)
        {
            _animatorP1.SetBool("isSpeak", true);
            _animatorP2.SetBool("isSpeak", false);
            return;
        }

        _animatorP1.SetBool("isSpeak", false);
        _animatorP2.SetBool("isSpeak", true);
    }
}
