using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float vInput;
    float hInput;
    Rigidbody2D rb;
    private float bulletSpeed = 2.5f;
    public GameObject bullet;
    public GameObject bulletPul;
    public GameObject bulletLaser;
    public GameObject expPrefab;
    public GameObject bulletRocket;
    public bool secondPressActive = false;
    GameObject currentLaserManager;
    public GameObject enemy;
    GameObject greenBuff, redBuff;
    bool _isShooting;
    public bool isShooting 
    {
        get { return _isShooting; }
        set
        {
            if (value) _isShooting = buffStatus ? false: true;
            else _isShooting = value; 
        }
    }
    bool _isPulemet;
    public bool isPulemet
    {
        get { return _isPulemet; }
        set { _isPulemet = value; 
            if (gameObject.name == "greentank") greenBuff.GetComponent<buffLabelController>().isPulemet.Value = value; 
            else if (gameObject.name == "redtank") redBuff.GetComponent<buffLabelController>().isPulemet.Value = value;
        }
    }
    bool _isLaser;
    public bool isLaser
    {
        get { return _isLaser; }
        set { _isLaser = value;
            if (gameObject.name == "greentank") greenBuff.GetComponent<buffLabelController>().isLazer.Value = value;
            else if (gameObject.name == "redtank") redBuff.GetComponent<buffLabelController>().isLazer.Value = value;
        }
    }
    bool _isRocket;
    public bool isRocket
    {
        get { return _isRocket; }
        set { _isRocket = value;
            if (gameObject.name == "greentank") greenBuff.GetComponent<buffLabelController>().isRocket.Value = value;
            else if (gameObject.name == "redtank") redBuff.GetComponent<buffLabelController>().isRocket.Value = value;
        }
    }
    private void Awake()
    {
        greenBuff = GameObject.Find("greenBuff");
        redBuff = GameObject.Find("redBuff");
        rb = GetComponent<Rigidbody2D>();
        isShooting = true;
        isPulemet = false;
        isLaser = false;
        isRocket = false;
    }
    public bool buffStatus 
    {
        get { return isPulemet || isLaser || isRocket; }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.name == "greentank") { 
            vInput = Input.GetAxisRaw("Vertical") * 3f;
            hInput = Input.GetAxisRaw("Horizontal") * 250f;
            rb.velocity = transform.up * vInput * Time.fixedDeltaTime * 50;
            rb.rotation += 1f * -hInput * Time.fixedDeltaTime;
        //transform.Rotate(Vector3.forward * -hInput * Time.fixedDeltaTime);
        }
        if (gameObject.name == "redtank")
        {
            vInput = Input.GetAxisRaw("VerticalArrow") * 3f;
            hInput = Input.GetAxisRaw("HorizontalArrow") * 250f;
            rb.velocity = transform.up * vInput * Time.fixedDeltaTime * 50;
            rb.rotation += 1f * -hInput * Time.fixedDeltaTime;
            //transform.Rotate(Vector3.forward * -hInput * Time.fixedDeltaTime);
        }
    }
    IEnumerator Shoot() 
    {
        Camera.main.GetComponents<AudioSource>()[4].Play();
        GameObject gameObject = Instantiate(bullet, transform.position + transform.up * 0.25f, Quaternion.identity);
        gameObject.GetComponent<AddForce>().direction = transform.up * bulletSpeed;
        yield return new WaitForSeconds(10f);
        isShooting = true;
        StopCoroutine("Shoot");
    }
    IEnumerator Pulemet(float time) 
    {
        while (time < 5f) 
        {
            yield return new WaitForSeconds(0.1f);
            Camera.main.GetComponents<AudioSource>()[4].Play();
            GameObject gameObject = Instantiate(bulletPul, transform.position + transform.up * 0.25f + transform.right * Random.Range(-0.1f, 0.1f), Quaternion.identity);
            gameObject.GetComponent<AddForce>().direction = transform.up * bulletSpeed;
            time += 0.1f;
            //fill amount degrees
        }
        
        StopCoroutine("Pulemet");
    }
    IEnumerator Rocket()
    {
        yield return new WaitForSeconds(0.1f);
        Camera.main.GetComponents<AudioSource>()[4].Play();
        GameObject gameObjectRock = Instantiate(bulletRocket, transform.position + transform.up * 0.25f, Quaternion.identity);
        gameObjectRock.GetComponent<RocketTest>().direction = transform.up * bulletSpeed;
        gameObjectRock.GetComponentInChildren<SpriteRenderer>().color = gameObject.name == "greentank" ? Color.green : Color.red;
        isShooting = true;
    }

    GameObject Laser()
    {
        Camera.main.GetComponents<AudioSource>()[4].Play();
        GameObject gameObj = Instantiate(bulletLaser, transform.position + transform.up * 0.25f, Quaternion.identity);
        gameObj.GetComponent<AddForce>().direction = transform.up * bulletSpeed;
        gameObj.GetComponent<AddForce>().parrentTank = this.gameObject;
        return gameObj;
    }
    private void Update()
    {
        if (Time.timeScale == 0f) return;

        if (Input.GetKeyDown(KeyCode.V) && gameObject.name == "greentank" && isShooting)
        {
            isShooting = false;
            StartCoroutine("Shoot");
        }
        if (Input.GetKeyDown(KeyCode.M) && gameObject.name == "redtank" && isShooting)
        {
            isShooting = false;
            StartCoroutine("Shoot");
        }
        if (Input.GetKeyDown(KeyCode.V) && gameObject.name == "greentank"  && secondPressActive)
        {
            if (currentLaserManager && currentLaserManager.GetComponent<AddForce>().LaserCanBeActivate)
            {
                currentLaserManager.GetComponent<AddForce>().LaserFinalActivate();
                
                isShooting = true;
                secondPressActive = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.M) && gameObject.name == "redtank"  && secondPressActive)
        {
            if(currentLaserManager && currentLaserManager.GetComponent<AddForce>().LaserCanBeActivate)
            {
                currentLaserManager.GetComponent<AddForce>().LaserFinalActivate();
                
                isShooting = true;
                secondPressActive = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.V) && gameObject.name == "greentank" && isPulemet)
        {
            StartCoroutine("Pulemet",0);
            Camera.main.GetComponents<AudioSource>()[14].Play();
        }
        if (Input.GetKeyDown(KeyCode.M) && gameObject.name == "redtank" && isPulemet)
        {
            StartCoroutine("Pulemet",0);
            Camera.main.GetComponents<AudioSource>()[14].Play();
        }
        if (Input.GetKeyDown(KeyCode.V) && gameObject.name == "greentank" && isRocket)
        {
            isRocket = false;
            StartCoroutine("Rocket");
        }
        if (Input.GetKeyDown(KeyCode.M) && gameObject.name == "redtank" && isRocket)
        {
            isRocket = false;
            StartCoroutine("Rocket");
        }
        if (Input.GetKeyUp(KeyCode.V) && gameObject.name == "greentank" && isPulemet)
        {
            isPulemet = false;
            isShooting = true;
            StopCoroutine("Pulemet");
        }
        if (Input.GetKeyUp(KeyCode.M) && gameObject.name == "redtank" && isPulemet)
        {
            isPulemet = false;
            isShooting = true;
            StopCoroutine("Pulemet");
        }
        if (Input.GetKeyDown(KeyCode.V) && gameObject.name == "greentank" && isLaser)
        {
            currentLaserManager = Laser();
            secondPressActive = true;
            isLaser = false;
        }
        if (Input.GetKeyDown(KeyCode.M) && gameObject.name == "redtank" && isLaser)
        {
            currentLaserManager = Laser();
            secondPressActive = true;
            isLaser = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bullet") 
        {
            Destroy(collision.collider.gameObject);
            DestroyTank();
        }
        rb.velocity = Vector2.zero;
    }
    public void DestroyTank() 
    {
        Camera.main.GetComponents<AudioSource>()[2].Play();
        Instantiate(expPrefab, transform.position, Quaternion.identity);
        GameObject.Find("GameController").GetComponent<GameController>().gameEnding = true;
        Destroy(gameObject);
    }
}
