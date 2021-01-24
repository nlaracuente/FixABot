using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// There's a row that contains a sequence of color the player has to match with the button nearests
/// to it
/// </summary>
public class PatternMatchPuzzle : Puzzle
{
    [SerializeField]
    Renderer[] colorPlates;

    [SerializeField]
    Renderer[] buttonColorPlates;

    MaterialPropertyBlock propBlock;

    /// <summary>
    /// Keeps track of the current color index the button has to make it easier to cycle through
    /// </summary>
    Dictionary<Renderer, int> colorIndexes;

    /// <summary>
    /// Stores the position of the render so that it matches the color plates positions
    /// </summary>
    Dictionary<Renderer, int> buttonIndexes;

    /// <summary>
    /// Keeps track of colors so that we can cycle through them
    /// </summary>
    Color[] colors;

    List<Color> plateColors;
    List<Color> buttonColors;

    bool isSolved;
    public override bool IsSolved
    {
        get
        {
            return isSolved;
        }
    }

    public override void BuildPuzzle()
    {
        propBlock = new MaterialPropertyBlock();
        colorIndexes = new Dictionary<Renderer, int>();
        buttonIndexes = new Dictionary<Renderer, int>();
        colors = GameManager.instance.ColorNameMapping.Values.ToArray();

        plateColors = new List<Color>();
        buttonColors = new List<Color>();

        var shuffledColors = GameManager.instance.GetShuffledColorNames();

        for (int i = 0; i < colorPlates.Length; i++)
        {
            // Set random colors for plates
            var color = GameManager.instance.ColorNameMapping[shuffledColors[i]];
            SetRendererColor(colorPlates[i], color);
            plateColors.Add(color);

            // Default buttons to black
            color = Color.black;
            SetRendererColor(buttonColorPlates[i], color);
            buttonIndexes.Add(buttonColorPlates[i], i);
            buttonColors.Add(color);
        }
    }

    void SetRendererColor(Renderer renderer, Color color)
    {
        renderer.GetPropertyBlock(propBlock);
        propBlock.SetColor("_Color", color);
        renderer.SetPropertyBlock(propBlock);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        var renderer = eventData.pointerEnter.GetComponent<Renderer>();
        if (renderer == null)
            Debug.Log($"{eventData.pointerEnter} does not have a renderer");

        // First time clicking on this button
        if (!colorIndexes.ContainsKey(renderer))
            colorIndexes.Add(renderer, -1);            

        var index = colorIndexes[renderer] + 1;
        if (index > colors.Length - 1)
            index = 0;

        var color = colors[index];
        SetRendererColor(renderer, color);
        colorIndexes[renderer] = index;

        var btnIndex = buttonIndexes[renderer];
        buttonColors[btnIndex] = color;
        isSolved = buttonColors.SequenceEqual(plateColors);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
