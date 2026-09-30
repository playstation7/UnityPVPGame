using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject[] rights;
    [SerializeField]
    GameObject[] lefts;
    bool Right;
    bool Left;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && !Left)
        {
            Left = true; 
            lefts[0].GetComponent<Image>().color = new Color(0f,0.5f,0.04f,1f);
            lefts[1].GetComponent<Image>().color = new Color(0f, 0.5f, 0.04f, 1f);
            lefts[1].GetComponent<Animator>().enabled = false;
            Camera.main.GetComponent<AudioSource>().Play();
        }
        if (Input.GetKeyDown(KeyCode.M) && !Right) 
        { 
            Right = true; 
            rights[0].GetComponent<Image>().color = new Color(0.7f, 0f, 0f, 1f);
            rights[1].GetComponent<Image>().color = new Color(0.7f, 0f, 0f, 1f);
            rights[1].GetComponent<Animator>().enabled = false;
            Camera.main.GetComponent<AudioSource>().Play();
        }
        if (Left && Right) StartCoroutine(LDScene());

    }

    IEnumerator LDScene() 
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}
