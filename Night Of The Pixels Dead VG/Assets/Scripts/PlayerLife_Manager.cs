using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerLife_Manager : MonoBehaviour
{
    public static float life = 100;

    Image lifeGreenImage;
    float originalImageWIdth = 1134;
    public const float maxLife = 100;

    // Start is called before the first frame update
    void Start()
    {
        lifeGreenImage = GameObject.Find("Green").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float myLife = (originalImageWIdth * life) / maxLife;
        lifeGreenImage.rectTransform.sizeDelta = new Vector2(myLife, lifeGreenImage.rectTransform.sizeDelta.y);
    }
}
