using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class DepthCamera : MonoBehaviour
{
    //[Header("Shader Setup")]
    private Shader _uberReplacementShader;
    private Shader _opticalFlowShader;
    private float opticalFlowSensitivity = 1.0f;

    private Camera _camera;

    
    public enum ReplacementMode
    {
        ObjectId = 0,
        CatergoryId = 1,
        DepthCompressed = 2,
        DepthMultichannel = 3,
        Normals = 4
    };
    
    private Material _opticalFlowMaterial;

    private void Start()
    {
        if (!_uberReplacementShader)
            _uberReplacementShader = Shader.Find("Hidden/UberReplacement");

        if (!_opticalFlowShader)
            _opticalFlowShader = Shader.Find("Hidden/OpticalFlow");

        int targetDisplay = 1;
        _camera = GetComponent<Camera>();
        
        if (!_opticalFlowMaterial || _opticalFlowMaterial.shader != _opticalFlowShader)
        {
            _opticalFlowMaterial = new Material(_opticalFlowShader);
        }
        _opticalFlowMaterial.SetFloat("_Sensitivity", opticalFlowSensitivity);
        SetupCameraWithReplacementShader(_camera, _uberReplacementShader, ReplacementMode.DepthCompressed, Color.white);
    }

    static private void SetupCameraWithReplacementShader(Camera cam, Shader shader, ReplacementMode mode, Color clearColor)
    {
        var cb = new CommandBuffer();
        cb.SetGlobalFloat("_OutputMode", (int)mode); // @TODO: CommandBuffer is missing SetGlobalInt() method
        cam.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, cb);
        cam.AddCommandBuffer(CameraEvent.BeforeFinalPass, cb);
        cam.SetReplacementShader(shader, "");
        cam.backgroundColor = clearColor;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.allowHDR = false;
        cam.allowMSAA = false;
    }
    
    public void OnSceneChange()
    {
        var renderers = FindObjectsOfType<Renderer>();
        var mpb = new MaterialPropertyBlock();
        foreach (var r in renderers)
        {
            var id = r.gameObject.GetInstanceID();
            var layer = r.gameObject.layer;
            var tag = r.gameObject.tag;

            mpb.SetColor("_ObjectColor", ColorEncoding.EncodeIDAsColor(id));
            mpb.SetColor("_CategoryColor", ColorEncoding.EncodeLayerAsColor(layer));
            r.SetPropertyBlock(mpb);
        }
    }
}
