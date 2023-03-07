using System.Collections.Generic;
using Sound.Detection;
using UnityEngine;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
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
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }
    }
}