using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class Algo : MonoBehaviour
{
    public int[] numbers = {1, 2, 3, 4, 5, 6, 7, 8, 9};

    [Range(0, 9)]
    public int num1;
    
    [Range(0, 9)]
    public int num2;



    public int[] array;
    private void Start()
    {
        array = new int[100];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = i;
        }
    }

    public void ElementSwap()
    {
        var temp = numbers[num1];
        numbers[num1] = numbers[num2];
        numbers[num2] = temp;
    }

    public void Swap(int index1, int index2)
    {
        var temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;
    }



    public void Shuffle()
    {
        
        foreach (var num in array)
        {
            var index1 = Random.Range(0, array.Length);
            var index2 = Random.Range(0, array.Length);
            Swap(index1, index2);
        }

        string str = "";
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                str += array[i * j] + "\t";
            }
            str += "\n";
        }

        print(str);
    }

    public void ResetArray()
    {
        array = new int[100];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = i;
        }
    }
}