using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteObjPhotoSetting : TextNote_Manager
{
    Image ObjImage;

    // Start is called before the first frame update
    void Start()
    {
        ObjImage = gameObject.GetComponent<Image>();
        ObjImage.color = new Color(ObjImage.color.r, ObjImage.color.g, ObjImage.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        ObjImage.color = new Color(ObjImage.color.r, ObjImage.color.g, ObjImage.color.b, 0);
        ActiveObj(2, "James_Photo");
    }

    void ActiveObj(int numberOfPage, string name)
    {
        if (page == numberOfPage && gameObject.name == name)
        {
            ObjImage.color = new Color(ObjImage.color.r, ObjImage.color.g, ObjImage.color.b, 1);
        }
    }
}
