namespace Management.SaveSystem
{
    [System.Serializable]
    public class SettingsData
    {
        // Audio Data
        public float masterVolume;
        public bool isMasterMuted;
        public float musicVolume;
        public bool isMusicMuted;
        public float sfxVolume;
        public bool isSfxMuted;
        public float ambientVolume;
        public bool isAmbientMuted;
        
        // Display Data
        public int resolutionIndex;
        public int fullscreenIndex = 1;
        public int refreshRateIndex = 5;
        public bool runInBackground = true;
        
        // Graphics Data
        public int vSyncCount = 1;
        public int textureQuality = 1;
        public bool textureStreaming = true;
        public int shadowQuality = 2;
        public bool ambientOcclusion = true;
        public bool motionBlur = true;
        public float motionBlurShutterAngle = 6;
        public bool filmGrain = true;
        public float filmGrainIntensity = 0.5f;
    }
}