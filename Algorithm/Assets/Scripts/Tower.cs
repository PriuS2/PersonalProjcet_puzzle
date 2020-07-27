using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Tower : MonoBehaviour
{
    public int[] towers;

    private void Update()
    {
        var result = ReceiveCheck(towers);
        var str = "";

        foreach (var value in result)
        {
            str += value + "\t";
        }
        print(str);

        for (int i = 0; i < towers.Length; i++)
        {
            DrawLine(i, towers[i], result[i]);
        }



    }


    private int[] ReceiveCheck(int[] array)
    {
        if (array.Length == 0) return new int[1] {-100};
        
        var result = new int[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            result[i] = 0;
            for (int j = i; j > 0; j--)
            {
                var currentVal = array[i];

                if (array[j] > currentVal)
                {
                    result[i] = j;
                    break;
                }
                //if(부딪히면) //부딪힌인덱스 배열에 넣기
            }
        }
        return result;
    }



    private void DrawLine(int x, int y, int leftIndex)
    {
        var xPos = new Vector3(x, 0, 0);
        var ypos = xPos + Vector3.up * y;
        var lPos = new Vector3(leftIndex, ypos.y, 0);
        Debug.DrawLine(xPos, ypos, Color.blue, 0.1f);
        Debug.DrawLine(ypos, lPos, Color.green, 0.1f);
    }
}
