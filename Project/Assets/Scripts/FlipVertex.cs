using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlipVertex : MonoBehaviour
{
    private Mesh _mesh;

    [SerializeField] private int[] triangles;

    [SerializeField] private List<Vector3> vertices;

    // Start is called before the first frame update
    void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        
        triangles = _mesh.triangles;
        
        
        //_mesh.GetVertices(vertices);
        
        triangles = _mesh.triangles;
        //_mesh.triangles = triangles.Reverse().ToArray();
        for (int i = 0; i < triangles.Length; i+=3)
        {
            var temp = triangles[i];
            triangles[i] = triangles[i + 1];
            triangles[i + 1] = temp;
        }
        
        _mesh.triangles = triangles;
        
         // _mesh.RecalculateBounds();
         // _mesh.RecalculateNormals();
         // _mesh.RecalculateTangents();

         var nomarls = _mesh.normals;
         for (int i = 0; i < nomarls.Length; i++)
         {
             nomarls[i] *= -1;
         }

         _mesh.normals = nomarls;
         
         
         //_mesh.Optimize();
         



    }

    // Update is called once per frame
    void Update()
    {

    }
}
