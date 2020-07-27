using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerRotate : MonoBehaviourPun, IPunObservable
{
    private Transform _camera;


    public float rotateSpeed = 1;

    private Vector3 _playerRot;
    private Vector3 _cameraRot;


    private Quaternion _cameraRotationSync;

    private void Start()
    {
        _camera = transform.Find("Camera");
        if (!photonView.IsMine) return;
        
        
        
        
        _camera.GetComponent<Camera>().enabled = true;
        _camera.GetComponent<AudioListener>().enabled = true;
        _playerRot = transform.localEulerAngles;
        _cameraRot = _camera.localEulerAngles;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            _camera.rotation = Quaternion.Slerp(_camera.rotation, _cameraRotationSync, 0.15f);
            return;
        }
        
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");
        _playerRot.y += mouseX * rotateSpeed * Time.deltaTime;
        _cameraRot.x -= mouseY * rotateSpeed * Time.deltaTime;
        _cameraRot.x = Mathf.Clamp(_cameraRot.x, -80, 80);

        transform.localEulerAngles = _playerRot;
        _camera.localEulerAngles = _cameraRot;

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_camera.rotation);
        }
    
        if (stream.IsReading)
        {
            _cameraRotationSync = (Quaternion)stream.ReceiveNext();
        }
    }
}
