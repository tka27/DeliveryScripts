using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    static string savePath = Application.persistentDataPath + "/Save.save";
    public static void Save()
    {
        SaveData saveData = new SaveData();
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);
        formatter.Serialize(stream, saveData);
        stream.Close();
    }
    public static SaveData Load()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);
            SaveData data = (SaveData)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        else
        {
            SaveSystem.Save();
            Debug.Log("Save file was create : " + savePath);
            return null;
        }
    }
    public static void ClearSaveData()
    {
        File.Delete(savePath);
    }
}
