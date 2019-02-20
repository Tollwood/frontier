using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager 
{
    private static readonly string SAVE_FOLDER = "/saves/";
    private static BinaryFormatter bf = new BinaryFormatter();

    public static void Save(string saveFile, object objectToSave)
    {
        Directory.CreateDirectory(Application.persistentDataPath + SAVE_FOLDER);
        using (FileStream fs = File.OpenWrite(Application.persistentDataPath + SAVE_FOLDER + saveFile))
        {
            bf.Serialize(fs,objectToSave);
        }
    }

    public static T Load<T>(string saveFile)
    {
        Directory.CreateDirectory(Application.persistentDataPath + SAVE_FOLDER);
        using (FileStream fs = File.OpenRead(Application.persistentDataPath + SAVE_FOLDER + saveFile))
        {
            return (T)bf.Deserialize(fs);
        }
    }
}