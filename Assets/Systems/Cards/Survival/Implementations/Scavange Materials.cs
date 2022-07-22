using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavangeMaterials : SurvivalCard
{
    protected override void PlayWithSelections(params object[] selections)
    {
        PlayerResources.Materials += Random.Range(1, 4);
    }
}
