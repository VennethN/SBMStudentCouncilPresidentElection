using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioExtension : MonoBehaviour
{
    public void playAudioEffect(string soundName)
    {
        AudioManager.PlayAudioEffect(soundName);
    }
}
