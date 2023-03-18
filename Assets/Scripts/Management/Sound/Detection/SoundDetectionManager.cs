using System.Collections.Generic;
using UnityEngine;

namespace Management.Sound.Detection
{
    public class SoundDetectionManager : MonoBehaviour
    {
        public static SoundDetectionManager Instance { get; private set; }
        
        public List<SoundReceiver> soundReceivers = new();
        
        public void RegisterSoundReceiver(SoundReceiver soundReceiver)
            => soundReceivers.Add(soundReceiver);

        public void UnregisterSoundReceiver(SoundReceiver soundReceiver)
            => soundReceivers.Remove(soundReceiver);

        public void EmitSound(SoundEmitter soundEmitter)
        {
            foreach (var receiver in soundReceivers)
            {
                receiver.lastSoundPosition = soundEmitter.GetPosition();
                receiver.ReceiveSound(soundEmitter);
            }
        }
        
        private void Awake()
        {
            Instance = this;
        }
    }
}