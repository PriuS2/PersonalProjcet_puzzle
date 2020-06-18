using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    private int _killCount;
    public Text text;
    public Text killCountText;

    public PlayerMovement playerMovement;

    //게임의 상태
    public enum GameState
    {
        Ready,
        Run, //공격 및 스폰은 run에만 가능
        Pause,
        GameOver
    };

    public GameState gameState;


    private void Awake()
    {
        gm = this;
        Cursor.lockState = CursorLockMode.Locked;
        
    }


    private void Start()
    {
        //최초 상태는 ready
        gameState = GameState.Ready;
        _killCount = 0;

        //2초 뒤 "Wave Start!" 묵구 출력 , 1초뒤 삭제
        StartCoroutine(StartGame());
        //run 상태로 전환
    }

    private void Update()
    {
        //플레이어의 체력이 0이 되면 게임 오버 상태로 변환

        //플레이어가 적을 처치할 때 마다 화면에 숫자 출력
    }


    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2.0f);
        text.text = "Wave Start!";

        yield return new WaitForSeconds(1.0f);
        text.text = "";

        gameState = GameState.Run;
    }

    public void IncrementKillCount()
    {
        _killCount++;
        if(killCountText) killCountText.text = _killCount.ToString();
    }

    public void PlayerDie()
    {
        gameState = GameState.GameOver;
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        //text.text = "GameOver";

        string msg = "GameOver";

        for (int i = 0; i < msg.Length; i++)
        {
            text.text += msg[i];
            yield return  new WaitForSeconds(.3f);
        }



        yield return null;
    }
}