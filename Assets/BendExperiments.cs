using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendExperiments : MonoBehaviour
{
    private Vector3[] _vertices;
    private Mesh _mesh;

    public int widthX;
    public int widthY;

    public float speedWave;

    public Transform centerWave;
    private Vector2 centerPoint => new Vector2(centerWave.position.x * 2, centerWave.position.z * 2);

    public AnimationCurve CurveOfWave;
    public AnimationCurve CurveFading;

    private Quad[] quads;

    float bias = -20f;

    void Start()
    {
        Generate();
    }

    Vector3 mousePos;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            centerWave.position = hit.point;
        }
//                mousePos = new Vector3(Input.mousePosition.x / 25f, 0f, Input.mousePosition.y / 20f);
//                centerWave.position = mousePos;

//        centerWave.position = new Vector3(Input.mousePosition.x / Screen.width, 0f, Input.mousePosition.y / Screen.height);

        Generate();
    }

    private void Generate()
    {
        bias += Time.deltaTime * speedWave;
        if (bias > 30f) 
        {
            bias = -10f;
        }

        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        quads = new Quad[widthX * widthY];
        _vertices = new Vector3[quads.Length* 4];

        Vector2[] uvs = new Vector2[_vertices.Length];
        Vector4[] tangents = new Vector4[_vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int pol = 0, x = 0; x < widthX; x++)
        {
            for (int y = 0; y < widthY; y++, pol++)
            {
                quads[pol] = new Quad();

                _vertices[pol * 4] =
                    new Vector3(x / 2f,
                    CurveOfWave.Evaluate(Vector2.Distance(centerPoint, new Vector2(x, y)) - bias) * (40 - Vector2.Distance(centerPoint, new Vector2(x, y))) / 20f,
                    y / 2f);
                _vertices[pol * 4 + 1] =
                    new Vector3((x + 1) / 2f,
                    CurveOfWave.Evaluate(Vector2.Distance(centerPoint, new Vector2(x + 1, y)) - bias) * (40 - Vector2.Distance(centerPoint, new Vector2(x + 1, y))) / 20f,
                    y / 2f);
                _vertices[pol * 4 + 2] =
                    new Vector3(x / 2f,
                    CurveOfWave.Evaluate(Vector2.Distance(centerPoint, new Vector2(x, y + 1)) - bias) * (40 - Vector2.Distance(centerPoint, new Vector2(x, y + 1))) / 20f,
                    (y + 1) / 2f);
                _vertices[pol * 4 + 3] =
                    new Vector3((x + 1) / 2f,
                    CurveOfWave.Evaluate(Vector2.Distance(centerPoint, new Vector2(x + 1, y + 1)) - bias) * (40 - Vector2.Distance(centerPoint, new Vector2(x + 1, y + 1))) / 20f,
                    (y + 1) / 2f);

                //                quads[pol].coord = new Vector3(0f, 0f, 0f);

                tangents[0] = tangent;
            }
        }

        _mesh.vertices = _vertices;
        _mesh.uv = uvs;
        _mesh.tangents = tangents;

        int[] triangles = new int[6 * quads.Length];

        for (int pol = 0; pol < quads.Length; pol++)
        {
            triangles[pol * 6 + 0] = pol * 4 + 0;
            triangles[pol * 6 + 1] = pol * 4 + 2;
            triangles[pol * 6 + 2] = pol * 4 + 1;

            triangles[pol * 6 + 3] = pol * 4 + 1;
            triangles[pol * 6 + 4] = pol * 4 + 2;
            triangles[pol * 6 + 5] = pol * 4 + 3;
        }

        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        return;
        if (_vertices == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(_vertices[i], 0.2f);
        }
    }
}
