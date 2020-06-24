using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    public Transform[] transforms;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MovePlayer(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MovePlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MovePlayer(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            MovePlayer(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            MovePlayer(4);
        }
    }

    private void MovePlayer(int num)
    {
        transform.position = transforms[num].position + (Vector3.up * 2);
    }
}
