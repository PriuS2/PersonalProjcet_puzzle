using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// using Valve.VR.InteractionSystem;

public class PlayerMovement : MonoBehaviour
{
    private float _inputHorizontal;
    private float _inputVertical;
    private Transform _body;
    private Vector3 _initRot;
    private bool _jumpStart;
    private CharacterController _controller;

    private float _yVelocity = 0;
    private int _remainJump;

    public float moveSpeed;
    public float jumpPower;
    public float limitTimeStack;
    public float gravityScale;
    public int jumpStack;
    public OVRHandInput leftHand;
    public OVRHandInput rightHand;
    public Transform cameraAnchor;

    public Slider slider;
    public Image hitImage;

    public int maxHp = 100;
    public MeshRenderer cameraFadeMeshRenderer;
    private Material _cameraFadeMaterial;
    private int _currentHp;

    private DrawTeleportArc _teleportArc;


    // Start is called before the first frame update
    private void Start()
    {
        //cameraFadeMaterial = GetComponent<MeshRenderer>().material
        _cameraFadeMaterial = cameraFadeMeshRenderer.material;
        _remainJump = jumpStack;
        _body = transform;
        _initRot = transform.eulerAngles;
        _controller = GetComponent<CharacterController>();

        _currentHp = maxHp;
        UpdateHpSlide();

        //GameManager.gm.playerMovement = this;
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.lockState = CursorLockMode.Confined;
        _teleportArc = GetComponent<DrawTeleportArc>();
    }

    // Update is called once per frame
    private void Update()
    {
#if USE_VR
        var resultLeft = leftHand.GetMoveInput();
        var resultRight = rightHand.GetMoveInput();

        if (resultLeft.State)
        {
            _inputHorizontal = resultLeft.Horizontal;
            _inputVertical = resultLeft.Vertical;
        }
        else if (resultRight.State)
        {
            _inputHorizontal = resultRight.Horizontal;
            _inputVertical = resultRight.Vertical;
        }
        else
        {
            _inputHorizontal = 0.0f;
            _inputVertical = 0.0f;
        }
        
        var forward = cameraAnchor.forward;
        forward = new Vector3(forward.x, 0, forward.z).normalized;
        forward *= _inputHorizontal;
        
        var right = cameraAnchor.right;
        right = new Vector3(right.x, 0, right.z).normalized;
        right *= _inputVertical;
        
        var dir = forward + right;
        
        _inputVertical = -dir.x;
        _inputHorizontal = -dir.z;

#else
        _inputHorizontal = Input.GetAxis("Horizontal");
        _inputVertical = Input.GetAxis("Vertical");
#endif
        
        if (Input.GetButtonDown("Jump"))
        {
            if (_controller.collisionFlags == CollisionFlags.Below || _controller.isGrounded)
            {
                _remainJump = jumpStack;
                _yVelocity = 0;
            }

            if (_remainJump > 0)
            {
                _remainJump--;
                _jumpStart = true;
            }
        }


        if (_controller.collisionFlags == CollisionFlags.Above || _controller.collisionFlags == CollisionFlags.Below)
        {
            _yVelocity = 0;
        }


        //텔레포트
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _teleportArc.DrawStart();
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            var destination = _teleportArc.DrawEnd();
            // print(destination);
            PlayerTeleport(destination);
            return;
        }

        var resultMove = Vector3.zero;
        resultMove += Move(_inputHorizontal, _inputVertical);
        Gravity();

        if (_jumpStart)
        {
            _jumpStart = false;
            _yVelocity = jumpPower * .01f; // * Time.deltaTime;
        }

        resultMove += new Vector3(0, _yVelocity, 0);
        _controller.Move(resultMove);
    }

    public void DrawTeleportArc()
    {
        _teleportArc.DrawStart();
    }
    
    public void DrawTeleportArc(Transform dir)
    {
        _teleportArc.launchTransform = dir;
        _teleportArc.DrawStart();
        
    }

    public void ExcuteTeleport()
    {
        var destination = _teleportArc.DrawEnd();
        PlayerTeleport(destination);
    }
    
    private Vector3 Move(float horizontalInput, float verticalInput)
    {
        var deltaTime = Time.deltaTime;
        var dir = new Vector3(horizontalInput, 0, verticalInput);
        if (dir.magnitude > 1)
        {
            dir = dir.normalized;
        }

        var move = dir * (moveSpeed * Time.deltaTime);
        move = transform.TransformDirection(move);

        return move;
    }

    private void Gravity()
    {
        _yVelocity -= gravityScale * Time.deltaTime;
    }

    public void RotateX(float rotX)
    {
        transform.eulerAngles = _initRot + new Vector3(0, rotX, 0);
    }
    
    public void OnDamaged(int damage)
    {
        _currentHp -= damage;
        UpdateHpSlide();
        if (_currentHp <= 0)
        {
            _currentHp = 0;
            //GameManager.gm.PlayerDie();
        }

        StartCoroutine(HitEffect());
    }
    
    private void UpdateHpSlide()
    {
        var check = _currentHp > 0;
        slider.value = check ? _currentHp / (float) maxHp : 0;
    }


    private float _targetAlpha = .3f;
    IEnumerator HitEffect()
    {
        float timeStack = 0;
        _targetAlpha = .3f;
        while (true)
        {
            ChangeAlpha();
            timeStack += Time.deltaTime;
            if (timeStack >= .5f)
            {
                break;
            }

            yield return null;
        }

        timeStack = 0;
        _targetAlpha = 0f;
        while (true)
        {
            ChangeAlpha();
            timeStack += Time.deltaTime;
            if (timeStack >= .5f)
            {
                break;
            }

            yield return null;
        }
    }

    private void ChangeAlpha()
    {
        var currentAlpah = hitImage.color.a;
        var newAlpah = Mathf.Lerp(currentAlpah, _targetAlpha, Time.deltaTime * 10);
        hitImage.color = new Color(1, 0, 0, newAlpah);
    }


    private bool isTeleport = false;

    private void PlayerTeleport(Vector3 destination)
    {
        StartCoroutine(CameraFade(destination));
    }

    IEnumerator CameraFade(Vector3 destination)
    {
        if (isTeleport) yield break;

        isTeleport = true;
        var time = .2f;
        //var targetAlpha = 1;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            var newAlpha = i / time;
            _cameraFadeMaterial.color = new Color(0, 0, 0, newAlpha);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        transform.position = destination + Vector3.up;


        for (float i = 0; i < time; i += Time.deltaTime)
        {
            var newAlpha = 1 - i / time;
            _cameraFadeMaterial.color = new Color(0, 0, 0, newAlpha);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        isTeleport = false;
    }
}