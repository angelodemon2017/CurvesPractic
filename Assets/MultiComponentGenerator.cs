using UnityEngine;

public class MultiComponentGenerator : MonoBehaviour
{
    private Vector3[] _vertices;
    private Mesh _mesh;

    public int quadsPerimetr;
    public int hightCyl;
    public int periodicHole = 2;

    public AnimationCurve RadiusDepencHight;

    private Quad[] quads;

    void Start()
    {
        Generate();
    }

    private void Update()
    {
    //    Generate();
    }

    private void Generate()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        quads = new Quad[quadsPerimetr * (hightCyl - 1)];
        _vertices = new Vector3[quads.Length * 4];
        Vector2[] uvs = new Vector2[_vertices.Length];
        Vector4[] tangents = new Vector4[_vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int pol = 0, y_h = 0; y_h < hightCyl - 1; y_h++)
        {
            for (int pCirc = 0; pCirc < quadsPerimetr; pCirc++, pol++)
            {
                if (y_h % periodicHole == 0 && pCirc % periodicHole == 0) 
                {
                    continue;
                }

                float p0 = 2f * Mathf.PI / quadsPerimetr * pCirc;
                float p1 = 2f * Mathf.PI / quadsPerimetr * (pCirc + 1);

                float cosP0 = Mathf.Cos(p0);
                float sinP0 = Mathf.Sin(p0);
                float cosP1 = Mathf.Cos(p1);
                float sinP1 = Mathf.Sin(p1);

                float r0 = RadiusDepencHight.Evaluate((float)y_h / (float)hightCyl);
                float r1 = RadiusDepencHight.Evaluate(((float)y_h + 1f) / (float)hightCyl);

                quads[pol] = new Quad()
                {
                    _x0y0 = new Vector3()
                    {
                        x = r0 * cosP0,
                        y = y_h / 2f,
                        z = r0 * sinP0
                    },
                    _x1y0 = new Vector3()
                    {
                        x = r0 * cosP1,
                        y = y_h / 2f,
                        z = r0 * sinP1
                    },
                    _x0y1 = new Vector3()
                    {
                        x = r1 * cosP0,
                        y = (y_h + 1) / 2f,
                        z = r1 * sinP0
                    },
                    _x1y1 = new Vector3()
                    {
                        x = r1 * cosP1,
                        y = (y_h + 1) / 2f,
                        z = r1 * sinP1
                    }
                };
                _vertices[pol * 4] = quads[pol]._x0y0;
                _vertices[pol * 4 + 1] = quads[pol]._x1y0;
                _vertices[pol * 4 + 2] = quads[pol]._x0y1;
                _vertices[pol * 4 + 3] = quads[pol]._x1y1;

                quads[pol].coord = new Vector3(0f, 0f, 0f);

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
