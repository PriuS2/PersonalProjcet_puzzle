using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Centerword : MonoBehaviour
{
    // Start is called before the first frame update
    
    //가운데 글자 반환

    public string str = "";

    private void Update()
    {
        print(GetMiddleChar());
    }


    private string GetMiddleChar()
    {
        if (str.Length == 0) return "null";
        
        var isOddNum = str.Length % 2 == 1 ? true : false;

        var result = "";
        var index =(int) (str.Length * 0.5f);

        if (!isOddNum)
        {
            result += str[index-1];
        }
        result += str[index];

        
        return result;
    }
}
