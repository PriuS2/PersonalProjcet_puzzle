using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerFire : MonoBehaviourPun
{
    public GameObject bullet;
    public float bulletDamage = 1.0f;
    public float launchPower = 1.0f;
    private Transform _camera;

    public GameObject bulletImpact;

    // private Vector3 pos;


    //총쏠때 소리나게

    //private PhotonView _photonView;


    // Start is called before the first frame update
    void Start()
    {
        _camera = transform.Find("Camera");
        if (!photonView.IsMine) return;
        
        
        //_photonView = GetComponent<PhotonView>();
        //Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        // photonView.Owner.

        if (Input.GetButtonDown("Fire1"))
        {
            photonView.RPC("FireBullet", RpcTarget.All);
            // FireBullet();
        }


        if (Input.GetButtonDown("Fire2"))
        {
            photonView.RPC("FireRay", RpcTarget.All);
        }
    }

    [PunRPC]
    private void FireBullet()
    {
        var firePos = _camera.position + _camera.forward * 1.5f;
        var bulletInst = Instantiate(bullet, firePos, Quaternion.identity);

        var dirSpeed = (_camera.forward * launchPower);

        bullet.GetComponent<Bullet>().InitBullet(dirSpeed, bulletDamage);
    }

    [PunRPC]
    private void FireRay()
    {
        Ray ray = new Ray(_camera.position, _camera.forward);
        int mask = ~(1 << LayerMask.NameToLayer("Player"));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, mask))
        {
            var impact = Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 0.5f);
            Destroy(impact, 3);
        }
    }
}