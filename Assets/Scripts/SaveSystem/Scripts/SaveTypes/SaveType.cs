namespace GlobalSaveLoad
{
    public abstract class SaveTypeClass
    {
        public abstract void ClearData();
        public abstract void Save(string path);
        public abstract void Load( string path);
    }
}