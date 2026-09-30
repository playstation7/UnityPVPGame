using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerGun : MonoBehaviour
{
    private float rotationSpeed = 30f;

    public GameObject vfxObjDes;
    private void Start()
    {
        Camera.main.transform.GetComponents<AudioSource>()[7].Play();
        StartCoroutine("laserGun");
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0f, 0f, 1f * rotationSpeed * Time.deltaTime);
    }

    IEnumerator laserGun() 
    {
        yield return new WaitForSeconds(15f);
        Camera.main.transform.GetComponents<AudioSource>()[7].Stop();
        Camera.main.transform.GetComponents<AudioSource>()[10].Play();
        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            Camera.main.transform.GetComponents<AudioSource>()[7].Stop();
            Destroy(collision.collider.gameObject);
            Instantiate(vfxObjDes, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
            Destroy(gameObject);
        }
    }
}
