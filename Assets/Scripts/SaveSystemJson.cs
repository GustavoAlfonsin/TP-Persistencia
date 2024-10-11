using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public static class SaveSystemJson 
{
    private static readonly string savePath = Application.persistentDataPath + "/saveJson";
    private static readonly string listPath = Application.persistentDataPath + "/savesJsonList.json";

    public static void SavePlayer(Player player, int dropdownValue, int saveID)
    {
        string path = savePath + saveID + ".json";
        PlayerData data = new PlayerData(player, dropdownValue, saveID);

        string cadenaJSON = JsonUtility.ToJson(data); //convierte los datos a un archivo Json
        File.WriteAllText(path, cadenaJSON); //Llena el archivo
        //SaveSavesList(saveID);
        SaveSavesList2(saveID);

        Debug.Log("Archivo Guardado");
    }

    // Cargar una partida
    public static PlayerData LoadPlayer(int saveID)
    {
        string path = savePath + saveID + ".json";
        if (File.Exists(path))
        {
            string contenido = File.ReadAllText(path); // Lee el contenido del archivo
            PlayerData data = JsonUtility.FromJson<PlayerData>(contenido); // Lo pasa de un archivo Json a un objeto del tipo especificado
            return data;
        }
        else
        {
            Debug.LogError("Archivo de guardado no encontrado en " + path);
            return null;
        }
    }


    public static void deleteGame(int saveID)
    {
        string path = savePath + saveID + ".json";
        if (File.Exists(path))
        {
            File.Delete(path); // Elimina el archivo si existe
            deleteSavesList(saveID); // Actualiza la lista de partidas
        }
        else
        {
            Debug.LogError("El archivo no se encuentra en " + path);
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
        string cadenaJson = JsonUtility.ToJson(savesList); // NO SE GUARDAN LAS LISTAS EN JSON
        File.WriteAllText(listPath, cadenaJson);
    }

    // Nueva funcion para guardar las partidas en un archivo Json
    private static void SaveSavesList2(int saveID)
    {
        SaveList saveList2 = LoadSavesList2() ?? new SaveList();
        if (!saveList2.listasPartidas.Contains(saveID))
        {
            saveList2.listasPartidas.Add(saveID);
            if (saveList2.cantidad > 10) // Limitar a 10 partidas
            {
                saveList2.listasPartidas.RemoveAt(0);
            }
        }
        string cadenaJson = JsonUtility.ToJson(saveList2);
        File.WriteAllText(listPath, cadenaJson);
    }

    // Cargar la lista de IDs de partidas guardadas
    public static List<int> LoadSavesList()
    {
        if (File.Exists(listPath))
        {
            string contenido = File.ReadAllText(listPath);
            List<int> listaSaves = JsonUtility.FromJson<List<int>>(contenido);
            Debug.Log("ID guardado a la lista");
            return listaSaves;
        }
        else
        {
            return new List<int>(); // Si no existe el archivo, devolver una lista vacía
        }
    }

    // Nueva función para cargar las partidas guardadas
    public static SaveList LoadSavesList2()
    {
        if (File.Exists(listPath))
        {
            string contenido = File.ReadAllText(listPath);
            SaveList listaSaves = JsonUtility.FromJson<SaveList>(contenido);
            Debug.Log("ID guardado a la lista");
            return listaSaves;
        }
        else
        {
            return null; // Si no existe el archivo, devolver una lista vacía
        }
    }

    // Función para actualizar la lista de partidas cuando es eliminada una
    private static void deleteSavesList(int saveID)
    {
        SaveList saveList = LoadSavesList2() ?? new SaveList();
        List<int> nuevaLista = new List<int>();
        foreach (int i in saveList.listasPartidas)
        { // Guarda aquellas partidas que no son las que se tienen que eliminar
            if (i != saveID)
            {
                nuevaLista.Add(i);
            }
        }
        saveList.listasPartidas.Clear();
        saveList.listasPartidas = nuevaLista;
        string cadenaJson = JsonUtility.ToJson(saveList);
        File.WriteAllText(listPath, cadenaJson);
    }

}
