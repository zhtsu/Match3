using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int Damage = 0;

    public void Initialize(int Row, int Column, float TileSize)
    {
        _Row = Row;
        _Column = Column;
        _TileSize = TileSize;
        _GridArray = new M3_GridCellContainer[_Row, _Column];
    }

    public IEnumerator SwapGem(int X1, int Y1, int X2, int Y2)
    {
        yield return SwapGemAnimCoroutine(X1, Y1, X2, Y2);

        if (IsCanMatch3FromCoords(X2, Y2))
        {
            Damage = 0;

            yield return ProcessMatch3Coroutine();

            M3_Event_Match3Damage Event = new M3_Event_Match3Damage(
                M3_GameController.Instance.CurrentBattleInputController,
                Damage
            );
            M3_EventBus.SendEvent<M3_Event_Match3Damage>(Event);
        }
        else
            yield return RevertGemAnimCoroutine(X1, Y1, X2, Y2);
    }

    public IEnumerator ProcessMatch3Coroutine()
    {
        bool IsChaining = false;

        do
        {
            IsChaining = false;

            if (IsCanMatch3(out HashSet<M3_GridCellContainer> CellsToDestroy1))
            {
                int TotalDamage = 3;
                int DamageDelta = CellsToDestroy1.Count - 3;
                for (int i = 0; i < DamageDelta; i++)
                    TotalDamage += (i + 1) * 2;

                Damage += TotalDamage;

                yield return StartCoroutine(DestroyGemCells(CellsToDestroy1));
                yield return StartCoroutine(ApplyGravityCoroutine());
                yield return StartCoroutine(FillEmptyCellsCoroutine());
            }

            if (IsCanMatch3(out HashSet<M3_GridCellContainer> CellsToDestroy2))
            {
                IsChaining = true;
            }
        }
        while (IsChaining);
    }

    private IEnumerator SwapGemAnimCoroutine(int X1, int Y1, int X2, int Y2)
    {
        M3_GridCellContainer ConA = _GridArray[X1, Y1];
        M3_GridCellContainer ConB = _GridArray[X2, Y2];

        M3_IGridCell CellA = ConA.GetGemCell();
        M3_IGridCell CellB = ConB.GetGemCell();

        Vector3 PosA = ConA.transform.position;
        Vector3 PosB = ConB.transform.position;

        Tween MoveA = Tween.Position(((MonoBehaviour)CellA).transform, PosB, 0.2f, Ease.InOutCubic);
        Tween MoveB = Tween.Position(((MonoBehaviour)CellB).transform, PosA, 0.2f, Ease.InOutCubic);
        Sequence Seq = Sequence.Create()
            .Group(MoveA)
            .Group(MoveB);

        yield return Seq.ToYieldInstruction();

        ConA.AddCell(CellB, M3_FillMode.AspectFit, false);
        ConB.AddCell(CellA, M3_FillMode.AspectFit, false);

        M3_Gem GemA = CellA as M3_Gem;
        if (GemA != null)
        {
            GemA.RecoverOrder();
            yield return GemA.RecoverScale();
        }
    }

    private IEnumerator RevertGemAnimCoroutine(int X1, int Y1, int X2, int Y2)
    {
        M3_GridCellContainer ConA = _GridArray[X1, Y1];
        M3_GridCellContainer ConB = _GridArray[X2, Y2];

        M3_IGridCell CellA = ConA.GetGemCell();
        M3_IGridCell CellB = ConB.GetGemCell();

        Vector3 PosA = ConA.transform.position;
        Vector3 PosB = ConB.transform.position;

        Tween MoveA = Tween.Position(((MonoBehaviour)CellA).transform, PosB, 0.2f, Ease.InOutCubic);
        Tween MoveB = Tween.Position(((MonoBehaviour)CellB).transform, PosA, 0.2f, Ease.InOutCubic);
        Sequence Seq = Sequence.Create()
            .Group(MoveA)
            .Group(MoveB);

        yield return Seq.ToYieldInstruction();

        ConA.AddCell(CellB, M3_FillMode.AspectFit, false);
        ConB.AddCell(CellA, M3_FillMode.AspectFit, false);
    }

    public void SwapGemByAI()
    {
        List<M3_DragGemData> SwappableList = new List<M3_DragGemData>();
        for (int i = 0; i < _Row; i++)
        {
            for (int j = 0; j < _Column; j++)
            {
                List<M3_MouseMoveDirection> DirList = GetSwappableDirectionList(i, j);
                foreach (M3_MouseMoveDirection Dir in DirList)
                {
                    SwappableList.Add(new M3_DragGemData(_GridArray[i, j], Dir));
                }
            }
        }

        if (SwappableList.Count == 0)
            return;

        int RandomIndex = UnityEngine.Random.Range(0, SwappableList.Count);
        M3_DragGemData RDGD = SwappableList[RandomIndex];

        M3_ManagerHub.Instance.CommandManager.PushCommand(
            new M3_Command_DragGem(RDGD.DraggedContainer, RDGD.DragDirection));
    }

    List<M3_MouseMoveDirection> GetSwappableDirectionList(int X, int Y)
    {
        List<M3_MouseMoveDirection> Result = new List<M3_MouseMoveDirection>();

        foreach (M3_MouseMoveDirection Dir in System.Enum.GetValues(typeof(M3_MouseMoveDirection)))
        {
            if (Dir == M3_MouseMoveDirection.None)
                continue;

            M3_GridCellContainer Cont = _GridArray[X, Y];
            if (IsCanSwap(Cont, Dir))
                Result.Add(Dir);
        }

        return Result;
    }

    public bool IsCanSwap(M3_GridCellContainer Cont, M3_MouseMoveDirection Dir)
    {
        if (Cont == null)
            return false;

        int TargetX = Cont.GridCoords.x;
        int TargetY = Cont.GridCoords.y;
        switch (Dir)
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

        M3_GridCellContainer ContB = null;
        if (TargetX >= 0 && TargetX < _Row && TargetY >= 0 && TargetY < _Column)
            ContB = _GridArray[TargetX, TargetY];
        if (ContB == null)
            return false;

        M3_IGridCell ContC = Cont.GetGemCell();
        Cont.SetGemCell(ContB.GetGemCell());
        ContB.SetGemCell(ContC);

        bool Result = false;
        if (IsCanMatch3FromCoords(TargetX, TargetY))
            Result = true;
        else
            Result = false;

        M3_IGridCell ContD = Cont.GetGemCell();
        Cont.SetGemCell(ContB.GetGemCell());
        ContB.SetGemCell(ContD);

        return Result;
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
                GridContainer.AddCell(GridCell, FillMode, true);
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
                GCC.ParentGrid = this;
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

    private bool IsCanMatch3FromCoords(int X, int Y)
    {
        List<M3_GridCellContainer> RightMatchs = GetMatchs(X, Y, 1, 0);
        List<M3_GridCellContainer> UpMatchs = GetMatchs(X, Y, 0, 1);
        List<M3_GridCellContainer> LeftMatchs = GetMatchs(X, Y, -1, 0);
        List<M3_GridCellContainer> DownMatchs = GetMatchs(X, Y, 0, -1);

        HashSet<M3_GridCellContainer> HMatchs = new HashSet<M3_GridCellContainer>();
        foreach (M3_GridCellContainer Cont in RightMatchs)
            HMatchs.Add(Cont);
        foreach (M3_GridCellContainer Cont in LeftMatchs)
            HMatchs.Add(Cont);

        HashSet<M3_GridCellContainer> VMatchs = new HashSet<M3_GridCellContainer>();
        foreach (M3_GridCellContainer Cont in UpMatchs)
            VMatchs.Add(Cont);
        foreach (M3_GridCellContainer Cont in DownMatchs)
            VMatchs.Add(Cont);

        if (HMatchs.Count >= 3 || VMatchs.Count >= 3)
            return true;

        return false;
    }

    private bool IsCanMatch3(out HashSet<M3_GridCellContainer> OutCellsToDestroy)
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

        OutCellsToDestroy = CellsToDestroy;
        return CellsToDestroy.Count > 0;
    }

    private IEnumerator DestroyGemCells(HashSet<M3_GridCellContainer> NeedToDestroy)
    {
        List<Tween> Tweens1 = new List<Tween>();
        foreach (M3_GridCellContainer Cont in NeedToDestroy)
        {
            if (M3_GameController.Instance.CurrentBattleInputController == M3_ControllerType.None)
                continue;

            SpriteRenderer TileSprite = ((M3_Tile)Cont.GetTileCell()).transform.GetComponent<SpriteRenderer>();
            ((M3_Gem)Cont.GetGemCell()).SetOrderToTop();

            Color Col = M3_CommonHelper.GetCommonColor(M3_ColorType.White);
            if (M3_GameController.Instance.CurrentBattleInputController == M3_ControllerType.AI)
                Col = M3_CommonHelper.GetCommonColor(M3_ColorType.Red);

            Tween Tw = Tween.Color(TileSprite, Col, 0.1f, Ease.InOutCubic);
            Tweens1.Add(Tw);
        }

        Sequence Seq1 = Sequence.Create();
        foreach (Tween Tw in Tweens1)
            Seq1.Chain(Tw);

        yield return Seq1.ToYieldInstruction();

        List<Sequence> Seqs = new List<Sequence>();
        foreach (M3_GridCellContainer Cont in NeedToDestroy)
        {
            if (M3_GameController.Instance.CurrentBattleInputController == M3_ControllerType.None)
            {
                Tween Tw1 = Tween.Scale(((MonoBehaviour)Cont.GetGemCell()).transform, 1.2f, 0.5f, Ease.InQuad);
                Tween Tw2 = Tween.Scale(((MonoBehaviour)Cont.GetGemCell()).transform, 0.0f, 0.25f, Ease.InQuad);

                Seqs.Add(Tw1.Chain(Tw2));
            }
            else
            {
                Vector3 TargetPos = Vector3.zero;
                if (M3_GameController.Instance.CurrentBattleInputController == M3_ControllerType.AI)
                    TargetPos = new Vector3(-8.5f, -3f, 0);
                else
                    TargetPos = new Vector3(8.5f, -3f, 0);

                Tween Tw1 = Tween.Scale(((MonoBehaviour)Cont.GetGemCell()).transform, 2f, 0.25f, Ease.OutQuad);
                Tween Tw2 = Tween.Position(((MonoBehaviour)Cont.GetGemCell()).transform, TargetPos, 0.1f, Ease.InQuad);
                Tween Tw3 = Tween.Scale(((MonoBehaviour)Cont.GetGemCell()).transform, 1f, 0.1f, Ease.InQuad);

                Seqs.Add(Tw1.Chain(Tw2).Group(Tw3));
            }
        }

        Sequence Seq2 = Sequence.Create();

        if (M3_GameController.Instance.CurrentBattleInputController == M3_ControllerType.None)
        {
            foreach (Sequence Seq in Seqs)
                Seq2.Group(Seq);
        }
        else
        {
            foreach (Sequence Seq in Seqs)
                Seq2.Chain(Seq);
        }

        yield return Seq2.ToYieldInstruction();

        foreach (M3_GridCellContainer Cont in NeedToDestroy)
        {
            Cont.ResetTileColor();
            Cont.RemoveGemCell();
        }
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

        return Matches;
    }

    public IEnumerator ApplyGravityCoroutine()
    {
        List<Tween> Tweens = new List<Tween>();
        for (int i = 0; i < _Column; i++)
        {
            int EmptyCount = 0;
            for (int j = 0; j < _Row; j++)
            {
                if (_GridArray[i, j].IsEmpty())
                {
                    EmptyCount++;
                }
                else if (EmptyCount > 0)
                {
                    M3_IGridCell GemCell = _GridArray[i, j].GetGemCell();
                    _GridArray[i, j].ClearGemCell();

                    int TargetI = i;
                    int TargetJ = j - EmptyCount;
                    Vector3 TargetPos = _GridArray[TargetI, TargetJ].transform.position;

                    Tween Tw = Tween.Position(((MonoBehaviour)GemCell).transform, TargetPos, 0.2f, Ease.InOutCubic);
                    Tw.OnComplete(() =>
                    {
                        _GridArray[TargetI, TargetJ].AddCell(GemCell, M3_FillMode.AspectFit, false);
                    });

                    Tweens.Add(Tw);
                }
            }
        }

        Sequence Seq = Sequence.Create();
        foreach (Tween Tw in Tweens)
            Seq.Group(Tw);

        if (Tweens.Count > 0)
            yield return Seq.ToYieldInstruction();
    }

    public IEnumerator FillEmptyCellsCoroutine(float AnimDuration = 0.2f)
    {
        List<Tween> Tweens = new List<Tween>();
        for (int i = 0; i < _Row; i++)
        {
            for (int j = 0; j < _Column; j++)
            {
                if (_GridArray[i, j].IsEmpty())
                {
                    M3_UnitData GemData = M3_CommonHelper.GetRandomGemData();
                    M3_IGridCell NewCell = M3_CommonHelper.SpawnGem(GemData.BelongingModId, GemData.Id);
                    _GridArray[i, j].AddCell(NewCell, M3_FillMode.AspectFit, false);

                    const float FallOffset = 100f;
                    Vector3 StartPosition = new Vector3(0f, FallOffset, 0f);

                    Transform CellTransform = ((MonoBehaviour)NewCell).transform;
                    CellTransform.localPosition = StartPosition;

                    Tween Tw = Tween.LocalPosition(CellTransform, Vector3.zero, AnimDuration, Ease.OutQuad);
                    Tweens.Add(Tw);
                }
            }
        }

        Sequence Seq = Sequence.Create();
        foreach (Tween Tw in Tweens)
            Seq.Group(Tw);

        if (Tweens.Count > 0)
            yield return Seq.ToYieldInstruction();
    }
}
