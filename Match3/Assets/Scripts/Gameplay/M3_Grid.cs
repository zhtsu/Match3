using PrimeTween;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class M3_Grid : MonoBehaviour
{
    [SerializeField]
    private GameObject _GridCellContainerPrefab;

    private int _Row = 0;
    public int Row { get { return _Row; } }

    private int _Column = 0;
    public int Column { get { return _Column; } }

    private float _TileSize = 0;
    public float TileSize { get { return _TileSize; } }

    private M3_GridCellContainer[,] _GridArray;
    private Sequence _SwapSequence;

    public void Initialize(int Row, int Column, float TileSize)
    {
        _Row = Row;
        _Column = Column;
        _TileSize = TileSize;
        _GridArray = new M3_GridCellContainer[_Row, _Column];
    }

    public void SwapGemCell(int X1, int Y1, int X2, int Y2)
    {
        M3_GridCellContainer ConA = _GridArray[X1, Y1];
        M3_GridCellContainer ConB = _GridArray[X2, Y2];

        M3_IGridCell GemA = ConA.GetGemCell();
        M3_IGridCell GemB = ConB.GetGemCell();

        Vector3 PosA = ConA.transform.position;
        Vector3 PosB = ConB.transform.position;

        _SwapSequence.Stop();
        Tween MoveA = Tween.Position(((MonoBehaviour)GemA).transform, PosB, 0.2f, Ease.InOutCubic);
        Tween MoveB = Tween.Position(((MonoBehaviour)GemB).transform, PosA, 0.2f, Ease.InOutCubic);
        _SwapSequence = Sequence.Create()
            .Group(MoveA)
            .Group(MoveB)
            .ChainCallback(() =>
            {
                OnSwapCompleted(ConA, ConB, GemA, GemB);
            });
    }

    public void OnSwapCompleted(M3_GridCellContainer ConA, M3_GridCellContainer ConB, M3_IGridCell CellA, M3_IGridCell CellB)
    {
        ConA.AddCell(CellB, M3_FillMode.AspectFit, false);
        ConB.AddCell(CellA, M3_FillMode.AspectFit, false);

        M3_Gem GemA = CellA as M3_Gem;
        if (GemA != null)
        {
            GemA.RecoverOrder();
            GemA.RecoverScale();
        }

        CheckMatch3IfCan();
    }

    public void AddCell(M3_IGridCell GridCell, int X, int Y, M3_FillMode FillMode = M3_FillMode.None, bool WithAnim = false)
    {
        if (X < _Row && Y < _Column)
        {
            M3_GridCellContainer GridContainer = _GridArray[X, Y];
            if (GridContainer != null)
            {
                GridContainer.ParentGrid = this;
                GridContainer.GridCoords = new Vector2Int(X, Y);
                GridContainer.AddCell(GridCell, FillMode, true, WithAnim);
            }
        }
    }

    public M3_GridCellContainer GetContainer(int CenterX, int CenterY, M3_MouseMoveDirection Direction)
    {
        int TargetX = CenterX;
        int TargetY = CenterY;
        switch (Direction)
        {
            case M3_MouseMoveDirection.Up:
                TargetY += 1;
                break;
            case M3_MouseMoveDirection.Down:
                TargetY -= 1;
                break;
            case M3_MouseMoveDirection.Left:
                TargetX -= 1;
                break;
            case M3_MouseMoveDirection.Right:
                TargetX += 1;
                break;
        }

        if (TargetX >= 0 && TargetX < _Row && TargetY >= 0 && TargetY < _Column)
        {
            return _GridArray[TargetX, TargetY];
        }

        return null;
    }

    private void AddTile(M3_IGridCell GridCell, int X, int Y)
    {
        if (X >= 0 && X < _Row && Y >= 0 && Y < _Column)
        {
            if (GridCell != null)
            {
                M3_GridCellContainer GridContainer = _GridArray[X, Y];
                if (GridContainer != null)
                {
                    GridContainer.AddTile(GridCell, M3_FillMode.ScaleFill);
                    GridCell.ParentContainer = _GridArray[X, Y];
                }
            }
        }
    }

    public M3_IGridCell GetGemCell(int X, int Y)
    {
        if (X < _Row && Y < _Column)
        {
            M3_GridCellContainer GridContainer = _GridArray[X, Y];
            if (GridContainer != null)
                return GridContainer.GetGemCell();
        }

        return null;
    }

    public void GenerateGrid()
    {
        ClearGrid();

        Vector3 Offset = new Vector3(-_Row * _TileSize * 0.5f, -_Column * _TileSize * 0.5f, 0);
        Offset += new Vector3(_TileSize * 0.5f, _TileSize * 0.5f, 0);
        for (int i = 0; i < _Row; i++)
        {
            for (int j = 0; j < _Column; j++)
            {
                Vector3 Position = new Vector3(i, j) * _TileSize + Offset;
                GameObject CellContainer = Instantiate(_GridCellContainerPrefab, Position, Quaternion.identity);
                CellContainer.transform.SetParent(this.transform);
                M3_GridCellContainer GCC = CellContainer.GetComponent<M3_GridCellContainer>();
                GCC.Initialize(_TileSize, _TileSize);
                GCC.GridCoords = new Vector2Int(i, j);
                _GridArray[i, j] = GCC;
            }
        }

        for (int i = 0; i < _Row; i++)
        {
            for (int j = 0; j < _Column; j++)
            {
                M3_GridCellContainer GCC = _GridArray[i, j];
                AddTile(M3_CommonHelper.SpawnTile(), i, j);
            }
        }
    }

    public void ClearGrid()
    {
        while (transform.childCount > 0)
        {
            Transform Child = transform.GetChild(0);
#if UNITY_EDITOR
            DestroyImmediate(Child.gameObject);
#else
            Destroy(Child.gameObject);
#endif
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 Offset = new Vector3(-_Row * _TileSize * 0.5f, -_Column * _TileSize * 0.5f, 0);
        for (int i = 0; i <= _Row; i++)
        {
            Vector3 StartPos = transform.position + new Vector3(i * _TileSize, 0, 0) + Offset;
            Vector3 EndPos = transform.position + new Vector3(i * _TileSize, _Column * _TileSize, 0) + Offset;
            Gizmos.DrawLine(StartPos, EndPos);
        }
        for (int j = 0; j <= _Column; j++)
        {
            Vector3 StartPos = transform.position + new Vector3(0, j * _TileSize, 0) + Offset;
            Vector3 EndPos = transform.position + new Vector3(_Row * _TileSize, j * _TileSize, 0) + Offset;
            Gizmos.DrawLine(StartPos, EndPos);
        }
    }

    private void CheckMatch3IfCan()
    {
        HashSet<M3_GridCellContainer> CellsToDestroy = new HashSet<M3_GridCellContainer>();

        for (int i = 0; i < _Row; i++)
        {
            for (int j = 0; j < _Column; j++)
            {
                M3_GridCellContainer CurrentCont = _GridArray[i, j];

                List<M3_GridCellContainer> HorizontalMatchs = GetMatchs(i, j, 1, 0);
                if (HorizontalMatchs.Count >= 3)
                {
                    foreach (var Cont in HorizontalMatchs)
                    {
                        CellsToDestroy.Add(Cont);
                    }
                }

                List<M3_GridCellContainer> VerticalMatchs = GetMatchs(i, j, 0, 1);
                if (VerticalMatchs.Count >= 3)
                {
                    foreach (var Cont in VerticalMatchs)
                    {
                        CellsToDestroy.Add(Cont);
                    }
                }
            }
        }

        List<Tween> Tweens = new List<Tween>();
        foreach (M3_GridCellContainer Cont in CellsToDestroy)
        {
            SpriteRenderer TileSprite = ((M3_Tile)Cont.GetTileCell()).transform.GetComponent<SpriteRenderer>();

            Tween Tw = Tween.Color(TileSprite, M3_CommonHelper.GetCommonColor(M3_ColorType.White), 0.1f, Ease.InOutCubic);
            Tweens.Add(Tw);
        }

        if (CellsToDestroy.Count > 0)
            M3_GameController.Instance.SetAllowInput(false);
        else
            M3_GameController.Instance.SetAllowInput(true);

        _SwapSequence.Stop();
        _SwapSequence = Sequence.Create();
        foreach (Tween Tw in Tweens)
        {
            _SwapSequence.Chain(Tw);
        }
        _SwapSequence.ChainCallback(() =>
        {
            foreach (M3_GridCellContainer Cont in CellsToDestroy)
            {
                Cont.RemoveGemCell();
            }
        });
    }

    private List<M3_GridCellContainer> GetMatchs(int StartX, int StartY, int DirX, int DirY)
    {
        M3_GridCellContainer StartCont = _GridArray[StartX, StartY];
        if (StartCont == null)
            return new List<M3_GridCellContainer>();

        M3_Gem Gem = StartCont.GetGemCell() as M3_Gem;
        if (Gem == null)
            return new List<M3_GridCellContainer>();

        Hash128 GemId = Gem.UnitId;

        List<M3_GridCellContainer> Matches = new List<M3_GridCellContainer> { StartCont };

        int X = StartX + DirX;
        int Y = StartY + DirY;

        while (X >= 0 && X < _Row && Y >= 0 && Y < _Column)
        {
            M3_GridCellContainer NextCont = _GridArray[X, Y];
            M3_Gem NextGem = NextCont.GetGemCell() as M3_Gem;
            Hash128 NextGemId;
            if (NextGem != null)
                NextGemId = NextGem.UnitId;
            else
                break;

            if (NextGemId != GemId)
            {
                break;
            }

            Matches.Add(NextCont);

            X += DirX;
            Y += DirY;
        }

        return Matches.Count >= 3 ? Matches : new List<M3_GridCellContainer>();
    }
}
