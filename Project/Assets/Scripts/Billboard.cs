using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target;
    public Transform obj;
    public Vector3 uVector3 = new Vector3(0,0,1);
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target & obj)
        {
            obj.LookAt(target,uVector3) ;
        }
    }
}
