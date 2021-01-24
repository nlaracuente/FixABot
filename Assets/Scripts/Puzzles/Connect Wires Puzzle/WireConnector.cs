using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WireConnector : MonoBehaviour
{
    public enum Type
    {
        Plug,
        Outlet,
    }

    [SerializeField]
    Type type;
    public Type ConnectorType { get { return type; } }
    public ColorName ColorName { get; private set; }

    [SerializeField, Tooltip("Positions to use when plug is connected. Should be in the order to connect to")]
    Transform[] wirePositions;
    public Transform[] WirePositions { get { return wirePositions; } }
           

    /// <summary>
    /// If this is a plug, then this is the outlet it connects toi
    /// </summary>
    WireConnector outlet;
    public WireConnector Outlet
    {
        get { return outlet; }
        set
        {
            if (ConnectorType == Type.Plug && value.ConnectorType == Type.Outlet)
                outlet = value;
        }
    }

    /// <summary>
    /// The wire contains the "line renderer" that draws the wire connection
    /// </summary>
    ConnectorWire wire;
    public ConnectorWire Wire
    {
        get { return wire; }
        set
        {
            if (ConnectorType == Type.Plug)
                wire = value;
        }
    }

    bool isPlugged;
    public bool IsPlugged 
    {
        get { return isPlugged; }
        set {
            isPlugged = value;
            if (isPlugged)
                PlugIn();
            else
                Disconnect();
        } 
    }

    MaterialPropertyBlock propBlock;

    private void LateUpdate()
    {
        if (ConnectorType == Type.Outlet)
            return;
    }

    public void Build(ColorName colorName)
    {
        ColorName = colorName;
        propBlock = new MaterialPropertyBlock();        

        var renderer = GetComponent<Renderer>();
        renderer.GetPropertyBlock(propBlock);
        var color = GameManager.instance.ColorNameMapping[colorName];

        propBlock.SetColor("_Color", color);
        renderer.SetPropertyBlock(propBlock);
    }

    /// <summary>
    /// Connect on the plug side of things but reach to the mouse's position
    /// </summary>
    public void FollowMouse()
    {
        var endPos = transform.position;

        var camera = GameManager.instance.MainCamera;
        if(camera.orthographic)
            endPos = GameManager.instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        else
        {
            Plane plan = new Plane(Vector3.right, 0);
            float distance;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (plan.Raycast(ray, out distance))
                endPos = ray.GetPoint(distance);
        }

        endPos.z = transform.position.z;

        var pos = new List<Vector3>(wirePositions.Select(p => p.position));
        pos.Add(endPos);
        Wire.SetPositions(pos.ToArray());
    }

    /// <summary>
    /// Updates line renderer to connect to outlet
    /// </summary>
    void PlugIn()
    {
        if (Outlet == null || Wire == null)
            return;

        var pPos = WirePositions;
        var oPos = Outlet.WirePositions;

        var positions = WirePositions.Union(Outlet.WirePositions).ToArray();
        Wire.SetPositions(positions.Select(p => p.position).ToArray());
    }

    public void Disconnect()
    {
        Wire.Clear();
    }
}
