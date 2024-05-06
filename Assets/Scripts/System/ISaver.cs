namespace DefaultNamespace.System
{
    public interface ISaver
    {
        void RegisterToSave(ISaving saving);
        void Unregister(ISaving saving);

        SaveData LoadSaveData();

        // ProgressElementsData ProgressElementsData();
        // ProgressCounterData ProgressCounterData();
    }
}