using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    int numbercoinsText;

    [SerializeField] private Button ReplayButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button XButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private Button ReplayWin;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volPercentageText;
    [SerializeField] private Image Tint;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private Image WinTint;
    [SerializeField] private ParticleSystem confetti;
    [SerializeField] private TextMeshProUGUI numberoftruckstext;
    [SerializeField] private Toggle vibrationToggle;
    [SerializeField] private AudioSource LevelCompleteAudio;
    public int numberoftrucks { get; private set; }

    private AudioSource[] audioSourcesArray;
    private void Start()
    {

        ReplayButton.onClick.AddListener(OnClickReplayButton);
        ReplayWin.onClick.AddListener(OnClickReplayButton);
        SettingsButton.onClick.AddListener(OnClickSettingsButton);
        QuitButton.onClick.AddListener(OnQuitButton);
        XButton.onClick.AddListener(OnXButtonClicked);
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);

        numbercoinsText = 0;
        coinsText.text = numbercoinsText.ToString();
        StartCoroutine(GetAllAudioSources());

        GameService.Instance.EventService.OnRestart.AddListener(OnRestart);

        numberoftrucks = 0;
        numberoftruckstext.text = "0/5";
    }

    private IEnumerator GetAllAudioSources()
    {
        yield return new WaitForSeconds(0.2f);

        audioSourcesArray = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach(var source in audioSourcesArray)
        {
            source.volume = volumeSlider.value;
        }
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

        WinTint.gameObject.SetActive(true);

        LevelCompleteAudio.volume = 1f;
        LevelCompleteAudio.Play();
        confetti.Play();
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

        numberoftrucks = 0;
        numberoftruckstext.text = "0/5";


        Tint.gameObject.SetActive(false);
        WinTint.gameObject.SetActive(false);

        StartCoroutine(GetAllAudioSources());
    }
    public void SetNumberofTrucksText(bool toggle)
    {
        if (toggle)
        {
            numberoftrucks++;
            numberoftruckstext.text = numberoftrucks.ToString() + "/5";
        }
        else
        {
            numberoftrucks--;
            numberoftruckstext.text = numberoftrucks.ToString() + "/5";
        }
    }
    public bool VibrationToggle() => vibrationToggle.isOn;
    private void OnQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
