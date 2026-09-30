using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Singletone : MonoBehaviour
{

    public bool sound;
    
    // Метод, выполняемый при старте игры
    void Awake()
    {
        sound = true;
        DontDestroyOnLoad(gameObject);

    }
    

    // Метод инициализации менеджера
    public void SoundChange()
    {
        if (sound)
        {
            sound = false;
            SoundCheck();
        }
        else 
        {
            sound = true;
            SoundCheck();

        }
    }


    public void SoundCheck() 
    {
        Color color = transform.GetComponentInChildren<Image>().color;

        float a;
        if (sound)
        {
            SetVolume(1f);
            a = 1f;
        }
        else 
        {
            SetVolume(0f);
            a = 0.4f;
        }

        transform.GetComponentInChildren<Image>().color = new Color(color.r, color.g, color.b, a);

    }


    void SetVolume(float vlm) 
    {
        foreach(AudioSource t in FindObjectsOfType<AudioSource>()) { t.volume = vlm; }
    }

}