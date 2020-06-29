using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySecond : MonoBehaviour
{
    public float destoryAfterSceond;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(destoryAfterSceond);
        GameObject.Destroy(gameObject);
    }
}
