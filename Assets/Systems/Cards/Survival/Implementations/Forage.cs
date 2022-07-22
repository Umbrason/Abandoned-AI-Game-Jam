using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forage : SurvivalCard
{
    protected override void PlayWithSelections(params object[] selections)
    {
        PlayerResources.Food += Random.Range(0, 3);
    }
}
