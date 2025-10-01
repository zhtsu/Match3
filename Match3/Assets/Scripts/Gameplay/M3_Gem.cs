using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_Gem : MonoBehaviour, M3_IGridCell
{
    public Vector2Int CellCoords { get; set; }

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void OnClick()
    {
        Debug.Log("Gem Clicked");
    }

    public void BeginDrag()
    {
        Debug.Log("Gem Clicked");
    }

    public void EndDrag()
    {
        Debug.Log("Gem Clicked");
    }
}
