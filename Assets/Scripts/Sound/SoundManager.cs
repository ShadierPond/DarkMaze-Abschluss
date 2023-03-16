using System.Collections.Generic;
using Sound.Detection;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        public AudioMixer audioMixer;
        public List<SoundReceiver> soundReceivers = new();
        
        private bool _isMasterMuted;
        private float _cachedMasterVolume;
        private bool _isMusicMuted;
        private float _cachedMusicVolume;
        private bool _isSfxMuted;
        private float _cachedSfxVolume;
        private bool _isAmbientMuted;
        private float _cachedAmbientVolume;
        
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

        public void SetMasterVolume(Slider slider)
        {
            if (_isMasterMuted)
                return;
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(slider.value) * 20);
            _cachedMasterVolume = slider.value;
        }

        public void MuteMasterVolume(Toggle toggle)
        {
            if (toggle.isOn)
                audioMixer.SetFloat("MasterVolume", -80);
            else
                audioMixer.SetFloat("MasterVolume", Mathf.Log10(_cachedMasterVolume) * 20);
            _isMasterMuted = toggle.isOn;
        }

        public void SetMusicVolume(Slider slider)
        {
            if (!_isMusicMuted)
                audioMixer.SetFloat("MusicVolume", Mathf.Log10(slider.value) * 20);
            _cachedMusicVolume = slider.value;
        }
        
        public void MuteMusicVolume(Toggle toggle)
        {
            if (toggle.isOn)
                audioMixer.SetFloat("MusicVolume", -80);
            else
                audioMixer.SetFloat("MusicVolume", Mathf.Log10(_cachedMusicVolume) * 20);
            _isMusicMuted = toggle.isOn;
        }

        public void SetSfxVolume(Slider slider)
        {
            if (!_isSfxMuted)
                audioMixer.SetFloat("SFXVolume", Mathf.Log10(slider.value) * 20);
            _cachedSfxVolume = slider.value;
        }
        
        public void MuteSfxVolume(Toggle toggle)
        {
            if (toggle.isOn)
                audioMixer.SetFloat("SFXVolume", -80);
            else
                audioMixer.SetFloat("SFXVolume", Mathf.Log10(_cachedSfxVolume) * 20);
            _isSfxMuted = toggle.isOn;
        }

        public void SetAmbientVolume(Slider slider)
        {
            if (!_isAmbientMuted)
                audioMixer.SetFloat("AmbientVolume", Mathf.Log10(slider.value) * 20);
            _cachedAmbientVolume = slider.value;
        }
        
        public void MuteAmbientVolume(Toggle toggle)
        {
            if (toggle.isOn)
                audioMixer.SetFloat("AmbientVolume", -80);
            else
                audioMixer.SetFloat("AmbientVolume", Mathf.Log10(_cachedAmbientVolume) * 20);
            _isAmbientMuted = toggle.isOn;
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