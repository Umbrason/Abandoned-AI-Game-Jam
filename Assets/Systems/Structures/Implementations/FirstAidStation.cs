using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidStation : StructureBase
{
    public static int uses = 1;
    public override int BuildCosts() => 2;
    public override void OnBeginTurn() => uses = 1;
}
