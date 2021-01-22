using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class FaceCablePlug : MonoBehaviour
{
    [SerializeField]
    FacePlate.Expression expression;

    [SerializeField]
    LineRenderer lineRenderer;

    [SerializeField]
    List<Transform> linePositions;

    FacePlate facePlate;

    private void Awake()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        facePlate = FindObjectOfType<FacePlate>();
    }

    void Update()
    {
        if(Application.isPlaying && facePlate.IsExpressionBroken(expression))
            lineRenderer.positionCount = 0;
        else
        {
            if (linePositions == null || linePositions.Count < 1)
                return;

            lineRenderer.positionCount = linePositions.Count;
            lineRenderer.SetPositions(linePositions.Select(p => p.position).ToArray());
        }
    }
}
