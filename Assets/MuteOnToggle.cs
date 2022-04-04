using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteOnToggle : MonoBehaviour
{
    public AudioSource audio;

    public void ToggleMute()
    {
        audio.mute = !audio.mute;
    }
}
