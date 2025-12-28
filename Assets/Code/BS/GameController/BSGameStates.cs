using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingBattleShip : IBSGameState
{
    public void EnterState()
    {

    }
}

public interface IBSGameState
{
    void EnterState();
}