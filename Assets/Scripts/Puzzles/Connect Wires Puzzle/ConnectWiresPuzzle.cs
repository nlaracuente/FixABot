using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectWiresPuzzle : Puzzle
{
    [SerializeField]
    WireConnector connectorPrefab;

    [SerializeField]
    ConnectorWire wirePrefab;

    [SerializeField]
    Transform[] plugsXForms;

    [SerializeField]
    Transform[] outletsXForms;

    GameObject connectorsParent;

    /// <summary>
    /// All the plugs available in the puzzle
    /// </summary>
    List<WireConnector> plugs = new List<WireConnector>();

    /// <summary>
    /// Plug currently selected to connect to an outlet
    /// </summary>
    WireConnector selectedPlug;

    /// <summary>
    /// All plugs are connected
    /// </summary>
    public override bool IsSolved { get { return plugs.Where(p => !p.IsPlugged).FirstOrDefault() == null; } }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            BuildPuzzle();

        if (Input.GetKeyDown(KeyCode.S))
            Debug.Log($"Is {name} solved? {IsSolved}");
    }

    private void LateUpdate()
    {
        if (selectedPlug == null)
            return;

        selectedPlug.FollowMouse();
    }

    public override void BuildPuzzle() 
    {
        if (outletsXForms == null || plugsXForms.Length < 1)
        {
            Debug.Log($"{name}: Plugs have not been assigned");
            return;
        }

        if (outletsXForms == null || outletsXForms.Length < 1)
        {
            Debug.Log($"{name}: Outlets have not been assigned");
            return;
        }

        if (outletsXForms.Length != outletsXForms.Length)
        {
            Debug.Log($"{name}: Plug and Outlet count does not match");
            return;
        }

        InitializeConnectorGO();
        plugs.Clear();

        // Decide how many will be unplugged - minum is 1
        var maxPlugs = plugsXForms.Length;
        var totalUnplugged = RandomNumbers.instance.Between(1, outletsXForms.Length);

        // Choose random colors
        var colorNames = Utility.GetEnumValues<ColorName>();
        var shuffledColors = Utility.ShuffleArray(colorNames, RandomNumbers.Seed);
        var plugColors = shuffledColors.Take(maxPlugs).ToArray();

        // Create all the plugs and outlets positioning them at random starting positions
        // And assigning the destination outlet to the plugs so that it knows how to auto-connect
        // All are defaulted as connected
        var plugPositions = Utility.ShuffleArray(plugsXForms.Select(p => p.transform).ToArray(), RandomNumbers.Seed);
        var outletPositions = Utility.ShuffleArray(outletsXForms.Select(o => o.transform).ToArray(), RandomNumbers.Seed);
        for (int i = 0; i < plugColors.Count(); i++)
        {
            var colorName = plugColors[i];
            var gm = GameManager.instance;
            var color = gm.ColorNameMapping[colorName];

            var plug = InstantiateConnector(WireConnector.Type.Plug, colorName, plugPositions[i]);
            plug.Outlet = InstantiateConnector(WireConnector.Type.Outlet, colorName, outletPositions[i]);
            plug.Wire = InstantiateWire(colorName, color, plug);
            plug.IsPlugged = true;

            plugs.Add(plug);
        }

        // Randomly choose ones to marked as unplugged
        var shuffledPlugs = Utility.ShuffleArray(plugs.ToArray(), RandomNumbers.Seed);
        for (int i = 0; i < totalUnplugged; i++)
            shuffledPlugs[i].IsPlugged = false;
    }

    private void InitializeConnectorGO()
    {
        if (connectorsParent == null)
        {
            connectorsParent = new GameObject($"{name}_Parent_GO");
            connectorsParent.transform.SetParent(transform);
        }

        for (int i = 0; i < connectorsParent.transform.childCount; i++)
        {
            var child = connectorsParent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    private WireConnector InstantiateConnector(WireConnector.Type type, ColorName colorName, Transform xFrom)
    {
        var connector = Instantiate(connectorPrefab, connectorsParent.transform);
        connector.name = $"{type}_{colorName}";
        connector.transform.position = xFrom.position;
        // connector.transform.rotation = xFrom.rotation;

        connector.Build(type, colorName);
        return connector;
    }

    private ConnectorWire InstantiateWire(ColorName colorName, Color color, WireConnector plug)
    {
        var wire = Instantiate(wirePrefab, plug.transform);
        wire.name = $"{colorName}_Wire";
        wire.SetColor(color);
        return wire;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        // TODO: avoid getting it from the parent as it might change...
        var connector = eventData.pointerEnter.GetComponentInParent<WireConnector>();

        // Ignore plugged in ones
        if (connector == null || connector.IsPlugged)
            return;
        
        // A plug must always be selected first
        if(selectedPlug == null)
        {
            if(connector.ConnectorType == WireConnector.Type.Plug)
                selectedPlug = connector;
            return;
        }

        // Switching plugs
        if(connector.ConnectorType == WireConnector.Type.Plug)
        {
            selectedPlug.Disconnect();
            selectedPlug = connector;
            return;
        }

        // Wrong outlet - reset
        if (connector.ConnectorType == WireConnector.Type.Outlet && selectedPlug.ColorName != connector.ColorName)
        {
            selectedPlug.IsPlugged = false;
            selectedPlug = null;
            return;
        }

        // Correctly plugged
        if (connector.ConnectorType == WireConnector.Type.Outlet && selectedPlug.ColorName == connector.ColorName)
        {
            selectedPlug.IsPlugged = true;
            selectedPlug = null;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log($"Pointer Drag: {eventData.pointerDrag}, Pointer Enter: {eventData.pointerEnter}");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log($"Pointer Enter: {eventData.pointerEnter}");
    }
}