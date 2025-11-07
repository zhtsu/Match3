using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UT_Widget : UT_UI
{
    [SerializeField]
    private UT_EScreenUIType _ParentType;
    public UT_EScreenUIType ParentType { get { return _ParentType; } }
}
