using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(LineRenderer))]
public class WireConnector : MonoBehaviour
{
    public enum Type
    {
        Plug,
        Outlet,
    }

    [SerializeField]
    LineRenderer lineRenderer;

    [SerializeField]
    GameObject plugGO;

    [SerializeField]
    GameObject outletGO;

    Type connectorType;

    /// <summary>
    /// Outlet to plug into if this is a plug
    /// </summary>
    WireConnector outlet;
    public WireConnector Outlet
    {
        get { return outlet; }
        set
        {
            if (connectorType == Type.Plug && value.connectorType == Type.Outlet)
                outlet = value;
        }
    }

    public Vector3 Destination { get; set; }
    public bool IsPlugged { get; set; } = true;

    MaterialPropertyBlock propBlock;

    private void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        if(lineRenderer != null)
        {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
        }
    }

    private void LateUpdate()
    {
        if (connectorType == Type.Outlet)
            return;

        if (IsPlugged)
            Destination = outlet.transform.position;
    }

    public void Build(Type _type, ColorName colorName)
    {
        propBlock = new MaterialPropertyBlock();

        connectorType = _type;
        plugGO.SetActive(_type == Type.Plug);
        outletGO.SetActive(_type == Type.Outlet);

        var connector = plugGO.activeSelf ? plugGO : outletGO;
        var renderer = connector.GetComponent<Renderer>();

        renderer.GetPropertyBlock(propBlock);
        var color = GameManager.instance.ColorNameMapping[colorName];

        propBlock.SetColor("_Color", color);
        renderer.SetPropertyBlock(propBlock);
    }
}
