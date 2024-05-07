namespace DefaultNamespace.System
{
    public interface ISaveSystem
    {
        public T Load<T>(string profileName) where T : Data, new();
        void Save<T>(string profileName, T dataSave) where T : Data;
    }
}