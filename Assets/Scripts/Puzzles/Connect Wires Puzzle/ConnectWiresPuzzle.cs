using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectWiresPuzzle : Puzzle
{
    [SerializeField]
    ConnectorWire wirePrefab;

    [SerializeField]
    WireConnector[] plugs;

    [SerializeField]
    WireConnector[] outlets;

    /// <summary>
    /// All the plugs available in the puzzle
    /// </summary>
    // List<WireConnector> plugs = new List<WireConnector>();

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
        if (plugs == null || plugs.Length < 1)
        {
            Debug.Log($"{name}: Plugs have not been assigned");
            return;
        }

        if (outlets == null || outlets.Length < 1)
        {
            Debug.Log($"{name}: Outlets have not been assigned");
            return;
        }

        if (plugs.Length != outlets.Length)
        {
            Debug.Log($"{name}: Plug and Outlet count does not match");
            return;
        }


        // Decide how many will be unplugged - minum is 1
        var maxPlugs = plugs.Length;
        var totalUnplugged = RandomNumbers.instance.Between(1, plugs.Length);

        // Choose random colors
        var colorNames = Utility.GetEnumValues<ColorName>();
        var shuffledColors = Utility.ShuffleArray(colorNames, RandomNumbers.Seed);
        var plugColors = shuffledColors.Take(maxPlugs).ToArray();

        // Updates the plugs with their color/state and pair it with an outlet
        // And assigning the destination outlet to the plugs so that it knows how to auto-connect
        // All are defaulted as connected
        var shuffledPlugs = Utility.ShuffleArray(plugs, RandomNumbers.Seed);
        var shuffledOutlets = Utility.ShuffleArray(outlets, RandomNumbers.Seed);

        for (int i = 0; i < plugColors.Count(); i++)
        {
            var colorName = plugColors[i];
            var color = GameManager.instance.ColorNameMapping[colorName];

            var outlet = shuffledOutlets[i];
            outlet.Build(colorName);

            var plug = shuffledPlugs[i];
            plug.Build(colorName);            
            plug.Outlet = outlet;            
            plug.Wire = InstantiateWire(colorName, color, shuffledPlugs[i]);

            // Has to be last as it require the plug to be built and know the outlet and wire
            plug.IsPlugged = true;
        }

        // Re-shuffle plugs to randomly choose ones to marked as unplugged
        shuffledPlugs = Utility.ShuffleArray(shuffledPlugs, RandomNumbers.Seed);
        for (int i = 0; i < totalUnplugged; i++)
            shuffledPlugs[i].IsPlugged = false;
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