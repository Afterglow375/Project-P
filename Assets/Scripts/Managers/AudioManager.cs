using Gameplay.BallArena;
using System;
using Gameplay.Balls;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class AudioManager : PersistentSingleton<AudioManager>
    {
        private AudioSource _effectsSource, _musicSource;
        private AudioClip _pegHitByBallClip, _forceComponentHitByBallClip, _attackHitClip, _diamondClip, _explosionClip;

        private void Start()
        {
            APComponent.HitEvent += ComponentHitByBall;
            CombatManager.TargetHitEvent += TargetHit;
            Ball.BallExplosionEvent += BallExplosionEvent;
            
            _effectsSource = transform.GetChild(0).GetComponent<AudioSource>();
            _musicSource = transform.GetChild(1).GetComponent<AudioSource>();
            _pegHitByBallClip = Resources.Load<AudioClip>("Audio/Effects/PegHitByBall");
            _forceComponentHitByBallClip = Resources.Load<AudioClip>("Audio/Effects/Click");
            _attackHitClip = Resources.Load<AudioClip>("Audio/Effects/quake_hitsound");
            _diamondClip = Resources.Load<AudioClip>("Audio/Effects/peggle_deluxe_peg_hit");
            _explosionClip = Resources.Load<AudioClip>("Audio/Effects/geometry_dash_explosion");
        }

        private void OnDestroy()
        {
            APComponent.HitEvent -= ComponentHitByBall;
            CombatManager.TargetHitEvent -= TargetHit;
            Ball.BallExplosionEvent -= BallExplosionEvent;
        }
    
        private void ComponentHitByBall(int damage, string componentType)
        {
            switch (componentType)
            {
                case nameof(PegController):
                    _effectsSource.PlayOneShot(_pegHitByBallClip);
                    break;
                case nameof(ForceComponentController):
                    _effectsSource.PlayOneShot(_forceComponentHitByBallClip);
                    break;
                case nameof(DiamondController):
                    _effectsSource.PlayOneShot(_diamondClip, .4f);
                    break;
            }
        }

        private void TargetHit()
        {
            _effectsSource.PlayOneShot(_attackHitClip, 0.08f);
        }

        private void BallExplosionEvent()
        {
            _effectsSource.PlayOneShot(_explosionClip, 0.7f);
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
}
