using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBoxObjectTake_Manager : MonoBehaviour
{
    private static GameObject TextBox;
    private static TextMeshPro TextObjRecovered;
    private static TextMeshPro TextRecovered;
    float timer = 0f;
    float maxTime = 2f; //tempo che deve trascorrere per disattivare il box

    // Start is called before the first frame update
    void Start()
    {
        TextBox = GameObject.Find("TextBoxObjTake");
        TextObjRecovered = GameObject.Find("Text_ObjRecovered").GetComponent<TextMeshPro>();
        TextRecovered = GameObject.Find("Text_Recovered").GetComponent<TextMeshPro>();

        TextBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= maxTime)
        {
            timer = 0;
            TextBox.SetActive(false);
        }
    }

    public static void ActivateTextBox(string name, bool recover)
    {
        TextBox.SetActive(true);
        TextObjRecovered.text = name;

        TextRecovered.text = (recover) ?  Translation_Manager.takeObject_TextBox : Translation_Manager.useObject_TextBox;
    }
}
