using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFilter : StructureBase
{
    public override int BuildCosts() => 1;
    public override void OnBeginTurn() => PlayerResources.Water += 2;
}
