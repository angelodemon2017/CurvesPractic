using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendControlCurves : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int samples = 50;
    public AnimationCurve lineToDraw;
    public float width = 1f;
    public float height = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        lineRenderer.positionCount = samples;

        Vector3[] positions = new Vector3[samples];

        for (int i = 0; i < samples; i++) 
        {
            float time = (1f / samples) * i;
            float value = lineToDraw.Evaluate(time);

            positions[i] = new Vector3(time* width, value * height);
        }
        lineRenderer.SetPositions(positions);
    }
}
