using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class CalAngle : MonoBehaviour
{
    public Transform pivot;
    public Transform transform1;
    public Transform transform2;

    public TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(pivot.position, transform1.position, Color.green);
        Debug.DrawLine(pivot.position, transform2.position, Color.green);
        Debug.DrawLine(pivot.position, pivot.position + pivot.right*5, Color.red);

        var degree1 = GetDegree(pivot, transform1);
        var degree2 = GetDegree(pivot, transform2);


        text.text = degree1.ToString() + "\n" + degree2.ToString() + "\n" + (degree2-degree1).ToString();
    }

    private float GetDegree(Transform a, Transform b)
    {
        var delta = b.position - a.position;
        //Debug.DrawLine(transform1.position, transform1.position + transform1.right * delta.x, Color.red);
        var divide = delta.z / delta.x;
        var atan = Mathf.Atan(divide);
        var degree = atan * Mathf.Rad2Deg;
        return degree;
    }
    
}
