using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathfinderScanAwake : MonoBehaviour
{

    private void Awake()
    {
        StartCoroutine(Scaning());
    }
    IEnumerator Scaning()
    {
        AstarPath.active.Scan();
        yield return null;


    }


}
