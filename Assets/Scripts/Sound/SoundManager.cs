using System;
using System.Collections.Generic;
using Sound.Detection;
using UnityEngine;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        public List<SoundEmitter> soundEmitters = new();

        public void RegisterSoundEmitter(SoundEmitter soundEmitter)
        {
            soundEmitters.Add(soundEmitter);
        }

        public void UnregisterSoundEmitter(SoundEmitter soundEmitter)
        {
            soundEmitters.Remove(soundEmitter);
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