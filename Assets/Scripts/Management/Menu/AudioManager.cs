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
        
        private SaveManager _saveManager;

        
        /// <summary>
        /// Initializes the audio settings UI elements and sets up listeners for changes in volume and mute status.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has four sliders and four toggles for master, music, sfx and ambient audio channels.
        /// </remarks>
        private void Start()
        {
            _saveManager = Management.GameManager.Instance.saveManager;
            
            // Get the saved audio settings data from the SaveManager
            var data = _saveManager.settingsDataClass;
    
            // Set the initial values of the sliders and toggles based on the saved data
            masterSlider.value = data.masterVolume;
            musicSlider.value = data.musicVolume;
            sfxSlider.value = data.sfxVolume;
            ambientSlider.value = data.ambientVolume;
    
            masterToggle.isOn = data.isMasterMuted;
            musicToggle.isOn = data.isMusicMuted;
            sfxToggle.isOn = data.isSfxMuted;
            ambientToggle.isOn = data.isAmbientMuted;

            // Add listeners to the sliders and toggles to invoke methods when their values change
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
            ambientSlider.onValueChanged.AddListener(SetAmbientVolume);
    
            masterToggle.onValueChanged.AddListener(SetMasterMute);
            musicToggle.onValueChanged.AddListener(SetMusicMute);
            sfxToggle.onValueChanged.AddListener(SetSfxMute);
            ambientToggle.onValueChanged.AddListener(SetAmbientMute);
    
            // Invoke the methods once to apply the initial audio settings
            masterSlider.onValueChanged.Invoke(data.masterVolume);
            musicSlider.onValueChanged.Invoke(data.musicVolume);
            sfxSlider.onValueChanged.Invoke(data.sfxVolume);
            ambientSlider.onValueChanged.Invoke(data.ambientVolume);
    
            masterToggle.onValueChanged.Invoke(data.isMasterMuted);
            musicToggle.onValueChanged.Invoke(data.isMusicMuted);
            sfxToggle.onValueChanged.Invoke(data.isSfxMuted);
            ambientToggle.onValueChanged.Invoke(data.isAmbientMuted);
        }
        
        /// <summary>
        /// Saves the current audio settings to the SaveManager and writes them to a file.
        /// </summary>
        private void Save()
        {
            // Get the audio settings data from the SaveManager
            var data = _saveManager.settingsDataClass;
            // Update the data with the current values of the sliders and toggles
            data.ambientVolume = ambientSlider.value;
            data.masterVolume = masterSlider.value;
            data.musicVolume = musicSlider.value;
            data.sfxVolume = sfxSlider.value;
    
            data.isAmbientMuted = ambientToggle.isOn;
            data.isMasterMuted = masterToggle.isOn;
            data.isMusicMuted = musicToggle.isOn;
            data.isSfxMuted = sfxToggle.isOn;
            // Save the updated data to the SaveManager and write it to a file
            _saveManager.SaveSettings();
        }
        
// ------ Master ------
        /// <summary>
        /// Sets the master volume of the audio mixer based on the value of the master slider.
        /// </summary>
        /// <param name="value">float - The value of the master slider.</param>
        private void SetMasterVolume(float value)
        {
            // If the master toggle is not on, set the master volume of the audio mixer using a logarithmic scale
            if (!masterToggle.isOn)
                audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
            // Save the current audio settings
            Save();
        }

        /// <summary>
        /// Sets the master mute status of the audio mixer based on the value of the master toggle.
        /// </summary>
        /// <param name="isOn">bool - The value of the master toggle.</param>
        private void SetMasterMute(bool isOn)
        {
            // If the master toggle is on, set the master volume of the audio mixer to -80 dB (minimum)
            if (isOn)
                audioMixer.SetFloat("MasterVolume", -80);
            // Else, set the master volume of the audio mixer based on the value of the master slider using a logarithmic scale
            else
                audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterSlider.value) * 20);
            // Save the current audio settings
            Save();
        }
        
// ------ Music ------
        /// <summary>
        /// Sets the music volume of the audio mixer based on the value of the music slider.
        /// </summary>
        /// <param name="value">float - The value of the music slider.</param>
        private void SetMusicVolume(float value)
        {
            // If the music toggle is not on, set the music volume of the audio mixer using a logarithmic scale
            if (!musicToggle.isOn)
                audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
            // Save the current audio settings
            Save();
        }
        
        /// <summary>
        /// Sets the music mute status of the audio mixer based on the value of the music toggle.
        /// </summary>
        /// <param name="isOn">bool - The value of the music toggle.</param>
        private void SetMusicMute(bool isOn)
        {
            // If the music toggle is on, set the music volume of the audio mixer to -80 dB (minimum)
            if (isOn)
                audioMixer.SetFloat("MusicVolume", -80);
            // Else, set the music volume of the audio mixer based on the value of the music slider using a logarithmic scale
            else
                audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 20);
            // Save the current audio settings
            Save();
        }

// ------ SFX ------
        /// <summary>
        /// Sets the sfx volume of the audio mixer based on the value of the sfx slider.
        /// </summary>
        /// <param name="value">float - The value of the sfx slider.</param>
        private void SetSfxVolume(float value)
        {
            // If the sfx toggle is not on, set the sfx volume of the audio mixer using a logarithmic scale
            if (!sfxToggle.isOn)
                audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
            // Save the current audio settings
            Save();
        }
        
        /// <summary>
        /// Sets the sfx mute status of the audio mixer based on the value of the sfx toggle.
        /// </summary>
        /// <param name="isOn">bool - The value of the sfx toggle.</param>
        private void SetSfxMute(bool isOn)
        {
            // If the sfx toggle is on, set the sfx volume of the audio mixer to -80 dB (minimum)
            if (isOn)
                audioMixer.SetFloat("SFXVolume", -80);
            // Else, set the sfx volume of the audio mixer based on the value of the sfx slider using a logarithmic scale
            else
                audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxSlider.value) * 20);
            // Save the current audio settings
            Save();
        }

// ------ Ambient ------
        /// <summary>
        /// Sets the ambient volume of the audio mixer based on the value of the ambient slider.
        /// </summary>
        /// <param name="value">float - The value of the ambient slider.</param>
        private void SetAmbientVolume(float value)
        {
            // If the ambient toggle is not on, set the ambient volume of the audio mixer using a logarithmic scale
            if (!ambientToggle.isOn)
                audioMixer.SetFloat("AmbientVolume", Mathf.Log10(value) * 20);
            // Save the current audio settings
            Save();
        }
        
        /// <summary>
        /// Sets the ambient mute status of the audio mixer based on the value of the ambient toggle.
        /// </summary>
        /// <param name="isOn">bool - The value of the ambient toggle.</param>
        private void SetAmbientMute(bool isOn)
        {
            // If the ambient toggle is on, set the ambient volume of the audio mixer to -80 dB (minimum)
            if (isOn)
                audioMixer.SetFloat("AmbientVolume", -80);
            // Else, set the ambient volume of the audio mixer based on the value of the ambient slider using a logarithmic scale
            else
                audioMixer.SetFloat("AmbientVolume", Mathf.Log10(ambientSlider.value) * 20);
            // Save the current audio settings
            Save();
        }
    }
}