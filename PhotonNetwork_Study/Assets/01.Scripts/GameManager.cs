using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.SendRate = 30; //클라이언트 -> 서버  횟수/초
        PhotonNetwork.SerializationRate = 30; // OnPhotonSerializeView 호출 빈도 : 횟수/초 

        PhotonNetwork.Instantiate("Player", new Vector3(0, 1, 0), Quaternion.identity);
    }
}
