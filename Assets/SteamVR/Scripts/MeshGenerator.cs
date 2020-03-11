using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] indices;
    public int size = 10;
    public static Vector3[] transformMatrix = { new Vector3(10, 0, 0), new Vector3(0, 10, 0), new Vector3(0, 0, 10) };



    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        // render new lines as user moves around?
        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[size  * size * size];
        int i = 0;

        // set mesh vertices
        for (int z = 0; z < size; z++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    vertices[i++] = new Vector3(x*transformMatrix[0].x + y*transformMatrix[0].y + z*transformMatrix[0].z,
                                                x*transformMatrix[1].x + y*transformMatrix[1].y + z*transformMatrix[1].z,
                                                x*transformMatrix[2].x + y*transformMatrix[2].y + z*transformMatrix[2].z);
                    
                }
            }
            
        }
        
        indices = new int[(size * size + size * size + size * size) * 2];
        i = 0;
        int j = 0;
        // Set horizontal lines
        for (int x = 0; x < size * size; x++)
        {
            indices[i++] = j;
            j += (size - 1);
            indices[i++] = j;
            j += 1;
        }

        j = 0;
        int k = size * size - size; // upper left vertex #
        // set vertical lines
        for (int test = 0; test < size; test++)
        {
            for (int y = 0; y < size; y++)
            {
                indices[i++] = j;
                j += 1;
                indices[i++] = k;
                k += 1;
            }
            j = j + (size * size) - size;
            k = k + (size * size) - size;
        }

        // set lines extending back in the z direction
        j = 0;
        k = (size * size * size) - (size * size);
        for (int z = 0; z < size * size; z++)
        {
            indices[i++] = j;
            j += 1;
            indices[i++] = k;
            k += 1;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Lines, 0);
    }
}
