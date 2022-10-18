using Gameplay.BallArena;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class AudioManager : PersistentSingleton<AudioManager>
    {
        private AudioSource _effectsSource, _musicSource;
        private AudioClip _pegHitByBallClip, _forceComponentHitByBallClip, _attackHitClip;

        private void Start()
        {
            PegController.PegHitEvent += PegHitByBall;
            ForceComponentController.ComponentHitEvent += ComponentHitByBall;
            CombatManager.TargetHitEvent += TargetHit;
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

        private void PegHitByBall(int damage)
        {
            _effectsSource.PlayOneShot(_pegHitByBallClip);
        }
    
        private void ComponentHitByBall(int damage)
        {
            _effectsSource.PlayOneShot(_forceComponentHitByBallClip);
        }

        private void TargetHit()
        {
            _effectsSource.PlayOneShot(_attackHitClip, 0.3f);
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
