using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddForce : MonoBehaviour
{
   
    GameObject cam;
    public GameObject vfx;
    public Vector3 direction;
    public GameObject lasPrefab;
    public GameObject parrentTank;
    public bool LaserCanBeActivate;


    private void Awake()
    {
        AudioSource audioSource;
        if (TryGetComponent<AudioSource>(out audioSource)) 
        {
            if (FindAnyObjectByType<Singletone>().GetComponent<Singletone>().sound) { audioSource.volume = 1f; }else { audioSource.volume = 0f; }
        }

    }
    private void Start()
    {

        cam = Camera.main.gameObject;
        gameObject.GetComponent<Rigidbody2D>().mass = 10f;
        gameObject.GetComponent<Rigidbody2D>().AddForce(direction*15f,ForceMode2D.Impulse);
        if (gameObject.name != "bulLas(Clone)")
            StartCoroutine("ballDestroy");
        else if (gameObject.name == "bulLas(Clone)") 
        {
            StartCoroutine("ballLasActivate", 5f);
            LaserCanBeActivate = true;
        }

    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            cam.GetComponents<AudioSource>()[Random.Range(0, 2)].Play();

        }
        else if (collision.gameObject.tag == "LaserGun" || collision.gameObject.name == "Rocket(Clone)") 
        {
            cam.GetComponents<AudioSource>()[2].Play();
        }


    }

    IEnumerator ballDestroy() 
    {
        yield return new WaitForSeconds(10f);
        Instantiate(vfx, transform.position, Quaternion.identity);
        Camera.main.transform.GetComponents<AudioSource>()[3].Play();
        Destroy(gameObject);
        StopCoroutine("ballDestroy");
    }


    IEnumerator ballLasActivate(float time)
    {
        //Camera.main.GetComponents<AudioSource>()[8].Play();
        yield return new WaitForSeconds(time);
        LaserFinalActivate();
        
                    
                

    }

    public void LaserFinalActivate() 
    {
        if (parrentTank) 
        {
            parrentTank.GetComponent<PlayerController>().isLaser = false;
            parrentTank.GetComponent<PlayerController>().secondPressActive = false;
            parrentTank.GetComponent<PlayerController>().isShooting = true;

        }
        LaserCanBeActivate = false;
        StopCoroutine("ballLasActivate");
        Instantiate(lasPrefab, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        Instantiate(vfx, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        Camera.main.GetComponents<AudioSource>()[9].Play();
        Destroy(gameObject);

        
    }


    


    








}
