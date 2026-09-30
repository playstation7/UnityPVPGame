using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LogoLanguageController : MonoBehaviour
{
    public Sprite LogoRU;
    public Sprite LogoEN;

    // Start is called before the first frame update
    void Awake()
    {
        if (YandexGame.EnvironmentData.language == "ru")
        {
            transform.GetComponent<Image>().sprite = LogoRU;
        }
        else 
        {
            transform.GetComponent<Image>().sprite = LogoEN;
        }
    }

    
}
