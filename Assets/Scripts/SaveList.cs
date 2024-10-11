using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveList
{
    public List<int> listasPartidas;

    public int cantidad { get => listasPartidas.Count;}

    public SaveList()
    {
        listasPartidas = new List<int>();
    }
}
