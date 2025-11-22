using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    int numbercoinsText;

    [SerializeField] private Button ReplayButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button XButton;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volPercentageText;
    [SerializeField] private Image Tint;
    [SerializeField] private TextMeshProUGUI coinsText;

    private AudioSource[] audioSourcesArray;
    private void Start()
    {
        ReplayButton.onClick.AddListener(OnClickReplayButton);
        SettingsButton.onClick.AddListener(OnClickSettingsButton);
        XButton.onClick.AddListener(OnXButtonClicked);
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);

        numbercoinsText = 0;
        coinsText.text = numbercoinsText.ToString();
        StartCoroutine(GetAllAudioSources());

        GameService.Instance.EventService.OnRestart.AddListener(OnRestart);
    }

    private IEnumerator GetAllAudioSources()
    {
        yield return new WaitForSeconds(0.2f);

        audioSourcesArray = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
    }

    private void OnClickReplayButton()
    {
        GameService.Instance.EventService.OnRestart.InvokeEvent();
    }
    private void OnClickSettingsButton()
    {
        volumeSlider.value = audioSourcesArray[0].volume;
        UpdateVolumeText(volumeSlider.value);

        Tint.gameObject.SetActive(true);

    }
    private void OnXButtonClicked()
    {
        Tint.gameObject.SetActive(false);
    }
    private void OnVolumeSliderChanged(float value)
    {
        UpdateVolumeText(value);
        foreach (var source in audioSourcesArray)
        {
            if (source != null)
            {
                source.volume = volumeSlider.value;
            }
        }
    }
    private void UpdateVolumeText(float value)
    {
        int percent = Mathf.RoundToInt(value * 100f);
        volPercentageText.text = percent.ToString() + "%";
    }
    public void AddCoins()
    {
        GameService.Instance.GameplayService.SetCurrentCoin(GameService.Instance.GameplayService.GetCurrentCoin() + 15);

        StartCoroutine(AddCoinsInText(GameService.Instance.GameplayService.GetCurrentCoin()));
    }
    private IEnumerator AddCoinsInText(int coins)
    {
        while(numbercoinsText < coins)
        {
            numbercoinsText++;
            coinsText.text = numbercoinsText.ToString();

            yield return new WaitForSeconds(0.05f);
        }
    }
    private void OnRestart()
    {
        GameService.Instance.GameplayService.SetCurrentCoin(0);
        numbercoinsText = GameService.Instance.GameplayService.GetCurrentCoin();
        coinsText.text = numbercoinsText.ToString();

        Tint.gameObject.SetActive(false);
    }
}
