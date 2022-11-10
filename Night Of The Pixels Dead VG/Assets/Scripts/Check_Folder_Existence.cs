using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;

public class Check_Folder_Existence : MonoBehaviour
{
    public static string pathForMobile;
    private void Awake()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            pathForMobile = Application.persistentDataPath;
            Translation_Manager.pathSaveSlot = $"{pathForMobile}/{Translation_Manager.translationsfolder}";
            Load_Manager.pathSaveSlot = $"{pathForMobile}/{Load_Manager.savesFolder}";
        }

        //TRANSLATION FILES
        if (CreateDirectroyIfNotExist(Translation_Manager.pathSaveSlot))
        {
            CreateXmlInNewFolder("Italiano");
            CreateXmlInNewFolder("English");
        }
        //SAVE AND LOAD FILES
        CreateDirectroyIfNotExist(Load_Manager.pathSaveSlot);
    }

    void CreateXmlInNewFolder(string nameFile)
    {
        TextAsset translation = (TextAsset)Resources.Load($"Translation/{nameFile}");
        XmlDocument xmlTranslation = new XmlDocument();
        xmlTranslation.LoadXml(translation.text);

        xmlTranslation.Save($"{Translation_Manager.pathSaveSlot}/{nameFile}.xml");
    }

    bool CreateDirectroyIfNotExist(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            return true;
        }

        return false;
    }
}
