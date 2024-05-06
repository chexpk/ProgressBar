namespace DefaultNamespace.System
{
    public interface ISaver
    {
        void RegisterToSave(ISaving saving);
        void Unregister(ISaving saving);

        ProgressElementsData ProgressElementsData();
        ProgressCounterData ProgressCounterData();
    }
}