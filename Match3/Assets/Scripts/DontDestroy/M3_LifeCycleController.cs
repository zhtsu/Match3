using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_LifeCycleController : MonoBehaviour
{
    [SerializeField]
    private GameObject _UIRootPrefab;

    [SerializeField]
    private GameObject _TestGrid;

    private void Start()
    {
        DontDestroyOnLoad(this);

        if (_UIRootPrefab)
        {
            Instantiate(_UIRootPrefab);
        }

        M3_EventManager.Subscribe<M3_Event_PrefabsLoadCompleted>(TestAnythingHere);

        M3_ManagerHub.Instance.Initialize();
    }

    private void OnDestroy()
    {
        M3_ManagerHub.Instance.Destroy();
    }

    private void TestAnythingHere(M3_Event_PrefabsLoadCompleted Event)
    {
        if (_TestGrid)
        {
            M3_Grid Grid = _TestGrid.GetComponent<M3_Grid>();

            Grid.Initialize(10, 10, 2.0f);
            Grid.GenetateTerrain();
        }
    }
}
