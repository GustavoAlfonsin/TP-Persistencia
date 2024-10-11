using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string savePath = Application.persistentDataPath + "/save";
    private static readonly string listPath = Application.persistentDataPath + "/savesList.save";

    // Guardar una partida
    public static void SavePlayer(Player player, int dropdownValue, int saveID)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = savePath + saveID + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player, dropdownValue, saveID);
        formatter.Serialize(stream, data);
        stream.Close();

        SaveSavesList(saveID); // Guardar la lista de partidas
    }

    // Cargar una partida
    public static PlayerData LoadPlayer(int saveID)
    {
        string path = savePath + saveID + ".save";
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
            Debug.LogError("Archivo de guardado no encontrado en " + path);
            return null;
        }
    }

    // Guardar la lista de IDs de las partidas
    private static void SaveSavesList(int saveID)
    {
        List<int> savesList = LoadSavesList() ?? new List<int>();
        if (!savesList.Contains(saveID))
        {
            savesList.Add(saveID);
            if (savesList.Count > 10) // Limitar a 10 partidas
            {
                savesList.RemoveAt(0);
            }
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(listPath, FileMode.Create);
        formatter.Serialize(stream, savesList);
        stream.Close();
    }

    // Cargar la lista de IDs de partidas guardadas
    public static List<int> LoadSavesList()
    {
        if (File.Exists(listPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(listPath, FileMode.Open);

            List<int> savesList = formatter.Deserialize(stream) as List<int>;
            stream.Close();

            return savesList;
        }
        else
        {
            return new List<int>(); // Si no existe el archivo, devolver una lista vacía
        }
    }
}