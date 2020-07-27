using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//2016년 1월 1일 : 금요일
//16년 m월 d일은 무슨요일??
//1: 31
[ExecuteInEditMode]
public class Day : MonoBehaviour
{
    private string[] _day = new [] {"일", "월", "화", "수", "목", "금", "토"};
    private int[] _lastDay = new []{31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

    public int m = 1;
    public int d = 1;


    private void Update()
    {
        print(WhatIsDay(m,d));
    }


    private string WhatIsDay(int m, int d)
    {
        var lastDay = _lastDay[m - 1];


        int dayOffset = 4;
        
        int stackDay = 0;
        for (int i = 0; i < m-1; i++)
        {
            stackDay += _lastDay[i];
        }
        stackDay += d;
        stackDay += dayOffset;

        var dayInt = (stackDay) % 7;
        
        //print(dayInt);
        return _day[dayInt];
    }
}
