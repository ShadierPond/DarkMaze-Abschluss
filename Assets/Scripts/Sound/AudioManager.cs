using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AddressableAssets;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        [SerializeField] private AssetReference[] _footstepSounds;
        [SerializeField] private AssetReference[] _attackSounds;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        public void PlayFootstepSound()
        {
            PlaySounds(_footstepSounds);
        }
        
        public void PlayAttackSound()
        {
            PlaySounds(_attackSounds);
        }

        public void PlaySounds(AssetReference[] sounds)
        {
            var sound = sounds[Random.Range(0, sounds.Length)];
            sound.LoadAssetAsync<AudioClip>().Completed += operation =>
            {
                _audioSource.clip = operation.Result;
                _audioSource.Play();
                
                // Unload the asset when the clip is done playing
                sound.ReleaseAsset();
            };
        }
        
        public void PlaySound(AssetReference sound)
        {
            sound.LoadAssetAsync<AudioClip>().Completed += operation =>
            {
                _audioSource.clip = operation.Result;
                _audioSource.Play();
                
                // Unload the asset when the clip is done playing
                sound.ReleaseAsset();
            };
        }
    }
}