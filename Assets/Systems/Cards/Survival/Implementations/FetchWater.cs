using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchWater : SurvivalCard
{
    protected override void PlayWithSelections(params object[] selections)
    {
        PlayerResources.Water += Random.Range(1, 3);
    }
}
