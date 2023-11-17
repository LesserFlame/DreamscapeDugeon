using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerController player)
    {
        string path = Application.persistentDataPath + "/player.moxie";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.moxie";
        if (File.Exists(path)) 
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            //Debug.LogError("Save file not found in " + path);
            PlayerData data = new PlayerData();
            return data;
        }
    }
    public static void DeletePlayer()
    {
        string path = Application.persistentDataPath + "/player.moxie";
        if (File.Exists(path)) 
        { 
            File.Delete(path);
        }
    }
}
