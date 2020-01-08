using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshParticalSystem : MonoBehaviour
{
    private const int MAX_QUAD_AMOU = 15000;
    private int quadIndex;


    private Mesh mesh;

    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    private void Awake()
    {
        mesh = new Mesh();

        vertices = new Vector3[4 * MAX_QUAD_AMOU];
        uv = new Vector2[4 * MAX_QUAD_AMOU];
        triangles = new int[6 * MAX_QUAD_AMOU];

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 1;
            int spawnindex = AddQuad(mousePos);
            


        }

    }

    private int AddQuad(Vector3 position)
    {
        if (quadIndex >= MAX_QUAD_AMOU) return;//mesh full

        UpdateQuad(quadIndex, position, 0f, new Vector3(1, 1));

        int spawnIndex = quadIndex;
        quadIndex++;
        return spawnIndex;
    }

    private void UpdateQuad(int quadIndex,Vector3 position,float rotation,Vector3 quadSize)
    {
        //Relocate vertices
        int vIndex = quadIndex * 4;
        int vIndex0 = vIndex;
        int vIndex1 = vIndex + 1;
        int vIndex2 = vIndex + 2;
        int vIndex3 = vIndex + 3;

        vertices[vIndex0] = position + Quaternion.Euler(0, 0, rotation - 180) * quadSize;
        vertices[vIndex1] = position + Quaternion.Euler(0, 0, rotation - 270) * quadSize;
        vertices[vIndex2] = position + Quaternion.Euler(0, 0, rotation - 0) * quadSize;
        vertices[vIndex3] = position + Quaternion.Euler(0, 0, rotation - 90) * quadSize;

        //uv
        uv[vIndex0] = new Vector2(0, 0);
        uv[vIndex1] = new Vector2(0, 1);
        uv[vIndex2] = new Vector2(1, 1);
        uv[vIndex3] = new Vector2(1, 0);


        //create triangles
        int tIndex = quadIndex * 6;
        triangles[tIndex + 0] = vIndex0;
        triangles[tIndex + 1] = vIndex1;
        triangles[tIndex + 2] = vIndex2;

        triangles[tIndex + 3] = vIndex0;
        triangles[tIndex + 4] = vIndex2;
        triangles[tIndex + 5] = vIndex3;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
