using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/AudioData")]
public class AudioDataScriptableObject : ScriptableObject
{
    public List<AudioClip> BGMList;
    public List<AudioClip> SEList;
}
