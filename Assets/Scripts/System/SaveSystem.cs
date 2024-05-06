using System.IO;
using DefaultNamespace.Progress;
using UnityEngine;

namespace DefaultNamespace.System
{
    public class SaveSystem : ISaveSystem
    {
        private static readonly string FilePath = Application.persistentDataPath + "/Save.json";
        private readonly IDefaultDataCreator _defaultDataCreator;

        public SaveSystem(IDefaultDataCreator defaultDataCreator)
        {
            _defaultDataCreator = defaultDataCreator;
        }

        public SaveData Load()
        {
            if (!File.Exists(FilePath))
            {
                // Debug.Log("Create SaveData");
                var saveData = CreateSaveData();
                Save(saveData);
                return saveData;
            }

            var json = "";
            json = File.ReadAllText(FilePath);
            // using (var reader = new StreamReader(FilePath))
            // {
            //     string line;
            //     while ((line = reader.ReadLine()) != null)
            //     {
            //         json += line;
            //     }
            // }

            if (string.IsNullOrEmpty(json))
            {
                // Debug.Log("Create SaveData");
                var saveData = CreateSaveData();
                Save(saveData);
                return saveData;
            }
            return JsonUtility.FromJson<SaveData>(json);
        }

        public void Save(SaveData data)
        {
            var json = JsonUtility.ToJson(data);
            using (var writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(json);
            }
            File.WriteAllText(path: FilePath, contents: json);
        }

        public void DestroySaveData()
        {
            if (!File.Exists(FilePath))
            {
                Debug.LogAssertion($"Save Profile not found!");
                return;
            }

            Debug.Log(message: $"Successfully deleted {FilePath}");
            File.Delete(FilePath);
        }

        private SaveData CreateSaveData()
        {
            return _defaultDataCreator.DefaultSaveData();
        }
    }
}