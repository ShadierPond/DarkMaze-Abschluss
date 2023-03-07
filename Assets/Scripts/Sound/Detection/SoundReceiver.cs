using UnityEngine;

namespace Sound.Detection
{
    public class SoundReceiver : MonoBehaviour
    {
        public float intensity;
        public Vector3 lastSoundPosition;
        public void ReceiveSound(SoundEmitter soundEmitter)
        {
            var distance = Vector3.Distance(transform.position, soundEmitter.transform.position);
            var soundRadius = soundEmitter.soundRadius;
            var soundIntensity = soundEmitter.soundIntensity;
            var soundDecay = soundEmitter.soundDecay;
            var soundDecayDelay = soundEmitter.soundDecayDelay;
            var soundDecayMultiplier = Mathf.Pow(soundDecay, distance / soundDecayDelay);
            intensity = soundIntensity * (1 - distance / soundRadius) * soundDecayMultiplier;
            if (intensity < 0)
                intensity = 0;
        }
        private void Awake()
            => SoundManager.Instance.RegisterSoundReceiver(this);
        private void OnDestroy()
            => SoundManager.Instance.UnregisterSoundReceiver(this);
    }
}