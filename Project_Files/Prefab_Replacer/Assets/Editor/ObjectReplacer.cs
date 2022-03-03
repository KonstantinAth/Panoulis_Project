using UnityEngine;
using UnityEditor;
namespace PersonalUtilities.SaveSystem {
    public class ObjectReplacer : EditorWindow {
        [SerializeField] private GameObject objectToSpawn;
        [MenuItem("Tools/Object Replacer")]
        static void ReplaceObject() {
            EditorWindow window = GetWindow(typeof(ObjectReplacer));
            GUIContent titleContent = new GUIContent("Object Replacer");
            window.titleContent = titleContent;
        }
        public void OnGUI() {
            objectToSpawn = (GameObject)EditorGUILayout.ObjectField("Prefab", objectToSpawn, typeof(GameObject), false);
            bool wantsToReplace = GUILayout.Button("Replace");
            GUILayout.BeginHorizontal();
            bool wantsToSave = GUILayout.Button("Save Asset");
            bool wantsToLoad = GUILayout.Button("Load Asset");
            GUILayout.EndHorizontal();
            if (wantsToReplace)
            {
                GameObject[] selection = Selection.gameObjects;
                for (int i = selection.Length - 1; i >= 0; i--)
                {
                    GameObject currentSelection = selection[i];
                    PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(objectToSpawn);
                    GameObject replaceGameObject;
                    if (prefabType == PrefabAssetType.Regular)
                    {
                        replaceGameObject = (GameObject)PrefabUtility.InstantiatePrefab(objectToSpawn);
                    }
                    else
                    {
                        replaceGameObject = Instantiate(objectToSpawn) as GameObject;
                        replaceGameObject.name = objectToSpawn.name;
                    }
                    if (objectToSpawn == null)
                    {
                        Debug.LogError("Object To Spawn has not been assigned");
                        break;
                    }
                    Undo.RegisterCreatedObjectUndo(replaceGameObject, "Replace With Prefabs");
                    replaceGameObject.transform.parent = currentSelection.transform.parent;
                    replaceGameObject.transform.localPosition = currentSelection.transform.localPosition;
                    replaceGameObject.transform.localRotation = currentSelection.transform.localRotation;
                    replaceGameObject.transform.localScale = currentSelection.transform.localScale;
                    replaceGameObject.transform.SetSiblingIndex(currentSelection.transform.GetSiblingIndex());
                    Undo.DestroyObjectImmediate(currentSelection);
                }
            }
            if(wantsToSave) { SaveAsset(); }
            if(wantsToLoad) { LoadAsset(); }
        }
        SaveData GetSaveData() {
            SaveData saveData = new SaveData();
            saveData.objectToSpawn = objectToSpawn;
            return saveData;
        }
        public void SaveAsset() {
            SaveData saveData = GetSaveData();
           string Json =  JsonUtility.ToJson(saveData);
            MySaveSystem.Save(Json);
        }
        public void LoadAsset() {
            string savedData = MySaveSystem.Load();
            if(savedData != null) {
                SaveData saveData = JsonUtility.FromJson<SaveData>(savedData);
                objectToSpawn = saveData.objectToSpawn;
            }
        }
    }
}
[System.Serializable]
public class SaveData {
    [SerializeField] public GameObject objectToSpawn;
}
