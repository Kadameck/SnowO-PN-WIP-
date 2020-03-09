using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sorgt dafür das der inhalt der klasse in einer file gespeichert werden kann
[System.Serializable]
// Sammelt alle Daten den Players die in das savegame übertragen werden sollen
public class PlayerData
{
    // Da es in einer binary file gespeichert werden soll müssen die datentypen unabhängig von unity sein und vector3 ist ein datentyp von unity
    // auso ausweichen auf ein float array
    public float[] position;

    // Konstruktor für das playercontrol script
    public PlayerData (PlayerControls player)
    {
        position = new float[3];

        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = 0;
    }
}
