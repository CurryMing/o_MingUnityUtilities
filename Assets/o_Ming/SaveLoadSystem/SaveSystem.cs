using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


//静态类，工具类
public static class SaveSystem
{
    //PlayerData
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();//二进制
        string path = Application.persistentDataPath + "/player.oMing";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData playerData = new PlayerData(player);

        formatter.Serialize(stream, playerData);
        stream.Close();

    }
    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.oMing";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData playerData = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            return playerData;
        }
        else
        {
            Debug.LogError("Save File is not found in" + path);
            return null;
        }
    }

    //InventoryData
    public static void SaveInventory(Inventory _Inventory)
    {
        Debug.Log(Application.persistentDataPath);
        if (!Directory.Exists(Application.persistentDataPath + "/Game_Data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Game_Data");
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Create(Application.persistentDataPath + "/Game_Data/inventory.txt");

        var json = JsonUtility.ToJson(_Inventory);

        formatter.Serialize(stream, json);

        stream.Close();
    }
    public static void LoadInventory(Inventory _Inventory)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if(File.Exists(Application.persistentDataPath + "/Game_Data/inventory.txt"))
        {
            FileStream stream = File.Open(Application.persistentDataPath + "/Game_Data/inventory.txt", FileMode.Open);

            JsonUtility.FromJsonOverwrite((string)formatter.Deserialize(stream), _Inventory);

            stream.Close();
        }
    }
}
