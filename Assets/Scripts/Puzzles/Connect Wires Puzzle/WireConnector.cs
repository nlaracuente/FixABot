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
    GameObject plugGO;

    [SerializeField]
    GameObject outletGO;

    [SerializeField, Tooltip("Positions to use when plug is connected. Should be in the order to connect to")]
    Transform[] plugWirePositions;
    public Transform[] PlugWirePositions { get { return plugWirePositions; } }

    [SerializeField, Tooltip("Positions to use when outlet is connected. Should be in the order to connect to")]
    Transform[] outletWirePositions;
    public Transform[] OutletWirePositions { get { return outletWirePositions; } }

    public Type ConnectorType { get; private set; }
    public ColorName ColorName { get; private set; }

    /// <summary>
    /// Outlet to plug into if this is a plug
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

    public void Build(Type type, ColorName colorName)
    {
        propBlock = new MaterialPropertyBlock();

        ConnectorType = type;
        ColorName = colorName;
        plugGO.SetActive(type == Type.Plug);
        outletGO.SetActive(type == Type.Outlet);

        var connector = plugGO.activeSelf ? plugGO : outletGO;
        var renderer = connector.GetComponent<Renderer>();

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
        var startPosition = plugWirePositions[0];
        var endPos = GameManager.instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        endPos.z = startPosition.position.z;

        var pos = new List<Vector3>(plugWirePositions.Select(p => p.position));
        pos.Add(endPos);
        Wire.SetPositions(pos.ToArray());
    }

    /// <summary>
    /// Updates line renderer to connect to outlet
    /// </summary>
    void PlugIn()
    {
        var positions = PlugWirePositions.Union(Outlet.PlugWirePositions).ToArray();
        Wire.SetPositions(positions.Select(p => p.position).ToArray());
    }

    public void Disconnect()
    {
        Wire.Clear();
    }
}
