using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int level;
    public float health;
    public UiManager uiManager;
    // Eventos
    public static event Action<int, int> OnSave; // saveID a�adido
    public static event Action<UiManager, int> OnLoad; // saveID a�adido
    public static event Action<UiManager,int> OnRemove; 

    public void OnEnable()
    {
        //OnSave += SaveGame;
        //OnLoad += LoadGame;

        OnSave += SaveGame2;
        OnLoad += LoadGame2;
        OnRemove += DeleteGame;
    }

    public void OnDisable()
    {
        //OnSave -= SaveGame;
        //OnLoad -= LoadGame;

        OnSave -= SaveGame2;
        OnLoad -= LoadGame2;
        OnRemove -= DeleteGame;
    }

    public  void SaveGame(int dropdownValue, int saveID)
    {
        SaveSystem.SavePlayer(this, dropdownValue, saveID);
        Debug.Log("Juego guardado con ID: " + saveID);
    }

    //funci�n con el sistema Json
    public void SaveGame2(int dropdownValue, int saveID)
    {
        SaveSystemJson.SavePlayer(this, dropdownValue, saveID);
        Debug.Log("Juego guardado con ID: " + saveID);
    }

    public void cargar()
    {
        
    }

    public void LoadGame(UiManager uiManager, int saveID)
    {
        PlayerData data = SaveSystem.LoadPlayer(saveID);

        if (data != null)
        {
            level = data.level;
            health = data.health;

            // Restaurar la posici�n del jugador
            Vector3 loadedPosition = new Vector3(data.positionX, data.positionY, data.positionZ);
            transform.position = loadedPosition;

            // Restaurar el valor del Dropdown
            uiManager.TMP_Dropdown.value = data.dropdownValue;

            Debug.Log("Juego cargado. Posici�n restaurada.");
        }
    }

    //funci�n con el sistema Json
    public void LoadGame2(UiManager uiManager, int saveID)
    {
        PlayerData data = SaveSystemJson.LoadPlayer(saveID);

        if (data != null)
        {
            level = data.level;
            health = data.health;

            // Restaurar la posici�n del jugador
            Vector3 loadedPosition = new Vector3(data.positionX, data.positionY, data.positionZ);
            transform.position = loadedPosition;

            // Restaurar el valor del Dropdown
            uiManager.TMP_Dropdown.value = data.dropdownValue;

            Debug.Log("Juego cargado. Posici�n restaurada.");
        }
    }

    // Funci�n para borrar partidas
    public void DeleteGame(UiManager uiManager, int saveID)
    {
        SaveSystemJson.deleteGame(saveID);

    }

    public static void TriggerSave(int dropdownValue, int saveID)
    {
        OnSave?.Invoke(dropdownValue, saveID);
    }

    public static void TriggerLoad(UiManager uiManager, int saveID)
    {
        OnLoad?.Invoke(uiManager, saveID);
    }


    public static void TriggerDelete(UiManager uiManager, int saveID)
    {
        OnRemove?.Invoke(uiManager, saveID);
    }
}