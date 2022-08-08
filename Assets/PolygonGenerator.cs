using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PolygonGenerator : MonoBehaviour
{
    private Vector3[] _vertices;
    private Mesh _mesh;

    public int xlen;
    public int zlen;

    private Polygon[] polygons;

    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        polygons = new Polygon[xlen * zlen];
        for (int pol = 0, x = 0; x < xlen; x++)
        {
            for (int z = 0; z < zlen; z++, pol++)
            {
                polygons[pol] = new Polygon();
                polygons[pol].coord = new Vector3(z * 1.5f, polygons[pol].yCor, 
                    Polygon._bias * 2 * x - (z % 2 > 0 ? 0f : Polygon._bias));
            }
        }

        _vertices = new Vector3[7 * polygons.Length];

        Vector2[] uvs = new Vector2[_vertices.Length];
        Vector4[] tangents = new Vector4[_vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int pol = 0; pol < polygons.Length; pol++)
        {
            _vertices[pol * 7 + 0] = polygons[pol].coord;
            _vertices[pol * 7 + 1] = polygons[pol].coord + Polygon.p1;
            _vertices[pol * 7 + 2] = polygons[pol].coord + Polygon.p2;
            _vertices[pol * 7 + 3] = polygons[pol].coord + Polygon.p3;
            _vertices[pol * 7 + 4] = polygons[pol].coord + Polygon.p4;
            _vertices[pol * 7 + 5] = polygons[pol].coord + Polygon.p5;
            _vertices[pol * 7 + 6] = polygons[pol].coord + Polygon.p6;
            tangents[0] = tangent;
        }

        _mesh.vertices = _vertices;
        _mesh.uv = uvs;
        _mesh.tangents = tangents;

        int[] triangles = new int[18 * polygons.Length];

        for (int pol = 0; pol < polygons.Length; pol++)
        {
            triangles[pol * 18 + 0] = pol * 7 + 0;
            triangles[pol * 18 + 1] = pol * 7 + 1;
            triangles[pol * 18 + 2] = pol * 7 + 2;

            triangles[pol * 18 + 3] = pol * 7 + 0;
            triangles[pol * 18 + 4] = pol * 7 + 2;
            triangles[pol * 18 + 5] = pol * 7 + 3;

            triangles[pol * 18 + 6] = pol * 7 + 0;
            triangles[pol * 18 + 7] = pol * 7 + 3;
            triangles[pol * 18 + 8] = pol * 7 + 4;

            triangles[pol * 18 + 9] = pol * 7 + 0;
            triangles[pol * 18 + 10] = pol * 7 + 4;
            triangles[pol * 18 + 11] = pol * 7 + 5;

            triangles[pol * 18 + 12] = pol * 7 + 0;
            triangles[pol * 18 + 13] = pol * 7 + 5;
            triangles[pol * 18 + 14] = pol * 7 + 6;

            triangles[pol * 18 + 15] = pol * 7 + 0;
            triangles[pol * 18 + 16] = pol * 7 + 6;
            triangles[pol * 18 + 17] = pol * 7 + 1;
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

public class Polygon
{
    public static float _bias => 0.866f;

    public Vector3 coord;
    public float xCor;
    public float zCor;
    public float yCor;// = Random.Range(0, 0.2f);

    public static Vector3 center => new Vector3(0f, 0f, 0f);
    public static Vector3 p1 => new Vector3(0.5f, 0f, -_bias);
    public static Vector3 p2 => new Vector3(-0.5f, 0f, -_bias);
    public static Vector3 p3 => new Vector3(-1f, 0f, 0);
    public static Vector3 p4 => new Vector3(-0.5f, 0f, _bias);
    public static Vector3 p5 => new Vector3(0.5f, 0f, _bias);
    public static Vector3 p6 => new Vector3(1f, 0f, 0f);
}