using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace Management.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        [SerializeField] private AssetReference[] _footstepSounds;
        [SerializeField] private AssetReference[] _attackSounds;
        
        /// <summary>
        /// This method is called when the script instance is being loaded. It gets the AudioSource component attached to the game object and assigns it to a private field.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has an AudioSource component and a SoundManager script attached to it.
        /// </remarks>
        private void Awake()
            // Use the expression-bodied method syntax to get the AudioSource component
            => _audioSource = GetComponent<AudioSource>();

        /// <summary>
        /// This method plays a random footstep sound from an array of AudioClip objects.
        /// </summary>
        public void PlayFootstepSound()
            // Use the expression-bodied method syntax to call the PlaySounds method with the footstep sounds array as the argument
            => PlaySounds(_footstepSounds);

        /// <summary>
        /// This method plays a random attack sound from an array of AudioClip objects.
        /// </summary>
        public void PlayAttackSound()
            // Use the expression-bodied method syntax to call the PlaySounds method with the attack sounds array as the argument
            => PlaySounds(_attackSounds);
        

        /// <summary>
        /// This method plays a random sound from a list of AssetReference objects.
        /// </summary>
        /// <param name="sounds">IReadOnlyList<AssetReference> - The list of sounds to choose from.</param>
        private void PlaySounds(IReadOnlyList<AssetReference> sounds)
        {
            // Get a random sound from the list
            var sound = sounds[Random.Range(0, sounds.Count)];
            // Load the sound asset asynchronously and assign a callback when the operation is completed
            sound.LoadAssetAsync<AudioClip>().Completed += operation =>
            {
                // Set the audio source clip to the loaded sound
                _audioSource.clip = operation.Result;
                // Play the audio source
                _audioSource.Play();
        
                // Unload the asset when the clip is done playing
                sound.ReleaseAsset();
            };
        }

        /// <summary>
        /// This method plays a specific sound from an AssetReference object.
        /// </summary>
        /// <param name="sound">AssetReference - The sound to play.</param>
        public void PlaySound(AssetReference sound)
        {
            // Load the sound asset asynchronously and assign a callback when the operation is completed
            sound.LoadAssetAsync<AudioClip>().Completed += operation =>
            {
                // Set the audio source clip to the loaded sound
                _audioSource.clip = operation.Result;
                // Play the audio source
                _audioSource.Play();
        
                // Unload the asset when the clip is done playing
                sound.ReleaseAsset();
            };
        }
    }
}