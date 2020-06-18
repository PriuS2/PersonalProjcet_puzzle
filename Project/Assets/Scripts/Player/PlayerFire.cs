using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerFire : MonoBehaviour
{
    //Fire1으로 발사
    //1. 레이 캐스트를 사용해서 사격. -시선방향
    //2. 피격 지점에 이펙트, (머리앞)사운드 발생
    //
    //Input.GetAxis("Fire1");

    private Camera _cam;

    [SerializeField] private List<GameObject> grenadePool = null;
    public int poolSize;
    public GameObject grenade;

    public GameObject bulletImpact;
    private ParticleSystem _bulletParticle;
    public float zoomTime;
    //private float _zoomStack = 0;
    private float _targetFieldOfView = 60;
    private bool _needZoom;
    private bool _zoomIn;

    public int fire1Damage = 5;

    private void Start()
    {
        _cam = Camera.main;
        _bulletParticle = bulletImpact.GetComponent<ParticleSystem>();


        GameObject grenadeParrent = new GameObject("GrenadePool");

        for (int i = 0; i < poolSize; i++)
        {
            var temp = Instantiate(grenade, Vector3.zero, Quaternion.identity);
            temp.transform.SetParent(grenadeParrent.transform);
            var temp2 = temp.GetComponent<Grenade>();
            temp2.playerFire = this;
            temp2.enabled = false;
            temp.SetActive(false);
            grenadePool.Add(temp);
        }
    }

    private void Update()
    {
        if (GameManager.gm.gameState != GameManager.GameState.Run)
        {
            return;
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, 1000))
            {

                // if(hit.transform.gameObject.layer)
                
                //Draw Debug Line
                StartCoroutine(DrawLine(.4f));
                var hitPoint = hit.point;

                //var impact = Instantiate(bulletImpact, hitPoint, Quaternion.identity);
                //
                bulletImpact.transform.position = hitPoint;
                bulletImpact.transform.LookAt(hitPoint + hit.normal, hit.normal);

                if (hit.transform.gameObject.layer == 9)
                {
                    hit.transform.GetComponent<EnemyFSM>().OnDamaged(fire1Damage);
                }
                else
                {
                    _bulletParticle.Play();
                }
            }
        }

        if (Input.GetButtonDown("Fire2") && grenadePool.Count > 0)
        {
            var current = grenadePool[0];
            grenadePool.RemoveAt(0);
            current.SetActive(true);
            current.transform.position = _cam.transform.position + _cam.transform.forward;
            current.transform.rotation = _cam.transform.rotation;
            current.GetComponent<Grenade>().enabled = true;
        }


        if (Input.GetButtonDown("Fire3"))
        {
            if (_zoomIn)
            {
                _targetFieldOfView = 30;
                _needZoom = true;
            }
            else
            {
                _targetFieldOfView = 60;
                _needZoom = true;
            }
            _zoomIn = !_zoomIn;
        }

        if (_needZoom)
        {
            if (Mathf.Abs(_cam.fieldOfView - _targetFieldOfView) > .01f)
            {
                _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _targetFieldOfView, zoomTime*Time.deltaTime);
            }
            else
            {
                _cam.fieldOfView = _targetFieldOfView;
                _needZoom = false;
            }
        }

        
    }

    // IEnumerator Zoom()
    // {
    //     while (_zoomStack < zoomTime)
    //     {
    //         _zoomStack += Time.deltaTime;
    //         yield return null;
    //     }
    // }


    IEnumerator DrawLine(float sec)
    {
        var remainTime = sec;
        while (remainTime > 0)
        {
            remainTime -= Time.deltaTime;
            Debug.DrawRay(_cam.transform.position, _cam.transform.TransformDirection(Vector3.forward) * 1000,
                Color.red);
            yield return null;
        }
    }


    public void ReturnToPool(GameObject gameObject)
    {
        //gameObject.GetComponent<>()
        var temp = gameObject.GetComponent<Grenade>();
        temp.playerFire = this;
        temp.enabled = false;
        gameObject.SetActive(false);


        grenadePool.Add(gameObject);
    }
}