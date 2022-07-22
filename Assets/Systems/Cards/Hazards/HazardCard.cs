using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HazardCard : CardBase
{
    public override void OnEndTurn()
    {
        if (CanPlay())
            OnPlay();
        else
            GameplayManager.EndGame(false);
    }
}
