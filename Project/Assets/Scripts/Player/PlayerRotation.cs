
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerRotation : MonoBehaviour
{
    // Start is called before the first frame update
    private float _mouseX=0;
    private float _mouseY=0;
    public float mouseSensitive;
    public float angleLimit;
    private PlayerMovement _playerMovement;
    

    void Start()
    {
        // var init = transform.eulerAngles;
        // _mouseY = init.x;
        // _mouseX = init.y;
        _playerMovement = transform.parent.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        _mouseX += Input.GetAxis("Mouse X") * Time.deltaTime* mouseSensitive;
        _mouseY += Input.GetAxis("Mouse Y") * Time.deltaTime* mouseSensitive;

        _mouseY= Mathf.Clamp(_mouseY, -angleLimit, angleLimit);
        
        transform.eulerAngles = new Vector3(-_mouseY, transform.eulerAngles.y, 0);
        _playerMovement.RotateX(_mouseX);
    }
}
