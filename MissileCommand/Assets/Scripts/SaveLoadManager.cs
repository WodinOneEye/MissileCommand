using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveLoadManager
{
    public static void SaveScores(List<HighScoreEntry> highScoreList)
    {
        //create the binary formatter
        BinaryFormatter myFormatter = new BinaryFormatter();

        // create a local path where to save
        string path = Application.persistentDataPath + "\\data.dat";

        //Create a file stream that creates and opens a file
        FileStream stream = new FileStream(path, FileMode.Create);

        //Write data to the file 
        myFormatter.Serialize(stream, highScoreList);

        //close the file
        stream.Close();

    }

    public static List<HighScoreEntry> LoadScores()
    {
        // Read a local path where saved data is
        string path = Application.persistentDataPath + "\\data.dat";

        if (File.Exists(path))
        {
            //create the binary formatter
            BinaryFormatter myFormatter = new BinaryFormatter();
            //Create a file stream that reads a file
            FileStream stream = new FileStream(path, FileMode.Open);
            //Read from the file and create a list of high score entries
            List<HighScoreEntry> myScores = myFormatter.Deserialize(stream) as List<HighScoreEntry>;
            //close the stream
            stream.Close();
            //Return the high score list
            return myScores;
        }
        else
        {
            Debug.LogError("High score save file not found");
            return null;
        }
    }
}
