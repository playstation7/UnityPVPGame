using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTest : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public Vector2 direction;
    GameObject greentank;
    GameObject redtank;
    private bool autopilot = false;
    public GameObject vfx;
    public GameObject vfxObjDes;

    float timer = 3f;

    GameObject cam;

    private void Start()
    {
        cam = Camera.main.gameObject;
        greentank = GameObject.Find("greentank");
        redtank = GameObject.Find("redtank");
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(direction,ForceMode2D.Impulse);
        StartCoroutine(AutopilotActivate());
        Destroy(gameObject,15f);
        cam.GetComponents<AudioSource>()[13].Play();
    }

    IEnumerator AutopilotActivate()
    {
        yield return new WaitForSeconds(3f);
        autopilot = true;
        GetComponent<AIPath>().canMove = true;
        rigidbody.velocity = Vector2.zero;
        
    }

    private void Update()
    {
        if (!autopilot) 
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -Mathf.Atan2(rigidbody.velocity.x, rigidbody.velocity.y) * Mathf.Rad2Deg);
            timer += Time.deltaTime;
            if (timer > 1.5f)
            {
                cam.GetComponents<AudioSource>()[12].Play();
                timer = 0;
            }

        }
        else 
        {
            if (greentank && redtank)
            {
                if (Vector2.Distance(greentank.transform.position, transform.position) < Vector2.Distance(redtank.transform.position, transform.position))
                {
                    if (GetComponent<AIDestinationSetter>().target != greentank.transform)
                    {
                        GetComponent<AIDestinationSetter>().target = greentank.transform;
                        GetComponentInChildren<ParticleSystem>().startColor = new Color(0.32f, 1f, 0.26f, 0.4f);
                    }

                }
                else
                {
                    if (GetComponent<AIDestinationSetter>().target != redtank.transform)
                    {
                        GetComponent<AIDestinationSetter>().target = redtank.transform;
                        GetComponentInChildren<ParticleSystem>().startColor = new Color(1f, 0.11f, 0.1f, 0.4f);
                    }
                }
                timer += Time.deltaTime;
                if (Vector2.Distance(transform.position, GetComponent<AIDestinationSetter>().target.position) > 10f)
                {
                    if (timer > 1f)
                    {
                        cam.GetComponents<AudioSource>()[12].Play();
                        timer = 0;
                    }

                }
                else
                {
                    if (timer > 0.5f)
                    {
                        cam.GetComponents<AudioSource>()[12].Play();
                        timer = 0;
                    }
                }
            }
            else 
            {
                Destroy(gameObject);
            }

            
        }
        
        
    }

    private void OnDestroy()
    {
        StopCoroutine(AutopilotActivate());
        Instantiate(vfx, transform.position, Quaternion.identity);
        Camera.main.transform.GetComponents<AudioSource>()[3].Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player") 
        { 
            collision.gameObject.GetComponent<PlayerController>().DestroyTank();
            Destroy(gameObject);
        }
        if (collision.collider.tag == "Bullet")
        {
            //Camera.main.transform.GetComponents<AudioSource>()[7].Stop();
            Destroy(collision.collider.gameObject);
            Instantiate(vfxObjDes, transform.position, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f)));
            Destroy(gameObject);
        }
    }


}
