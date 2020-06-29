using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRDebug : MonoBehaviour
{
    public static VRDebug VrDebug;

    private Text textBox;
    // Start is called before the first frame update
    void Start()
    {
        textBox = transform.GetComponentInChildren<Text>();
        VrDebug = this;
        textBox.text = null;
    }

    public void Print(string text)
    {
        textBox.text = text;
    }

}
