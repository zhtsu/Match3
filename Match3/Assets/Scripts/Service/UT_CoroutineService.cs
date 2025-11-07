using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UT_CoroutineService : MonoBehaviour, UT_ICoroutineSercive
{
    public void RunCoroutine(IEnumerator InCoroutine)
    {
        StartCoroutine(InCoroutine);
    }
}
