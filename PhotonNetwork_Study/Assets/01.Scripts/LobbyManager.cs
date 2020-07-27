using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

// using UnityEngine.UIElements;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    public InputField inputRoomName;
    public InputField inputMaxPlayer;

    public Button btnJoinRoom;
    public Button btnCreateRoom;



    private void Start()
    {
        inputRoomName.onValueChanged.AddListener(delegate { InputRoomNameChanged(); });
        inputMaxPlayer.onValueChanged.AddListener(delegate { InputMaxPlayerChanged(); });

        //람다식
        // inputRoomName.onValueChanged.AddListener((string text) =>
        // {
        //     btnJoinRoom.interactable = text.Length > 0;
        //     InputMaxPlayerChanged();
        // });

        btnJoinRoom.onClick.AddListener(delegate { OnClickJoinRoom(); });
        btnCreateRoom.onClick.AddListener(delegate { OnClickCreateRoom(); });


        inputMaxPlayer.contentType = InputField.ContentType.IntegerNumber;
    }

    private void InputRoomNameChanged()
    {
        btnJoinRoom.interactable = inputRoomName.text.Length > 0;
        InputMaxPlayerChanged();
    }

    private void InputMaxPlayerChanged()
    {
        btnCreateRoom.interactable = inputMaxPlayer.text.Length > 0 && inputRoomName.text.Length > 0;
    }


    public void CreateContents(string[] rooms)
    {
        var roomNum = rooms.Length;
    }


    public void OnClickCreateRoom()
    {
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = Byte.Parse(inputMaxPlayer.text);
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.CreateRoom(inputRoomName.text, roomOptions);
    }

    //방생성 완료
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(System.Reflection.MethodBase.GetCurrentMethod().Name + "/ code : " + returnCode + " / message : " +
              message);
    }


    //방입장 시도
    public void OnClickJoinRoom()
    {
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        PhotonNetwork.JoinRoom(inputRoomName.text);
    }


    //방입장 완료
    public override void OnJoinedRoom()
    {
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        base.OnJoinedRoom();
        
        PhotonNetwork.LoadLevel("GameScene");
    }

    //방 참가 실패
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        print(System.Reflection.MethodBase.GetCurrentMethod().Name + "\n /code : " + returnCode + "\n /message : " +
              message);
    }


    private Dictionary<string, RoomInfo> _cacheRoom = new Dictionary<string, RoomInfo>();
    private Dictionary<string, GameObject> _cacheRoomObject = new Dictionary<string, GameObject>();
    public GameObject roomUIElement;
    public Transform contentParent;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);


        UpdateCacheRoom(roomList);
    }

    private void UpdateCacheRoom(List<RoomInfo> roomList)
    {
        foreach (var roomInfo in roomList)
        {
            var roomName = roomInfo.Name;
            if (_cacheRoom.ContainsKey(roomName))//이미 있음
            {
                if (roomInfo.RemovedFromList)
                {
                    _cacheRoom.Remove(roomName);
                    Destroy(_cacheRoomObject[roomName]);
                    _cacheRoomObject.Remove(roomName);
                }
                else
                {
                    //갱신
                    _cacheRoom.Add(roomName, roomInfo);
                }
            }
            else
            {
                //방 새로 생성
                _cacheRoom.Add(roomName, roomInfo);
                _cacheRoomObject.Add(roomName, Instantiate(roomUIElement, contentParent));
            }
        }


        foreach (var roomInfo in _cacheRoom.Values)
        {
            var info = "#" + roomInfo.Name + " (" + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers + ")";
            _cacheRoomObject[roomInfo.Name].GetComponent<RoomInfoButton>().UpdateText(roomInfo, this);//transform.GetChild(0).GetComponent<Text>().text = info;
            print(info);
        }


        var height = _cacheRoom.Values.Count * 100.0f;
        var rect = contentParent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.rect.width, height);
        
    }

    public void UpdateName(string name)
    {
        inputRoomName.text = name;
    }
    
}