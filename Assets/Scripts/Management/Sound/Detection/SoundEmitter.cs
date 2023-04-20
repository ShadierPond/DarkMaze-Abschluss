using UnityEngine;

namespace Management.Sound.Detection
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        public float soundRadius = 10f;
        public float soundIntensity = 1f;
        public float soundDecay = 1f;
        public float soundDecayDelay = 1f;
        public AudioSource audioSource;

        /// <summary>
        /// This method is called when the script instance is being loaded. It gets the AudioSource component attached to the game object and assigns it to a private field.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has an AudioSource component and a SoundEmitter script attached to it.
        /// </remarks>
        private void Awake()
            // Use the expression-bodied method syntax to get the AudioSource component
            => audioSource = GetComponent<AudioSource>();

        /// <summary>
        /// This method is called once per frame. It calls the EmitSound method to emit a sound if the audio source is playing.
        /// </summary>
        private void Update() 
            // Use the expression-bodied method syntax to call the EmitSound method
            => EmitSound();

        /// <summary>
        /// This method emits a sound from this game object to the SoundDetectionManager singleton instance if the audio source is playing and the SoundDetectionManager instance is not null.
        /// </summary>
        private void EmitSound()
        {
            // Check if the audio source is playing and the SoundDetectionManager instance is not null
            if(audioSource.isPlaying && SoundDetectionManager.Instance != null)
                // Call the EmitSound method of the SoundDetectionManager instance with this script as the argument
                SoundDetectionManager.Instance.EmitSound(this);
        }

        /// <summary>
        /// This method returns the position of this game object as a Vector3 value.
        /// </summary>
        /// <returns>Vector3 - The position of this game object.</returns>
        public Vector3 GetPosition()
            // Use the expression-bodied method syntax to return the transform position
            => transform.position;

        /// <summary>
        /// This method draws a wire sphere gizmo around this game object with a radius equal to the sound radius field in blue color.
        /// </summary>
        private  void OnDrawGizmos()
        {
            // Set the gizmo color to blue
            Gizmos.color = Color.blue;
            // Draw a wire sphere gizmo at the transform position with the sound radius as the radius
            Gizmos.DrawWireSphere(transform.position, soundRadius);
        }
    }
}