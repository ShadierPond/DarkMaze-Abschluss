using System;
using UnityEngine;

namespace Sound.Detection
{
    public class SoundEmitter : MonoBehaviour
    {
        public float soundRadius = 10f;
        public float soundIntensity = 1f;
        public float soundDecay = 1f;
        public float soundDecayDelay = 1f;
        public AudioSource audioSource;
        
        public int maxObjectsToEmitSoundTo = 10;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if(audioSource.isPlaying)
                EmitSound();
        }


        private void Start()
        {
            //SoundManager.Instance.RegisterSoundEmitter(this);
        }
        
        public void EmitSound()
        {
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
        }
        
        private Vector3 GetRandomPointInRadius()
        {
            var randomPoint = UnityEngine.Random.insideUnitSphere * soundRadius;
            randomPoint += transform.position;
            return randomPoint;
        }





        private  void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, soundRadius);
        }
    }
}