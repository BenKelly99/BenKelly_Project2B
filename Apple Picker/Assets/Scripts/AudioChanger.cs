using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioChanger : MonoBehaviour
{
    [SerializeField]
    Slider volSlider;
    [SerializeField]
    Toggle soundOn;
    [SerializeField]
    string keyVol = null, keyMute = null;
    bool awoken;
    void Awake()
    {
        awoken = false;
        volSlider.value = PlayerPrefs.GetFloat(keyVol);
        soundOn.isOn = PlayerPrefs.GetInt(keyMute) == 1;
        awoken = true;
        UpdateVolume();
    }
    public void UpdateVolume()
    {
        if (!awoken)
            return;
        GetComponent<AudioSource>().volume = volSlider.value;
        GetComponent<AudioSource>().mute = !soundOn.isOn;
        PlayerPrefs.SetFloat(keyVol, volSlider.value);
        PlayerPrefs.SetInt(keyMute, soundOn.isOn ? 1 : 0);
    }
}
