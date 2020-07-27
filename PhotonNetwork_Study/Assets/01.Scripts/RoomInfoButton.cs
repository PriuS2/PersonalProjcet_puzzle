using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class RoomInfoButton : MonoBehaviour
{
    public Text text;
    private LobbyManager _manager;
    private RoomInfo _info;


    private Action<string> _onClickAction;
    

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { OnClick(); });
    }


    public void UpdateText(RoomInfo roomInfo, LobbyManager manager)
    {
        var info = "#" + roomInfo.Name + " (" + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers + ")";
        text.text = info;
        _manager = manager;
        _info = roomInfo;
    }

    public void OnClick()
    {
        _manager.UpdateName(_info.Name);

        // if (_onClickAction != null)
        // {
        //     _onClickAction
        // }
    }


    public void AddOnClickAction(Action<string> action)
    {
        _onClickAction = action;
    }
}
