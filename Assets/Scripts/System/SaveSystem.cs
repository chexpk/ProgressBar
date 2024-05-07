using DefaultNamespace.Progress;
using UnityEngine;

namespace DefaultNamespace.System
{
    public class SaveSystem : ISaveSystem
    {
        private readonly IDefaultDataCreator _defaultDataCreator;

        public SaveSystem(IDefaultDataCreator defaultDataCreator)
        {
            _defaultDataCreator = defaultDataCreator;
        }

        public T Load<T>(string profileName) where T : Data, new()
        {
            if (PlayerPrefs.HasKey(profileName))
            {
                var jsonString = PlayerPrefs.GetString(profileName);
                return JsonUtility.FromJson<T>(jsonString);
            }

            return _defaultDataCreator.CreateDefault<T>();
        }

        public void Save<T>(string profileName, T dataSave) where T : Data
        {
            var jsonString = JsonUtility.ToJson(dataSave);
            PlayerPrefs.SetString(profileName, jsonString);
            PlayerPrefs.Save();
        }
    }
}