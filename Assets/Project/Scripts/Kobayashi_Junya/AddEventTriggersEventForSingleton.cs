using BatotteChannel.AudioSystem;
using UnityEngine;

public class AddEventTriggersEventForSingleton : MonoBehaviour
{
    public void PlayMoveSE()
    {
        Debug.Log("a");
        SoundManager.Instance?.PlaySE("push_determining_button_2");
    }
}
