using UnityEngine;

namespace Management.Sound.Detection
{
    public class SoundReceiver : MonoBehaviour
    {
        public float intensity;
        public Vector3 lastSoundPosition;
        
        /// <summary>
        /// This method receives a sound from a SoundEmitter object and calculates its intensity based on the distance, sound radius, sound intensity, sound decay, and sound decay delay.
        /// </summary>
        /// <param name="soundEmitter">SoundEmitter - The sound emitter that emits the sound.</param>
        public void ReceiveSound(SoundEmitter soundEmitter)
        {
            // Calculate the distance between this game object and the sound emitter
            var distance = Vector3.Distance(transform.position, soundEmitter.transform.position);
            // Get the sound radius from the sound emitter
            var soundRadius = soundEmitter.soundRadius;
            // Get the sound intensity from the sound emitter
            var soundIntensity = soundEmitter.soundIntensity;
            // Get the sound decay from the sound emitter
            var soundDecay = soundEmitter.soundDecay;
            // Get the sound decay delay from the sound emitter
            var soundDecayDelay = soundEmitter.soundDecayDelay;
            // Calculate the sound decay multiplier based on the distance and the sound decay delay
            var soundDecayMultiplier = Mathf.Pow(soundDecay, distance / soundDecayDelay);
            // Calculate the intensity of the received sound based on the sound intensity, distance, sound radius, and sound decay multiplier
            intensity = soundIntensity * (1 - distance / soundRadius) * soundDecayMultiplier;
            // If the intensity is negative, set it to zero
            if (intensity < 0)
                intensity = 0;
        }

        /// <summary>
        /// This method is called before the first frame update. It registers this game object as a sound receiver to the SoundDetectionManager singleton instance.
        /// </summary>
        private void Start()
            // Use the expression-bodied method syntax to call the RegisterSoundReceiver method of the SoundDetectionManager instance with this script as the argument
            => SoundDetectionManager.Instance.RegisterSoundReceiver(this);

        /// <summary>
        /// This method is called when the game object is destroyed. It unregisters this game object as a sound receiver from the SoundDetectionManager singleton instance.
        /// </summary>
        private void OnDestroy()
            // Use the expression-bodied method syntax to call the UnregisterSoundReceiver method of the SoundDetectionManager instance with this script as the argument
            => SoundDetectionManager.Instance.UnregisterSoundReceiver(this);
    }
}