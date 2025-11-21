using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Button ReplayButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button XButton;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volPercentageText;
    [SerializeField] private Image Tint;

    private AudioSource[] audioSourcesArray;

    private void Start()
    {
        ReplayButton.onClick.AddListener(OnClickReplayButton);
        SettingsButton.onClick.AddListener(OnClickSettingsButton);
        XButton.onClick.AddListener(OnXButtonClicked);
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);

        StartCoroutine(GetAllAudioSources());

        
    }
    private IEnumerator GetAllAudioSources()
    {
        yield return new WaitForSeconds(0.2f);

        audioSourcesArray = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
    }

    private void OnClickReplayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnClickSettingsButton()
    {
        volumeSlider.value = audioSourcesArray[0].volume;
        UpdateVolumeText(volumeSlider.value);

        Tint.gameObject.SetActive(true);

    }
    private void OnXButtonClicked()
    {
        float vol = volumeSlider.value;
        foreach (var source in audioSourcesArray)
        {
            if(source != null)
            {
                source.volume = volumeSlider.value;
            }
        }

        Tint.gameObject.SetActive(false);
    }
    private void OnVolumeSliderChanged(float value)
    {
        UpdateVolumeText(value);
    }
    private void UpdateVolumeText(float value)
    {
        int percent = Mathf.RoundToInt(value * 100f);
        volPercentageText.text = percent.ToString() + "%";
    }

}
