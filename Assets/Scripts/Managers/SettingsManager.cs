using UI;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private Slider _sfxVolumeSlider;
    [SerializeField]
    private Slider _musicVolumeSlider;
    [SerializeField]
    private GameObject _pauseMenuParent;
    private PauseMenu _pauseMenu;

    private GameObject _settingsMenuUI;
    private Image _settingsMenuBackground;

    // Start is called before the first frame update
    void Start()
    {
        _settingsMenuUI = transform.GetChild(0).gameObject;
        _settingsMenuUI.SetActive(false);
        _sfxVolumeSlider.value = AudioManager.Instance.GetEffectsVolume();
        _musicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
        _sfxVolumeSlider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeEffectsVolume(val));
        _musicVolumeSlider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeMusicVolume(val));

        if (_pauseMenuParent != null)
        {
            _pauseMenu = _pauseMenuParent.GetComponent<PauseMenu>();
        }

        if (GameManager.Instance.state == GameState.MainMenu) // Set background to not be transparent in main menu
        {
            _settingsMenuBackground = _settingsMenuUI.GetComponent<Image>();
            var newColor = _settingsMenuBackground.color;
            newColor.a = 1f;
            _settingsMenuBackground.color = newColor;
        }
    }

    public void BackButton()
    {
        if (_pauseMenu != null)
        {
            _pauseMenu.TogglePauseMenuUI();
        }
        ToggleSettingsMenuUI();
    }

    public void ToggleSettingsMenuUI()
    {
        _settingsMenuUI.SetActive(!_settingsMenuUI.activeSelf);
    }

    public void CloseSettingsMenuUI()
    {
        _settingsMenuUI.SetActive(false);
    }
}
