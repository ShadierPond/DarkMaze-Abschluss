using Management.SaveSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Management.Menu
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Sliders")]
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider ambientSlider;
        [Header("Toggles")]
        [SerializeField] private Toggle masterToggle;
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Toggle sfxToggle;
        [SerializeField] private Toggle ambientToggle;
        [Header("Audio Mixer")]
        public AudioMixer audioMixer;

        
        private void Start()
        {
            GameManager.Instance.saveManager.LoadSettings();
            var data = SaveManager.Instance.settingsDataClass;
            
            masterSlider.value = data.masterVolume;
            musicSlider.value = data.musicVolume;
            sfxSlider.value = data.sfxVolume;
            ambientSlider.value = data.ambientVolume;
            
            masterToggle.isOn = data.isMasterMuted;
            musicToggle.isOn = data.isMusicMuted;
            sfxToggle.isOn = data.isSfxMuted;
            ambientToggle.isOn = data.isAmbientMuted;

            masterSlider.onValueChanged.AddListener(SetMasterVolume);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
            ambientSlider.onValueChanged.AddListener(SetAmbientVolume);
            
            masterToggle.onValueChanged.AddListener(SetMasterMute);
            musicToggle.onValueChanged.AddListener(SetMusicMute);
            sfxToggle.onValueChanged.AddListener(SetSfxMute);
            ambientToggle.onValueChanged.AddListener(SetAmbientMute);
            
            masterSlider.onValueChanged.Invoke(data.masterVolume);
            musicSlider.onValueChanged.Invoke(data.musicVolume);
            sfxSlider.onValueChanged.Invoke(data.sfxVolume);
            ambientSlider.onValueChanged.Invoke(data.ambientVolume);
            
            masterToggle.onValueChanged.Invoke(data.isMasterMuted);
            musicToggle.onValueChanged.Invoke(data.isMusicMuted);
            sfxToggle.onValueChanged.Invoke(data.isSfxMuted);
            ambientToggle.onValueChanged.Invoke(data.isAmbientMuted);
        }
        
        private void Save()
        {
            var data = SaveManager.Instance.settingsDataClass;
            data.ambientVolume = ambientSlider.value;
            data.masterVolume = masterSlider.value;
            data.musicVolume = musicSlider.value;
            data.sfxVolume = sfxSlider.value;
            
            data.isAmbientMuted = ambientToggle.isOn;
            data.isMasterMuted = masterToggle.isOn;
            data.isMusicMuted = musicToggle.isOn;
            data.isSfxMuted = sfxToggle.isOn;
            SaveManager.Instance.SaveSettings();
        }

        // Master
        private void SetMasterVolume(float value)
        {
            if (!masterToggle.isOn)
                audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
            Save();
        }

        private void SetMasterMute(bool isOn)
        {
            if (isOn)
                audioMixer.SetFloat("MasterVolume", -80);
            else
                audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterSlider.value) * 20);
            Save();
        }

        // Music
        private void SetMusicVolume(float value)
        {
            if (!musicToggle.isOn)
                audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
            Save();
        }
        
        private void SetMusicMute(bool isOn)
        {
            if (isOn)
                audioMixer.SetFloat("MusicVolume", -80);
            else
                audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 20);
            Save();
        }

        // SFX
        private void SetSfxVolume(float value)
        {
            if (!sfxToggle.isOn)
                audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
            Save();
        }
        
        private void SetSfxMute(bool isOn)
        {
            if (isOn)
                audioMixer.SetFloat("SFXVolume", -80);
            else
                audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxSlider.value) * 20);
            Save();
        }

        // Ambient
        private void SetAmbientVolume(float value)
        {
            if (!ambientToggle.isOn)
                audioMixer.SetFloat("AmbientVolume", Mathf.Log10(value) * 20);
            Save();
        }
        
        private void SetAmbientMute(bool isOn)
        {
            if (isOn)
                audioMixer.SetFloat("AmbientVolume", -80);
            else
                audioMixer.SetFloat("AmbientVolume", Mathf.Log10(ambientSlider.value) * 20);
            Save();
        }
    }
}