using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class buffActivate : MonoBehaviour
{
    public GameController.Cell cell;
    private Collider2D trigger;
    private CompositeDisposable disposables = new CompositeDisposable();

    private void Awake()
    {
        trigger = transform.GetComponent<Collider2D>();
        //spawn sound
        Camera.main.transform.GetComponents<AudioSource>()[5].Play();
        trigger.OnTriggerStay2DAsObservable().Where(t => t.gameObject.tag.Equals("Player") && !t.gameObject.GetComponent<PlayerController>().buffStatus).First().Subscribe(x => { Grab(x) ; }).AddTo(disposables);
    }
    private void Grab(Collider2D collision)
    {
        //Debug.Log("Сработало");
        
        if (gameObject.name == "buffPulemet(Clone)") 
        {
            collision.gameObject.GetComponent<PlayerController>().isPulemet = true;
            collision.gameObject.GetComponent<PlayerController>().isShooting = false;
            //taked sound
            Camera.main.transform.GetComponents<AudioSource>()[6].Play();
        }
        if (gameObject.name == "buffLaser(Clone)")
        {
            collision.gameObject.GetComponent<PlayerController>().isLaser = true;
            collision.gameObject.GetComponent<PlayerController>().isShooting = false;
            //taked sound
            Camera.main.transform.GetComponents<AudioSource>()[6].Play();
        }
        if (gameObject.name == "buffRocket(Clone)")
        {
            collision.gameObject.GetComponent<PlayerController>().isRocket = true;
            collision.gameObject.GetComponent<PlayerController>().isShooting = false;
            //taked sound
            Camera.main.transform.GetComponents<AudioSource>()[6].Play();
        }
        Destroy(gameObject);
    }
    
    public void OnDestroy()
    {
        disposables.Clear();
    }
}
