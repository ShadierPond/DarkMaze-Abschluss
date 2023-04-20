using Management.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using ShadowQuality = UnityEngine.ShadowQuality;

namespace Management.Menu
{
    public class VideoManager : MonoBehaviour
    {
        private VolumeProfile _postProcessVolume;
        private AmbientOcclusion _ambientOcclusion;
        private MotionBlur _motionBlur;
        private ScreenSpaceRefraction _screenSpaceReflections;
        private FilmGrain _filmGrain;
        private SaveManager _saveManager;
        
        // Display
        private Resolution[] _resolutions;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        private readonly FullScreenMode[] _fullScreenModes = {FullScreenMode.ExclusiveFullScreen, FullScreenMode.FullScreenWindow, FullScreenMode.MaximizedWindow, FullScreenMode.Windowed};
        [SerializeField] private TMP_Dropdown fullscreenDropdown;
        private readonly int[] _refreshRates = {30, 60, 120, 144, 240, 999};
        [SerializeField] private TMP_Dropdown refreshRateDropdown;
        [SerializeField] private Toggle runInBackgroundToggle;

        // Graphics
        [SerializeField] private TMP_Dropdown vSyncDropdown;
        [SerializeField] private TMP_Dropdown textureQualityDropdown;
        [SerializeField] private Toggle textureStreamingToggle;
        [SerializeField] private TMP_Dropdown shadowQualityDropdown;
        [SerializeField] private Toggle ambientOcclusionToggle;
        [SerializeField] private Toggle motionBlurToggle;
        [SerializeField] private Slider motionBlurSlider;
        [SerializeField] private Toggle filmGrainToggle;
        [SerializeField] private Slider filmGrainSlider;
        
        /// <summary>
        /// Initializes the settings menu with the available and saved options for various graphics and application settings.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has references to the following fields:
        /// - _resolutions: an array of Resolution structs that store the available screen resolutions.
        /// - resolutionDropdown: a TMP_Dropdown component that displays the resolution options.
        /// - _refreshRates: an array of int values that store the available refresh rates.
        /// - refreshRateDropdown: a TMP_Dropdown component that displays the refresh rate options.
        /// - _fullScreenModes: an array of FullScreenMode enums that store the available fullscreen modes.
        /// - fullscreenDropdown: a TMP_Dropdown component that displays the fullscreen mode options.
        /// - runInBackgroundToggle: a Toggle component that controls the run in background option.
        /// - vSyncDropdown: a TMP_Dropdown component that displays the vSync options.
        /// - textureQualityDropdown: a TMP_Dropdown component that displays the texture quality options.
        /// - textureStreamingToggle: a Toggle component that controls the texture streaming option.
        /// - shadowQualityDropdown: a TMP_Dropdown component that displays the shadow quality options.
        /// - _ambientOcclusion: an instance of the AmbientOcclusion class from the UnityEngine.Rendering.PostProcessing namespace that controls the ambient occlusion effect.
        /// - ambientOcclusionToggle: a Toggle component that controls the ambient occlusion option.
        /// - _motionBlur: an instance of the MotionBlur class from the UnityEngine.Rendering.PostProcessing namespace that controls the motion blur effect.
        /// - motionBlurToggle: a Toggle component that controls the motion blur option.
        /// - motionBlurSlider: a Slider component that controls the motion blur intensity.
        /// - _filmGrain: an instance of the FilmGrain class from the UnityEngine.Rendering.PostProcessing namespace that controls the film grain effect.
        /// - filmGrainToggle: a Toggle component that controls the film grain option.
        /// - filmGrainSlider: a Slider component that controls the film grain intensity.
        /// </remarks>
        private void Start()
        {
            _saveManager = Management.GameManager.Instance.saveManager;
            
            // Get the available resolutions and refresh rates
            _resolutions = Screen.resolutions;
            // Remove duplicate resolutions
            resolutionDropdown.ClearOptions();
            // Add the resolutions to the dropdown
            foreach (var resolution in _resolutions)
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString().Split('@')[0]));
            // Refresh the dropdown
            resolutionDropdown.RefreshShownValue();
            
            // Add the refresh rates to the dropdown
            refreshRateDropdown.ClearOptions();
            foreach (var refreshRate in _refreshRates)
                refreshRateDropdown.options.Add(refreshRate == 999
                    ? new TMP_Dropdown.OptionData("Unlimited")
                    : new TMP_Dropdown.OptionData(refreshRate.ToString())
                );
            // Refresh the dropdown
            refreshRateDropdown.RefreshShownValue();
            
            // Add the fullscreen modes to the dropdown
            fullscreenDropdown.ClearOptions();
            foreach (var fullScreenMode in _fullScreenModes)
                fullscreenDropdown.options.Add(new TMP_Dropdown.OptionData(fullScreenMode.ToString()));
            // Refresh the dropdown
            fullscreenDropdown.RefreshShownValue();

            // Get the post processing volume and the settings and the Save data
            var data = _saveManager.settingsDataClass;
            _postProcessVolume = Management.GameManager.Instance.profile;
            _postProcessVolume.TryGet(out _ambientOcclusion);
            _postProcessVolume.TryGet(out _motionBlur);
            _postProcessVolume.TryGet(out _filmGrain);

            // Get the saved settings
            resolutionDropdown.value = data.resolutionIndex;
            refreshRateDropdown.value = data.refreshRateIndex;
            fullscreenDropdown.value = data.fullscreenIndex;
            resolutionDropdown.RefreshShownValue();
            refreshRateDropdown.RefreshShownValue();
            fullscreenDropdown.RefreshShownValue();
            
            // Set the saved settings
            runInBackgroundToggle.isOn = data.runInBackground;
            vSyncDropdown.value = data.vSyncCount;
            textureQualityDropdown.value = data.textureQuality;
            textureStreamingToggle.isOn = data.textureStreaming;
            shadowQualityDropdown.value = data.shadowQuality;
            ambientOcclusionToggle.isOn = data.ambientOcclusion;
            motionBlurToggle.isOn = data.motionBlur;
            motionBlurSlider.value = data.motionBlurShutterAngle;
            filmGrainToggle.isOn = data.filmGrain;
            filmGrainSlider.value = data.filmGrainIntensity;
            
            // Apply the saved settings
            SetScreen(resolutionDropdown.value, fullscreenDropdown.value, refreshRateDropdown.value);
            SetRunInBackground(runInBackgroundToggle.isOn);
            SetVSync(vSyncDropdown.value);
            SetTextureQuality(textureQualityDropdown.value);
            SetTextureStreaming(textureStreamingToggle.isOn);
            SetShadowQuality(shadowQualityDropdown.value);
            SetAmbientOcclusion(ambientOcclusionToggle.isOn);
            SetMotionBlur(motionBlurToggle.isOn);
            SetMotionBlurSlider(motionBlurSlider.value);
            SetFilmGrain(filmGrainToggle.isOn);
            SetFilmGrainSlider(filmGrainSlider.value);

            // Set the values of the dropdowns to the current settings
            resolutionDropdown.onValueChanged.AddListener(delegate { SetScreen(resolutionDropdown.value, fullscreenDropdown.value, refreshRateDropdown.value); });
            fullscreenDropdown.onValueChanged.AddListener(delegate { SetScreen(resolutionDropdown.value, fullscreenDropdown.value,  refreshRateDropdown.value); });
            refreshRateDropdown.onValueChanged.AddListener(delegate { SetScreen(resolutionDropdown.value, fullscreenDropdown.value,  refreshRateDropdown.value); });
            runInBackgroundToggle.onValueChanged.AddListener(delegate { SetRunInBackground(runInBackgroundToggle.isOn); });
            vSyncDropdown.onValueChanged.AddListener(delegate { SetVSync(vSyncDropdown.value); });
            textureQualityDropdown.onValueChanged.AddListener(delegate { SetTextureQuality(textureQualityDropdown.value); });
            textureStreamingToggle.onValueChanged.AddListener(delegate { SetTextureStreaming(textureStreamingToggle.isOn); });
            shadowQualityDropdown.onValueChanged.AddListener(delegate { SetShadowQuality(shadowQualityDropdown.value); });
            ambientOcclusionToggle.onValueChanged.AddListener(delegate { SetAmbientOcclusion(ambientOcclusionToggle.isOn); });
            motionBlurToggle.onValueChanged.AddListener(delegate { SetMotionBlur(motionBlurToggle.isOn); });
            motionBlurSlider.onValueChanged.AddListener(delegate { SetMotionBlurSlider(motionBlurSlider.value); });
            filmGrainToggle.onValueChanged.AddListener(delegate { SetFilmGrain(filmGrainToggle.isOn); });
            filmGrainSlider.onValueChanged.AddListener(delegate { SetFilmGrainSlider(filmGrainSlider.value); });
        }

        /// <summary>
        /// Saves the current graphics settings to the SaveManager and writes them to a file.
        /// </summary>
        private void Save()
        {
            // Get the graphics settings data from the SaveManager
            var data = _saveManager.settingsDataClass;
            // Update the data with the current values of the dropdowns and toggles
            data.resolutionIndex = resolutionDropdown.value;
            data.fullscreenIndex = fullscreenDropdown.value;
            data.refreshRateIndex = refreshRateDropdown.value;
            data.runInBackground = runInBackgroundToggle.isOn;

            data.vSyncCount = vSyncDropdown.value;
            data.textureQuality = textureQualityDropdown.value;
            data.textureStreaming = textureStreamingToggle.isOn;
            data.shadowQuality = shadowQualityDropdown.value;
            data.ambientOcclusion = ambientOcclusionToggle.isOn;
            data.motionBlur = motionBlurToggle.isOn;
            data.motionBlurShutterAngle = motionBlurSlider.value;
            data.filmGrain = filmGrainToggle.isOn;
            data.filmGrainIntensity = filmGrainSlider.value;
            // Save the updated data to the SaveManager and write it to a file
            _saveManager.SaveSettings();
        } 

// ------------------ Display ------------------
        /// <summary>
        /// Sets the screen resolution, fullscreen mode and refresh rate based on the given indices.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a reference to the _resolutions, _fullScreenModes and _refreshRates arrays, which store the available options for each setting.
        /// </remarks>
        /// <param name="resolutionIndex">int - The index of the resolution to set.</param>
        /// <param name="fullscreenIndex">int - The index of the fullscreen mode to set.</param>
        /// <param name="refreshRateIndex">int - The index of the refresh rate to set.</param>
        private void SetScreen(int resolutionIndex, int fullscreenIndex, int refreshRateIndex)
        {
            // Set the screen resolution, fullscreen mode and refresh rate using the Screen class
            Screen.SetResolution(_resolutions[resolutionIndex].width, _resolutions[resolutionIndex].height, _fullScreenModes[fullscreenIndex], _refreshRates[refreshRateIndex]);
            // Save the settings using a custom method
            Save();
        }

        /// <summary>
        /// Sets the run in background option for the application based on the given boolean value.
        /// </summary>
        /// <param name="runInBackground">bool - The value to set for the run in background option.</param>
        private void SetRunInBackground(bool runInBackground)
        {
            // Set the run in background option using the Application class
            Application.runInBackground = runInBackground;
            // Save the settings using a custom method
            Save();
        }
        
// ------------------ Quality ------------------
        /// <summary>
        /// Sets the vertical synchronization (vSync) option for the quality settings based on the given index.
        /// </summary>
        /// <param name="vSyncIndex">int - The index of the vSync option to set. 0 means no vSync, 1 means every vBlank and 2 means every second vBlank.</param>
        private void SetVSync(int vSyncIndex)
        {
            // Set the vSync option using the QualitySettings class
            QualitySettings.vSyncCount = vSyncIndex;
            // Save the settings using a custom method
            Save();
        }
        
        /// <summary>
        /// Sets the texture quality option for the quality settings based on the given index.
        /// </summary>
        /// <param name="textureQualityIndex">int - The index of the texture quality option to set. 0 means full resolution, 1 means half resolution and so on.</param>
        private void SetTextureQuality(int textureQualityIndex)
        {
            // Set the texture quality option using the QualitySettings class
            QualitySettings.masterTextureLimit = textureQualityIndex;
            // Save the settings using a custom method
            Save();
        }
        
        /// <summary>
        /// Sets the texture streaming option for the quality settings based on the given boolean value.
        /// </summary>
        /// <param name="textureStreaming">bool - The value to set for the texture streaming option. True means enabled and false means disabled.</param>
        private void SetTextureStreaming(bool textureStreaming)
        {
            // Set the texture streaming option using the QualitySettings class
            QualitySettings.streamingMipmapsActive = textureStreaming;
            // Save the settings using a custom method
            Save();
        }
        
        /// <summary>
        /// Sets the shadow quality option for the quality settings based on the given index.
        /// </summary>
        /// <param name="shadowQualityIndex">int - The index of the shadow quality option to set. 0 means disabled, 1 means hard only, 2 means all.</param>
        private void SetShadowQuality(int shadowQualityIndex)
        {
            // Set the shadow quality option using the QualitySettings class and casting the index to the ShadowQuality enum
            QualitySettings.shadows = (ShadowQuality) shadowQualityIndex;
            // Save the settings using a custom method
            Save();
        }
        
        /// <summary>
        /// Sets the ambient occlusion option for the post-processing effect based on the given boolean value.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a reference to the _ambientOcclusion field, which is an instance of the AmbientOcclusion class from the UnityEngine.Rendering.PostProcessing namespace.
        /// </remarks>
        /// <param name="ambientOcclusion">bool - The value to set for the ambient occlusion option. True means enabled and false means disabled.</param>
        private void SetAmbientOcclusion(bool ambientOcclusion)
        {
            // Set the ambient occlusion option using the _ambientOcclusion field and its active property
            _ambientOcclusion.active = ambientOcclusion;
            // Save the settings using a custom method
            Save();
        }

        /// <summary>
        /// Sets the motion blur option for the post-processing effect based on the given boolean value.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a reference to the _motionBlur field, which is an instance of the MotionBlur class from the UnityEngine.Rendering.PostProcessing namespace.
        /// </remarks>
        /// <param name="motionBlur">bool - The value to set for the motion blur option. True means enabled and false means disabled.</param>
        private void SetMotionBlur(bool motionBlur)
        {
            // Set the motion blur option using the _motionBlur field and its active property
            _motionBlur.active = motionBlur;
            // Save the settings using a custom method
            Save();
        }
        
        /// <summary>
        /// Sets the motion blur intensity for the post-processing effect based on the given float value.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a reference to the _motionBlur field, which is an instance of the MotionBlur class from the UnityEngine.Rendering.PostProcessing namespace.
        /// </remarks>
        /// <param name="motionBlurValue">float - The value to set for the motion blur intensity. The range is from 0 to 1.</param>
        private void SetMotionBlurSlider(float motionBlurValue)
        {
            // Set the motion blur intensity using the _motionBlur field and its intensity property
            _motionBlur.intensity.value = motionBlurValue;
            // Save the settings using a custom method
            Save();
        }

        /// <summary>
        /// Sets the film grain option for the post-processing effect based on the given boolean value.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a reference to the _filmGrain field, which is an instance of the FilmGrain class from the UnityEngine.Rendering.PostProcessing namespace.
        /// </remarks>
        /// <param name="filmGrain">bool - The value to set for the film grain option. True means enabled and false means disabled.</param>
        private void SetFilmGrain(bool filmGrain)
        {
            // Set the film grain option using the _filmGrain field and its active property
            _filmGrain.active = filmGrain;
            // Save the settings using a custom method
            Save();
        }

        /// <summary>
        /// Sets the film grain intensity for the post-processing effect based on the given float value.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a reference to the _filmGrain field, which is an instance of the FilmGrain class from the UnityEngine.Rendering.PostProcessing namespace.
        /// </remarks>
        /// <param name="filmGrainValue">float - The value to set for the film grain intensity. The range is from 0 to 1.</param>
        private void SetFilmGrainSlider(float filmGrainValue)
        {
            // Set the film grain intensity using the _filmGrain field and its intensity property
            _filmGrain.intensity.value = filmGrainValue;
            // Save the settings using a custom method
            Save();
        }
    }
}

