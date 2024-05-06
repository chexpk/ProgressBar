namespace DefaultNamespace.System
{
    public interface ISaveSystem
    {
        SaveData Load();
        void Save(SaveData saveData);
        // void SaveAll();
    }
}