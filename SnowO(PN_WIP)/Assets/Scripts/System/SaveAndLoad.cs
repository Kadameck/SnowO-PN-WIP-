using UnityEngine;
using System.IO; // Brauch man immer wenn man mit externen files arbeitet
using System.Runtime.Serialization.Formatters.Binary; // Gibt zugriff auf den "Binary Formatierer" um zu ninary zu formatieren

// static damit die klasse nicht instanziert werden kann
public static class SaveAndLoad
{
    // static damit man es von überall aufrufen kann ohne es zu instanzieren
    public static void SavePlayer(PlayerControls player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        
        // Erstellt über die DefSavePath() Funktion einen geeigneten speicherort
        string savePath = DefFilePath();

        // Erstellt eine filestrem der am definierten pfad eine file erstellen soll
        FileStream stream = new FileStream(savePath, FileMode.Create);

        // Überträgt die werte aus der PlayerData classe in eine variable namens pData
        // Dashier ruft einfach denn konstruktor in der PlayerData class auf und speichert seinen inhalt in der pData variable
        PlayerData pData = new PlayerData(player);

        // Formatter soll eine file schreiben. diese file soll im definierten stream geschrieben sein und pData enthalten
        formatter.Serialize(stream, pData);

        // der stream wird geschlossen weil keine daten mehr an eine externe file übergeben werden sollen
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string loadPath = DefFilePath();

        // Prüft ob es eine File zum laden im angegebenen pfad gibt
        if(File.Exists(loadPath))
        {
            // Ein formatter zum dechiffrieren der ausgelesenen binary file
            BinaryFormatter formatter = new BinaryFormatter();

            // Erstellt eine filestrem der am definierten pfad eine file auslesen soll
            FileStream stream = new FileStream(loadPath, FileMode.Open);

            // Wandelt die binary die durch den stream übermittelt werden wieder in brauchbare daten um und speichert es in pData
            // dafür muss aber gesagt werden was für daten eigentlich ausgelesen werden, daher die extra convertierung in PlayerData
            PlayerData pdata = (PlayerData) formatter.Deserialize(stream);

            // es muss nichts mehr ausgelesen werden also wird der stream beendet
            stream.Close();

            return pdata;

        }
        else
        {
            Debug.LogError("No File found in " + loadPath);
            return null;
        }
    }

    /// <summary>
    /// habe ich in einer extrafunktion gemacht weil dieser pfad beim speichern und laden exakt gleich sein muss
    /// Auf diese weise haben beide Funktionen (Save und Load) den immer identischen pfad, selbst falls man da mal etwas ändern sollte
    /// </summary>
    /// <returns></returns>
    private static string DefFilePath()
    {
        // Erzeugt automatisch einen geeigneten speicherpfad auf dem system (egal ob windows, android usw) und speichert dort alles in einer datei namens "savegame.sav"
        // der name und die dateiendung kann frei gewählt werden ( es ginge auch etwas wie "/dasIstMeineSupperTolleSave.DateiYoMann" oder sowas)
        return Application.persistentDataPath + "/savegame.sav";
    }
}
