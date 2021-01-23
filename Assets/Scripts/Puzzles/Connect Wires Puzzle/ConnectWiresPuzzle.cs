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
    Transform[] plugsXForms;

    [SerializeField]
    Transform[] outletsXForms;   

    /// <summary>
    /// All the plugs available in the puzzle
    /// </summary>
    List<WireConnector> plugs = new List<WireConnector>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            BuildPuzzle();
    }

    public override void BuildPuzzle()
    {
        // Clear any already spawned
        // Mainly for testing purposes 
        foreach (var plug in plugs)
        {
            if(plug.Outlet != null)
                Destroy(plug.Outlet.gameObject);

            if(plug.gameObject != null)
                Destroy(plug.gameObject);
        }
        plugs.Clear();

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

        if(outletsXForms.Length != outletsXForms.Length)
        {
            Debug.Log($"{name}: Plug and Outlet count does not match");
            return;
        }

        // Decide how many will be unplugged - minum is 1
        var maxPlugs = plugsXForms.Length;
        var totalUnplugged = RandomNumbers.instance.Between(1, outletsXForms.Length);

        // Choose random colors
        var colorNames = Utility.GetEnumValues<ColorName>();
        var shuffledColors = Utility.ShuffleArray(colorNames, RandomNumbers.Seed);
        var plugColors = shuffledColors.Take(maxPlugs).ToArray();

        // Create all the plugs and outlets positioning them at random starting positions
        // And assigning the destination outlet to the plugs so that it knows how to auto-connect
        var plugPositions = Utility.ShuffleArray(plugsXForms.Select(p => p.transform).ToArray(), RandomNumbers.Seed);
        var outletPositions = Utility.ShuffleArray(outletsXForms.Select(o => o.transform).ToArray(), RandomNumbers.Seed);
        for (int i = 0; i < plugColors.Count(); i++)
        {
            var colorName = plugColors[i];
            var gm = GameManager.instance;
            var color = gm.ColorNameMapping[colorName];

            var plug = InstantiateConnector(WireConnector.Type.Plug, colorName, plugPositions[i]);
            plug.Outlet = InstantiateConnector(WireConnector.Type.Outlet, colorName, outletPositions[i]);
            plugs.Add(plug);
        }

        // Randomly choose ones to marked as unplugged
        var shuffledPlugs = Utility.ShuffleArray(plugs.ToArray(), RandomNumbers.Seed);
        for (int i = 0; i < totalUnplugged; i++)
            shuffledPlugs[i].IsPlugged = false;
    }

    private WireConnector InstantiateConnector(WireConnector.Type type, ColorName colorName, Transform xFrom)
    {
        var connector = Instantiate(connectorPrefab, transform);
        connector.name = $"{type}_{colorName}";
        connector.transform.position = xFrom.position;
        connector.transform.rotation = xFrom.rotation;

        connector.Build(type, colorName);
        return connector;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}