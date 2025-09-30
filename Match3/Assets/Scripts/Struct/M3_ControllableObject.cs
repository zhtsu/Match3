using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct M3_ControllableObject
{
    private M3_Character _OwnedCharacter;
    private M3_Controller _OwnedController;

    M3_ControllableObject(M3_Character InCharacter, M3_Controller InController)
    {
        _OwnedCharacter = InCharacter;
        _OwnedController = InController;
    }
}
