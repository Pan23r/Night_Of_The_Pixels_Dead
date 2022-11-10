using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class SaveOptions_Manager : MonoBehaviour
{
#if UNITY_ANDROID
    static string pathOptionXml;
#elif UNITY_EDITOR
    static string pathOptionXml = "Assets/";
#else
    static string pathOptionXml = $"{System.Environment.CurrentDirectory}/";
#endif

    static string fileName = "Options.xml";

    public static bool ReturnFileExist()
    {
        if (File.Exists($"{pathOptionXml}{fileName}"))
            return true;

        return false;
    }

    private void Awake()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
            pathOptionXml = $"{Application.persistentDataPath}/";
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateSaveOption();

        XmlTextReader XmlTR = new XmlTextReader($"{pathOptionXml}{fileName}");
        while (XmlTR.Read())
        {
            if (XmlTR.MoveToAttribute("Volume"))
            {
                FMOD_Sound_Manager.vca.setVolume(XmlTR.ReadContentAsFloat());
            }
            
            if (XmlTR.MoveToAttribute("Language"))
            {
                Translation_Manager.translationName = XmlTR.ReadContentAsString();
                Translation_Manager.LoadText();
            }

            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                int resWidth = 1920; 
                int resHeight = 1080;

                if (XmlTR.MoveToAttribute("ResolutionWIdth"))
                {
                    resWidth = XmlTR.ReadContentAsInt();
                }

                if (XmlTR.MoveToAttribute("ResolutionHeight"))
                {
                    resHeight = XmlTR.ReadContentAsInt();
                }

                Screen.SetResolution(resWidth, resHeight, true);
            }            
        }
    }

    public static string ReturnLanguage()
    {
        XmlTextReader XmlTR = new XmlTextReader($"{pathOptionXml}{fileName}");
        while (XmlTR.Read())
        {
            if (XmlTR.MoveToAttribute("Language"))
            {
                return XmlTR.ReadContentAsString();
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector2 ReturnResolutionSaved(Vector2 alternativeResolutionReturned)
    {
        if (File.Exists($"{pathOptionXml}{fileName}"))
        {
            XmlTextReader XmlTR = new XmlTextReader($"{pathOptionXml}{fileName}");
            int resWidth = 1920;
            int resHeight = 1080;

            while (XmlTR.Read())
            {

                if (XmlTR.MoveToAttribute("ResolutionWIdth"))
                {
                    resWidth = XmlTR.ReadContentAsInt();
                }

                if (XmlTR.MoveToAttribute("ResolutionHeight"))
                {
                    resHeight = XmlTR.ReadContentAsInt();
                }
            }

            return new Vector2(resWidth, resHeight);
        }

        return alternativeResolutionReturned;
    }

    void CreateSaveOption()
    {
        if (!File.Exists($"{pathOptionXml}{fileName}"))
        {
            //VIENE CREATO
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("data");
            doc.AppendChild(root);

            XmlElement tileVolume = doc.CreateElement("tile");
            root.AppendChild(tileVolume);
            XmlAttribute attributeVolume = doc.CreateAttribute("Volume");

            float myJustAudio;
            FMOD_Sound_Manager.vca.getVolume(out myJustAudio);
            attributeVolume.Value = myJustAudio.ToString().Replace(',', '.');
            tileVolume.Attributes.Append(attributeVolume);

            XmlElement tileLanguage = doc.CreateElement("tile");
            root.AppendChild(tileLanguage);
            XmlAttribute attributeLanguage = doc.CreateAttribute("Language");
            attributeLanguage.Value = Translation_Manager.translationName;
            tileLanguage.Attributes.Append(attributeLanguage);

            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                XmlElement titleResolutionx = doc.CreateElement("tile");
                root.AppendChild(titleResolutionx);
                XmlAttribute attributeResolutionx = doc.CreateAttribute("ResolutionWIdth");
                attributeResolutionx.Value = "1920";
                titleResolutionx.Attributes.Append(attributeResolutionx);

                XmlElement titleResolutiony = doc.CreateElement("tile");
                root.AppendChild(titleResolutiony);
                XmlAttribute attributeResolutiony = doc.CreateAttribute("ResolutionHeight");
                attributeResolutiony.Value = "1080";
                titleResolutiony.Attributes.Append(attributeResolutiony);
            }

            doc.Save($"{pathOptionXml}{fileName}");
        }
    }

    public static void OverrideSaveOptions()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load($"{pathOptionXml}{fileName}");

        XmlAttribute volumeAttribute = (XmlAttribute)doc.SelectSingleNode("data/tile/@Volume");
        float myJustAudio;
        FMOD_Sound_Manager.vca.getVolume(out myJustAudio);
        volumeAttribute.Value = myJustAudio.ToString().Replace(',', '.');


        XmlAttribute languageAttribute = (XmlAttribute)doc.SelectSingleNode("data/tile/@Language");
        languageAttribute.Value = Translation_Manager.translationName;

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            XmlAttribute screenXAttribute = (XmlAttribute)doc.SelectSingleNode("data/tile/@ResolutionWIdth");
            screenXAttribute.Value = Screen.width.ToString();

            XmlAttribute screenYAttribute = (XmlAttribute)doc.SelectSingleNode("data/tile/@ResolutionHeight");
            screenYAttribute.Value = Screen.height.ToString();
        }

            doc.Save($"{pathOptionXml}{fileName}");
    }
}
