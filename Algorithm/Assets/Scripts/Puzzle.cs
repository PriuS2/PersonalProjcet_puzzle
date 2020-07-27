using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Puzzle : MonoBehaviour
{
//클릭한 부분 인덱스 출력
//2. 클릭한부분 위 아래 왼쪽 오른쪽 인덱스 출력하기
// 0,1을 랟덤하게 세팅, 클릭했을때 그 카드와 같은 값을 가지고 이씅면 색변화
    public float elementSize = 100;
    // public Vector2 anchor;
    public Image[] elements;
    public int[] elementValues;
    
    private float _elementWidth;
    public Vector2 pivotPosition;
    
    
    
    private void Start()
    {
        
        _elementWidth = elements[0].rectTransform.rect.width;
        var halfWidth = _elementWidth / 2.0f;
        pivotPosition = elements[0].rectTransform.anchoredPosition - new Vector2(halfWidth, halfWidth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            


            
            
            
            var mousePosition = Input.mousePosition;
            var x = GetIndex(mousePosition.x, pivotPosition.x);
            var y = GetIndex(mousePosition.y, pivotPosition.y);
            var index = x + (y * 3);
            print(x + "," + y + " /index : " + index);
            
            elements[index].color = Color.red;

            var udlr = "상:";
             //위
             if (index < 6)
             {
                 udlr += (index + 3);
             }
             udlr += "/ 하:";
             //아래
             if (index > 3)
             {
                 udlr += index - 3;
             }
             udlr += "/ 좌: ";
            //좌
            if (index % 3 != 0)
            {
                udlr += index - 1;
            }
            udlr += "/우 :";
            //우
            if (index % 3 != 2)
            {
                udlr += index + 1;
            }
            print(udlr);
            
            
            
            //3.
            elementValues = new int[elements.Length];
            for (int i = 0; i < elementValues.Length; i++)
            {
                elementValues[i] = Random.Range(0, 2);
            }

            for(int i=0; i<elementValues.Length; i++)
            if (elementValues[index] == elementValues[i])
            {
                elements[i].color = Color.red;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //색상 리셋

            foreach (var element in elements)
            {
                element.color = Color.white;
            }
        }
    }

    private int GetIndex(float mouse, float pivot)
    {
        int temp = (int) (mouse - pivot)/100;
        return temp;
    }
}