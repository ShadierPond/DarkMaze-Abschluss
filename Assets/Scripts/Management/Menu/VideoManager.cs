using Management.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using MotionBlur = UnityEngine.Rendering.PostProcessing.MotionBlur;
using ScreenSpaceReflections = UnityEngine.Rendering.PostProcessing.ScreenSpaceReflections;
using ShadowQuality = UnityEngine.ShadowQuality;
using FilmGrain = UnityEngine.Rendering.PostProcessing.Grain;

namespace Management.Menu
{
    public class VideoManager : MonoBehaviour
    {
        private PostProcessVolume _postProcessVolume;
        private AmbientOcclusion _ambientOcclusion;
        private Bloom _bloom;
        private MotionBlur _motionBlur;
        private ScreenSpaceReflections _screenSpaceReflections;
        private FilmGrain _filmGrain;
        
        // Display
        private Resolution[] _resolutions;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        private readonly FullScreenMode[] _fullScreenModes = {FullScreenMode.ExclusiveFullScreen, FullScreenMode.FullScreenWindow, FullScreenMode.MaximizedWindow, FullScreenMode.Windowed};
        [SerializeField] private TMP_Dropdown fullscreenDropdown;
        private readonly int[] _refreshRates = {30, 60, 120, 144, 240, 999};
        [SerializeField] private TMP_Dropdown refreshRateDropdown;
        [SerializeField] private Toggle runInBackgroundToggle;

        // Graphics
        [SerializeField] private TMP_Dropdown qualityDropdown;
        [SerializeField] private TMP_Dropdown vSyncDropdown;
        [SerializeField] private TMP_Dropdown textureQualityDropdown;
        [SerializeField] private Toggle textureStreamingToggle;
        [SerializeField] private TMP_Dropdown shadowQualityDropdown;
        [SerializeField] private Toggle ambientOcclusionToggle;
        [SerializeField] private Toggle bloomToggle;
        [SerializeField] private Toggle motionBlurToggle;
        [SerializeField] private Slider motionBlurSlider;
        [SerializeField] private Toggle filmGrainToggle;
        [SerializeField] private Slider filmGrainSlider;
        
        private void Start()
        {
            _resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            foreach (var resolution in _resolutions)
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString().Split('@')[0]));
            resolutionDropdown.RefreshShownValue();
            

            refreshRateDropdown.ClearOptions();
            foreach (var refreshRate in _refreshRates)
                refreshRateDropdown.options.Add(refreshRate == 999
                    ? new TMP_Dropdown.OptionData("Unlimited")
                    : new TMP_Dropdown.OptionData(refreshRate.ToString())
                );
            refreshRateDropdown.RefreshShownValue();

            fullscreenDropdown.ClearOptions();
            foreach (var fullScreenMode in _fullScreenModes)
                fullscreenDropdown.options.Add(new TMP_Dropdown.OptionData(fullScreenMode.ToString()));
            fullscreenDropdown.RefreshShownValue();

            // Get the post processing volume and the settings and the Save data
            var data = SaveManager.Instance.settingsDataClass;
            _postProcessVolume = GameManager.Instance.postProcessVolume;
            _postProcessVolume.profile.TryGetSettings(out _ambientOcclusion);
            _postProcessVolume.profile.TryGetSettings(out _bloom);
            _postProcessVolume.profile.TryGetSettings(out _motionBlur);
            _postProcessVolume.profile.TryGetSettings(out _filmGrain);

            // Get the saved settings
            resolutionDropdown.value = data.resolutionIndex;
            refreshRateDropdown.value = data.refreshRateIndex;
            fullscreenDropdown.value = data.fullscreenIndex;
            resolutionDropdown.RefreshShownValue();
            refreshRateDropdown.RefreshShownValue();
            fullscreenDropdown.RefreshShownValue();
            
            runInBackgroundToggle.isOn = data.runInBackground;
            
            qualityDropdown.value = data.qualityLevel;
            vSyncDropdown.value = data.vSyncCount;
            textureQualityDropdown.value = data.textureQuality;
            textureStreamingToggle.isOn = data.textureStreaming;
            shadowQualityDropdown.value = data.shadowQuality;
            ambientOcclusionToggle.isOn = data.ambientOcclusion;
            bloomToggle.isOn = data.bloom;
            motionBlurToggle.isOn = data.motionBlur;
            motionBlurSlider.value = data.motionBlurShutterAngle;
            filmGrainToggle.isOn = data.filmGrain;
            filmGrainSlider.value = data.filmGrainIntensity;
            
            // Apply the saved settings
            SetScreen(resolutionDropdown.value, fullscreenDropdown.value, refreshRateDropdown.value);
            SetRunInBackground(runInBackgroundToggle.isOn);

            SetQuality(qualityDropdown.value);
            SetVSync(vSyncDropdown.value);
            SetTextureQuality(textureQualityDropdown.value);
            SetTextureStreaming(textureStreamingToggle.isOn);
            SetShadowQuality(shadowQualityDropdown.value);
            SetAmbientOcclusion(ambientOcclusionToggle.isOn);
            SetBloom(bloomToggle.isOn);
            SetMotionBlur(motionBlurToggle.isOn);
            SetMotionBlurSlider(motionBlurSlider.value);
            SetFilmGrain(filmGrainToggle.isOn);
            SetFilmGrainSlider(filmGrainSlider.value);

            // Set the values of the dropdowns to the current settings
            resolutionDropdown.onValueChanged.AddListener(delegate { SetScreen(resolutionDropdown.value, fullscreenDropdown.value, refreshRateDropdown.value); });
            fullscreenDropdown.onValueChanged.AddListener(delegate { SetScreen(resolutionDropdown.value, fullscreenDropdown.value,  refreshRateDropdown.value); });
            refreshRateDropdown.onValueChanged.AddListener(delegate { SetScreen(resolutionDropdown.value, fullscreenDropdown.value,  refreshRateDropdown.value); });
            
            runInBackgroundToggle.onValueChanged.AddListener(delegate { SetRunInBackground(runInBackgroundToggle.isOn); });

            qualityDropdown.onValueChanged.AddListener(delegate { SetQuality(qualityDropdown.value); });
            vSyncDropdown.onValueChanged.AddListener(delegate { SetVSync(vSyncDropdown.value); });
            textureQualityDropdown.onValueChanged.AddListener(delegate { SetTextureQuality(textureQualityDropdown.value); });
            textureStreamingToggle.onValueChanged.AddListener(delegate { SetTextureStreaming(textureStreamingToggle.isOn); });
            shadowQualityDropdown.onValueChanged.AddListener(delegate { SetShadowQuality(shadowQualityDropdown.value); });
            ambientOcclusionToggle.onValueChanged.AddListener(delegate { SetAmbientOcclusion(ambientOcclusionToggle.isOn); });
            bloomToggle.onValueChanged.AddListener(delegate { SetBloom(bloomToggle.isOn); });
            motionBlurToggle.onValueChanged.AddListener(delegate { SetMotionBlur(motionBlurToggle.isOn); });
            motionBlurSlider.onValueChanged.AddListener(delegate { SetMotionBlurSlider(motionBlurSlider.value); });
            filmGrainToggle.onValueChanged.AddListener(delegate { SetFilmGrain(filmGrainToggle.isOn); });
            filmGrainSlider.onValueChanged.AddListener(delegate { SetFilmGrainSlider(filmGrainSlider.value); });
        }

        private void Save()
        {
            var data = SaveManager.Instance.settingsDataClass;
            data.resolutionIndex = resolutionDropdown.value;
            data.fullscreenIndex = fullscreenDropdown.value;
            data.refreshRateIndex = refreshRateDropdown.value;
            data.runInBackground = runInBackgroundToggle.isOn;

            data.qualityLevel = qualityDropdown.value;
            data.vSyncCount = vSyncDropdown.value;
            data.textureQuality = textureQualityDropdown.value;
            data.textureStreaming = textureStreamingToggle.isOn;
            data.shadowQuality = shadowQualityDropdown.value;
            data.ambientOcclusion = ambientOcclusionToggle.isOn;
            data.bloom = bloomToggle.isOn;
            data.motionBlur = motionBlurToggle.isOn;
            data.motionBlurShutterAngle = motionBlurSlider.value;
            data.filmGrain = filmGrainToggle.isOn;
            data.filmGrainIntensity = filmGrainSlider.value;
            SaveManager.Instance.SaveSettings();
        }
        // ------------------ Display ------------------
        private void SetScreen(int resolutionIndex, int fullscreenIndex, int refreshRateIndex)
        {
            Screen.SetResolution(_resolutions[resolutionIndex].width, _resolutions[resolutionIndex].height, _fullScreenModes[fullscreenIndex], _refreshRates[refreshRateIndex]);
            Save();
        }

        private void SetRunInBackground(bool runInBackground)
        {
            Application.runInBackground = runInBackground;
            Save();
        }

        
        
        // ------------------ Graphics ------------------
        private void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            Save();
        }
        
        private void SetVSync(int vSyncIndex)
        {
            QualitySettings.vSyncCount = vSyncIndex;
            Save();
        }
        
        private void SetTextureQuality(int textureQualityIndex)
        {
            QualitySettings.masterTextureLimit = textureQualityIndex;
            Save();
        }
        
        private void SetTextureStreaming(bool textureStreaming)
        {
            QualitySettings.streamingMipmapsActive = textureStreaming;
            Save();
        }
        
        private void SetShadowQuality(int shadowQualityIndex)
        {
            QualitySettings.shadows = (ShadowQuality) shadowQualityIndex;
            Save();
        }
        
        private void SetAmbientOcclusion(bool ambientOcclusion)
        {
            _ambientOcclusion.active = ambientOcclusion;
            Save();
        }
        
        private void SetBloom(bool bloom)
        {
            _bloom.active = bloom;
            Save();
        }
        
        private void SetMotionBlur(bool motionBlur)
        {
            _motionBlur.active = motionBlur;
            Save();
        }
        
        private void SetMotionBlurSlider(float motionBlurValue)
        {
            _motionBlur.shutterAngle.value = motionBlurValue;
            Save();
        }

        private void SetFilmGrain(bool filmGrain)
        {
            _filmGrain.active = filmGrain;
            Save();
        }

        private void SetFilmGrainSlider(float filmGrainValue)
        {
            _filmGrain.intensity.value = filmGrainValue;
            Save();
        }
    }
}