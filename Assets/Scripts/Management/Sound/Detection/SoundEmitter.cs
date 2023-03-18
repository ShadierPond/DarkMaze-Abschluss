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
        //public int maxObjectsToEmitSoundTo = 10;

        private void Awake()
            => audioSource = GetComponent<AudioSource>();

        private void Update() 
            => EmitSound();

        private void EmitSound()
        {
            if(audioSource.isPlaying)
                SoundDetectionManager.Instance.EmitSound(this);
            /*
            var results = new Collider[maxObjectsToEmitSoundTo];
            Physics.OverlapSphereNonAlloc(transform.position, soundRadius, results);
            foreach (var result in results)
            {
                if (result == null)
                    continue;
                var soundReciever = result.GetComponent<SoundReciever>();
                if (soundReciever != null)
                {
                    soundReciever.ReceiveSound(this);
                    soundReciever.lastSoundPosition = GetRandomPointInRadius();
                }
            }
            */
        }
        
        public Vector3 GetPosition()
            => transform.position;
        private  void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, soundRadius);
        }
    }
}