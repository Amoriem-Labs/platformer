using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveUtil : MonoBehaviour
{
    #region Methods to read, write, and delete save files.
    // `ref` keyword to make sure saveData is passed by reference
    public static void ReadFile(ref SaveData saveData, int saveIndex)
    {
        // Generate the pathway to get the data
        string saveFile = GetSaveFilePath(saveIndex);
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);

            // Deserialize the JSON data into a pattern matching the SaveData class.
            saveData = JsonUtility.FromJson<SaveData>(fileContents);

            // TODO: Populate the scene accordingly in PD.
        }
    }

    public static void WriteFile(ref SaveData saveData, int saveIndex)
    {
        // Serialize the object into JSON and save string.
        string jsonString = JsonUtility.ToJson(saveData);

        // Generate the pathway to store the data
        string saveFile = GetSaveFilePath(saveIndex);

        Debug.Log($"Save file path is: {saveFile}");

        // Write JSON to file.
        File.WriteAllText(saveFile, jsonString);
    }

    // Deletes save file from local system
    public static void DeleteSaveFile(int saveIndex){
        string saveFile = GetSaveFilePath(saveIndex);
        File.Delete(saveFile);
        #if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh();
		#endif
    }

    // Get the filepath for this saveIndex
    public static string GetSaveFilePath(int saveIndex)
    {
        // saveIndex 0 is autosave
        return Application.persistentDataPath + "/save" + saveIndex + "data.json";
    }
    #endregion
}
