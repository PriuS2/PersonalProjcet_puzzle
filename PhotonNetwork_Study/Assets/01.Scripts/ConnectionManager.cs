using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.Serialization;

//MonoBehaviourPunCallbacks : PUN 이벤트 콜벡
public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public InputField inputId;
    // public Button btnConnection;
    
    //GameVersion
    public string gameVersion;
    
    private void Start()
    {

    }

    public void OnClickConnect()
    {
        //id에 값이 있으면 진행
        if (inputId.text.Length == 0)
        {
            print("ID 가 없습니다");
            return;
        }
        
        //포톤 기본 셋팅
        PhotonNetwork.GameVersion = gameVersion;
        
        //마스터가 Scene전환하면 나머지도 동기화되도록? defalut : false
        // PhotonNetwork.AutomaticallySyncScene = false;
        
        //접속시도(name server)
        PhotonNetwork.ConnectUsingSettings();
    }

    
    //접속 됬을때 //MonoBehaviourPunCallbacks
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }


    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //로비에 들어갈 수 있는 상태

        
        
        //닉네임 설정
        PhotonNetwork.NickName = inputId.text;
        
        //로비 진입
        PhotonNetwork.JoinLobby();// -> Default Lobby
        // PhotonNetwork.JoinLobby(new TypedLobby("MediciLobby", LobbyType.Default)); //특정 로비 join
        
        
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        PhotonNetwork.LoadLevel("LobbyScene");
        
        
        //PhotonNetwork.
    }
}
