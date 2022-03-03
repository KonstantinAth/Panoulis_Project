using System.IO;
using UnityEngine;
namespace PersonalUtilities.SaveSystem {
    public static class MySaveSystem {
        private static readonly string SAVE_FOLDER = Application.dataPath + "/Json_Saves/";
        private static string path = SAVE_FOLDER + "/gamesave.json";
        public static void InitializeSaveLocation()
        {
            if (!Directory.Exists(SAVE_FOLDER))
            {
                Directory.CreateDirectory(SAVE_FOLDER);
            }
        }
        public static void Save(string saveAsset)
        {
            File.WriteAllText(path, saveAsset);
        }
        public static string Load()
        {
            if (File.Exists(path))
            {
                string saveString = File.ReadAllText(path);
                return saveString;
            }
            else
            {
                return null;
            }
        }
    }
}