using System;
using Gameplay;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _effectsSource, _musicSource;
    private static AudioManager _instance;
    public AudioClip _pegHitByBallClip;
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
        Debug.Log($"Playing clip: {_pegHitByBallClip.name}");
        _effectsSource.PlayOneShot(_pegHitByBallClip);
    }
}
