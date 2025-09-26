using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_Grid : MonoBehaviour
{
    [SerializeField]
    private int _Row = 0;
    [SerializeField]
    private int _Column = 0;
    [SerializeField]
    private float _TileSize = 0;
    
    private M3_Tile[,] _GridArray;

    private void Awake()
    {
        M3_EventManager.Subscribe<M3_Event_PrefabsLoadCompleted>(OnPrefabsLoadCompleted);
    }

    public void Initialize(int Row, int Column, float TileSize)
    {
        _Row = Row;
        _Column = Column;
        _TileSize = TileSize;
        _GridArray = new M3_Tile[_Row, _Column];
    }

    public void Generate()
    {
        for (int i = 0; i < _Row; i++)
        {
            for (int j = 0; j < _Column; j++)
            {
                Vector3 Position = new Vector3(i, j) * _TileSize;
                GameObject Tile = Instantiate(M3_PrefabManager.GetPrefab(FB_PrefabType.SquareTile), Position, Quaternion.identity);
                Tile.transform.SetParent(this.transform);
                _GridArray[i, j] = Tile.GetComponent<M3_Tile>();
            }
        }
    }

    private void OnPrefabsLoadCompleted(M3_Event_PrefabsLoadCompleted Event)
    {
        Initialize(10, 10, 0.2f);
        Generate();

        M3_EventManager.Unsubscribe<M3_Event_PrefabsLoadCompleted>(OnPrefabsLoadCompleted);
    }
}
