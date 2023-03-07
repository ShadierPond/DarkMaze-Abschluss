using UnityEngine;

namespace Sound.Detection
{
    public class SoundReciever : MonoBehaviour
    {
        public float intensity;
        public Vector3 lastSoundPosition;
        public void ReceiveSound(SoundEmitter soundEmitter)
        {
            var distance = Vector3.Distance(transform.position, soundEmitter.transform.position);
            var soundIntensity = soundEmitter.soundIntensity;
            var soundDecay = soundEmitter.soundDecay;
            var soundDecayDelay = soundEmitter.soundDecayDelay;
            intensity= soundIntensity / (distance * soundDecay + soundDecayDelay);
        }
    }
}