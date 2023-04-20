using System.Collections.Generic;
using UnityEngine;

namespace Management.Sound.Detection
{
    public class SoundDetectionManager : MonoBehaviour
    {
        public static SoundDetectionManager Instance { get; private set; }
        public List<SoundReceiver> soundReceivers = new();
        
        /// <summary>
        /// This method registers a SoundReceiver object to the list of sound receivers.
        /// </summary>
        /// <param name="soundReceiver">SoundReceiver - The sound receiver to register.</param>
        public void RegisterSoundReceiver(SoundReceiver soundReceiver)
            // Use the expression-bodied method syntax to add the sound receiver to the list
            => soundReceivers.Add(soundReceiver);

        /// <summary>
        /// This method unregisters a SoundReceiver object from the list of sound receivers.
        /// </summary>
        /// <param name="soundReceiver">SoundReceiver - The sound receiver to unregister.</param>
        public void UnregisterSoundReceiver(SoundReceiver soundReceiver)
            // Use the expression-bodied method syntax to remove the sound receiver from the list
            => soundReceivers.Remove(soundReceiver);

        /// <summary>
        /// This method emits a sound from a SoundEmitter object to all the registered sound receivers.
        /// </summary>
        /// <param name="soundEmitter">SoundEmitter - The sound emitter that emits the sound.</param>
        public void EmitSound(SoundEmitter soundEmitter)
        {
            // Loop through each sound receiver in the list
            foreach (var receiver in soundReceivers)
            {
                // Set the last sound position of the receiver to the position of the sound emitter
                receiver.lastSoundPosition = soundEmitter.GetPosition();
                // Call the ReceiveSound method of the receiver with the sound emitter as the argument
                receiver.ReceiveSound(soundEmitter);
            }
        }

        /// <summary>
        /// This method is called when the script instance is being loaded. It assigns this script to the singleton instance field.
        /// </summary>
        private void Awake()
            // Use the expression-bodied method syntax to assign this script to the instance field
            => Instance = this;
    }
}