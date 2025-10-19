using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_UIComponent : MonoBehaviour
{
    [SerializeField]
    private M3_UIType _ParentType;

    public M3_UIType ParentType { get { return _ParentType; } }
}
