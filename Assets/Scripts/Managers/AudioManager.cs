using Gameplay;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _effectsSource, _musicSource;
    private static AudioManager _instance;
    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        // for safety, delete duplicate instance if it exists in the scene
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        PegController.PegHitEvent += PegHitByBall;
    }

    private void OnDestroy()
    {
        PegController.PegHitEvent -= PegHitByBall;
    }

    private void PegHitByBall(int deletethis)
    {
        PlaySoundByPath("Assets/Sounds/Effects/PegHitByBall.ogg");
    }

    private void PlaySoundByPath(string audioClipFilePath)
    {
        var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioClipFilePath);
        Debug.Log($"Playing clip: {clip.name}");
        _effectsSource.PlayOneShot(clip);
    }
}
