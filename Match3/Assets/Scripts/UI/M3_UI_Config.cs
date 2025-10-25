using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M3_UI_Config : M3_UI
{
    [SerializeField]
    private Slider _MusicVolumeSlider;
    [SerializeField]
    private Slider _FXVolumeSlider;
    [SerializeField]
    private TextMeshProUGUI _MusicVolumeText;
    [SerializeField]
    private TextMeshProUGUI _FXVolumeText;

    private void Start()
    {
        if (_MusicVolumeSlider != null)
            _MusicVolumeSlider.value = M3_GameController.Instance.GetMusicVolume();
        if (_FXVolumeSlider != null)
            _FXVolumeSlider.value = M3_GameController.Instance.GetFXVolume();
    }

    public void OnMusicVolumeValueChanged()
    {
        if (_MusicVolumeText != null)
            _MusicVolumeText.text = ((int)(_MusicVolumeSlider.value * 100)).ToString() + "%";

        M3_GameController.Instance.SetMusicVolume(_MusicVolumeSlider.value);
    }

    public void OnFXVolumeValueChanged()
    {
        if (_FXVolumeText != null)
            _FXVolumeText.text = ((int)(_FXVolumeSlider.value * 100)).ToString() + "%";

        M3_GameController.Instance.SetMusicVolume(_FXVolumeSlider.value);
    }
}
