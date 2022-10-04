using System;
using Gameplay;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _effectsSource, _musicSource;
    private AudioClip _pegHitByBallClip, _bigPegHitByBallClip;

    private static AudioManager _instance;
    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        // for safety, delete duplicate instance if it exists in the scene
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        PegController.PegHitEvent += PegHitByBall;
        BigPegController.BigPegHitEvent += BigPegHitByBall;
    }

    private void Start()
    {
        _effectsSource = transform.GetChild(0).GetComponent<AudioSource>();
        _musicSource = transform.GetChild(1).GetComponent<AudioSource>();
        _pegHitByBallClip = Resources.Load<AudioClip>("Audio/Effects/PegHitByBall");
        _bigPegHitByBallClip = Resources.Load<AudioClip>("Audio/Effects/Click");
    }

    private void OnDestroy()
    {
        PegController.PegHitEvent -= PegHitByBall;
        BigPegController.BigPegHitEvent -= BigPegHitByBall;
    }

    private void PegHitByBall(int deletethis)
    {
        _effectsSource.PlayOneShot(_pegHitByBallClip);
    }
    
    private void BigPegHitByBall(int deletethis)
    {
        _effectsSource.PlayOneShot(_bigPegHitByBallClip);
    }
}
