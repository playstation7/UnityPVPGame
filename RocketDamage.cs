using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketDamage : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player") { collision.GetComponent<PlayerController>().DestroyTank(); Destroy(gameObject.GetComponentInParent<Transform>().gameObject); }
    }
}