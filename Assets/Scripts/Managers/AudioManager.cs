using System;
using Gameplay;
using Managers;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _effectsSource, _musicSource;
    private AudioClip _pegHitByBallClip, _forceComponentHitByBallClip, _attackHitClip;

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
        ForceComponentController.ComponentHitEvent += ComponentHitByBall;
        CombatManager.TargetHitEvent += TargetHit;
    }

    private void Start()
    {
        _effectsSource = transform.GetChild(0).GetComponent<AudioSource>();
        _musicSource = transform.GetChild(1).GetComponent<AudioSource>();
        _pegHitByBallClip = Resources.Load<AudioClip>("Audio/Effects/PegHitByBall");
        _forceComponentHitByBallClip = Resources.Load<AudioClip>("Audio/Effects/Click");
        _attackHitClip = Resources.Load<AudioClip>("Audio/Effects/AttackHit");
    }

    private void OnDestroy()
    {
        PegController.PegHitEvent -= PegHitByBall;
        ForceComponentController.ComponentHitEvent -= ComponentHitByBall;
        CombatManager.TargetHitEvent -= TargetHit;
    }

    private void PegHitByBall(int deletethis)
    {
        _effectsSource.PlayOneShot(_pegHitByBallClip);
    }
    
    private void ComponentHitByBall(int deletethis)
    {
        _effectsSource.PlayOneShot(_forceComponentHitByBallClip);
    }

    private void TargetHit()
    {
            _effectsSource.PlayOneShot(_attackHitClip);
    }

    public void ChangeEffectsVolume(float value)
    {
        _effectsSource.volume = value;
    }

    public void ChangeMusicVolume(float value)
    {
        _musicSource.volume = value;
    }

    public float GetEffectsVolume()
    {
        return _effectsSource.volume;
    }

    public float GetMusicVolume()
    {
        return _musicSource.volume;
    }
}
