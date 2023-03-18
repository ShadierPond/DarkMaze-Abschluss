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
    }
}