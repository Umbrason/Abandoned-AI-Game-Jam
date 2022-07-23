using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forage : SurvivalCard
{
    protected override void PlayWithSelections(params object[] selections)
    {
        PlayerResources.Food += 1;
    }

    public override string Name => "Forage";
    public override string Description => "+1 Food";
}
