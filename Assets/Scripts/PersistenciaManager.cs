using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersistenciaManager : MonoBehaviour
{
    public UiManager uiManager;
    public TMP_Dropdown saveDropdown;
    public Button saveButton;
    public Button loadButton;
    public Button deleteButton;

    private List<int> savesList;

    private void Start()
    {
        // Cargar las partidas guardadas al inicio y llenar el Dropdown
        //savesList = SaveSystem.LoadSavesList();

        // Cargar las partidas guardadas con Json al inicio y llenar el Dropdown
        savesList = SaveSystemJson.LoadSavesList2().listasPartidas;
        UpdateDropdown();

        // Configurar eventos de los botones
        saveButton.onClick.AddListener(OnSaveButtonClicked);
        loadButton.onClick.AddListener(OnLoadButtonClicked);
        deleteButton.onClick.AddListener(OnDeleteButtonClicker); //boton de borrar

        if (savesList.Count > 0)
        {
            Player.TriggerLoad(uiManager, savesList[savesList.Count - 1]); // Cargar la última partida
        }
    }

    // Llamado al hacer clic en el botón de guardar
    private void OnSaveButtonClicked()
    {
        //int saveID = savesList.Count + 1; // Generar nuevo ID para la partida
        // Cambie la forma de seleccionar el ID por que al borrar la partida habian problemas
        // con el indice
        int saveID = savesList.OrderByDescending(x => x).FirstOrDefault() + 1; 
        Player.TriggerSave(uiManager.TMP_Dropdown.value, saveID);
        //savesList = SaveSystem.LoadSavesList(); // Actualizar la lista de partidas guardadas
        
        savesList = SaveSystemJson.LoadSavesList2().listasPartidas; // Actualizar la lista de partidas guardadas en Json
        UpdateDropdown();
    }

    // Llamado al hacer clic en el botón de cargar
    private void OnLoadButtonClicked()
    {
        int selectedSaveID = savesList[saveDropdown.value];
        Player.TriggerLoad(uiManager, selectedSaveID);
    }

    private void OnDeleteButtonClicker() // funcion para borrar la partida
    {
        int selectedSaveID = savesList[saveDropdown.value];
        Player.TriggerDelete(uiManager, selectedSaveID);

        savesList = SaveSystemJson.LoadSavesList2().listasPartidas; // Actualizar la lista de partidas guardadas en Json
        UpdateDropdown();

        if (savesList.Count > 0) // carga la ultima partida de la lista
        {
            Player.TriggerLoad(uiManager, savesList[savesList.Count - 1]); // Cargar la última partida
        }
    }

    // Actualizar el Dropdown con las partidas guardadas
    private void UpdateDropdown()
    {
        saveDropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach (int saveID in savesList)
        {
            options.Add("Partida " + saveID);
        }

        saveDropdown.AddOptions(options);
    }
}