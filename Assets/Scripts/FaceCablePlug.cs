using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class FaceCablePlug : MonoBehaviour
{
    [SerializeField]
    FacePlate.Expression expression;
    public FacePlate.Expression Expression { get { return expression; } }

    public bool IsBroken { get { return FacePlate.IsExpressionBroken(Expression); } }

    [SerializeField]
    LineRenderer lineRenderer;

    [SerializeField]
    Transform startPosition;

    [SerializeField]
    Transform endPosition;

    FacePlate facePlate;
    FacePlate FacePlate
    {
        get
        {
            if(facePlate == null)
                facePlate = FindObjectOfType<FacePlate>();

            return facePlate;
        }
    }

    private void Awake()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        facePlate = FindObjectOfType<FacePlate>();
    }

    private void Start()
    {
        if (Application.isPlaying && IsBroken)
            lineRenderer.positionCount = 0;
        else
            ConnectCables();
    }

    void Update()
    {
        // Update the lines in the editor to see how they will look when not broken
        if (!Application.isPlaying)
        {
            ConnectCables();
        }
        else
        {
            if (IsBroken)
                lineRenderer.positionCount = 0;
            else
                ConnectCables();
        }   
    }

    private void ConnectCables()
    {
        var pos = new Vector3[] { startPosition.position, endPosition.position };
        lineRenderer.positionCount = pos.Count();
        lineRenderer.SetPositions(pos);
    }

    public void FollowMouse()
    {
        var endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPos.z = startPosition.position.z;

        var pos = new Vector3[] { startPosition.position, endPos };
        lineRenderer.positionCount = pos.Count();
        lineRenderer.SetPositions(pos);
    }
}
