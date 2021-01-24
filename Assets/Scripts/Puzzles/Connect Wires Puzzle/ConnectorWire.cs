using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ConnectorWire : MonoBehaviour
{
    LineRenderer lineRenderer;
    LineRenderer WireLineRenderer
    {
        get
        {
            if(lineRenderer == null)
            {
                lineRenderer = GetComponent<LineRenderer>();
                lineRenderer.positionCount = 0;
            }

            return lineRenderer;
        }
    }

    public void SetColor(Color color)
    {
        WireLineRenderer.startColor = color;
        WireLineRenderer.endColor = color;
    }

    public void Clear()
    {        
        WireLineRenderer.positionCount = 0;
        WireLineRenderer.enabled = false;
    }

    public void SetPositions(Vector3[] positions)
    {
        WireLineRenderer.enabled = true;
        WireLineRenderer.positionCount = positions.Length;
        WireLineRenderer.SetPositions(positions);
    }
}
