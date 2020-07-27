using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    private CharacterController _controller;
    public float jumpPower = 1.0f;

    public float moveMul = 1.0f;
    public float gravity = -1;
    private float _gravityStack;

    public Text nickName;



    private Vector3 _targetPlayerPosition;
    private Quaternion _targetPlayerRotation;

    // Start is called before the first frame update
    void Start()
    {
        nickName.text = photonView.Owner.NickName;
        
        if (!photonView.IsMine)
        {
            GetComponent<MeshRenderer>().material = new Material(GetComponent<MeshRenderer>().material);
            GetComponent<MeshRenderer>().material.color = new Color(1,0,0,1);
            return;
        }

        _controller = GetComponent<CharacterController>();
        _gravityStack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPlayerPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetPlayerRotation, 0.1f);
            
            
            
            Debug.DrawRay(_targetPlayerPosition, Vector3.up * 3, Color.red);
            
            return;
        }
        
        var z = Input.GetAxis("Horizontal");
        var x = Input.GetAxis("Vertical");
        var jump = Input.GetButtonDown("Jump");

        var moveVec = new Vector3(x, 0, z);
        if (moveVec.magnitude > 1)
        {
            moveVec = moveVec.normalized;
        }

        var forwardVec = transform.forward * moveVec.x * moveMul * Time.deltaTime;
        var rightVec = transform.right * moveVec.z * moveMul * Time.deltaTime;
        moveVec = forwardVec + rightVec;

        if (jump)
        {
            _gravityStack = jumpPower;
        }
        else
        {
            if (!_controller.isGrounded)
            {
                _gravityStack += gravity * Time.deltaTime;
            }
            else
            {
                _gravityStack = 0;
            }
        }
        moveVec.y = _gravityStack;
        _controller.Move(moveVec);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }

        if (stream.IsReading)
        {
            _targetPlayerPosition = (Vector3) stream.ReceiveNext();
            _targetPlayerRotation = (Quaternion) stream.ReceiveNext();
        }
    }
}