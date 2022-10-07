using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        _pauseMenu = _pauseMenuParent.GetComponent<PauseMenu>();
        _settingsMenuUI = transform.GetChild(0).gameObject;
        _settingsMenuUI.SetActive(false);
        _sfxVolumeSlider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeEffectsVolume(val));
        _musicVolumeSlider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeMusicVolume(val));
    }

    public void BackButton()
    {
        _pauseMenu.TogglePauseMenuUI();
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
