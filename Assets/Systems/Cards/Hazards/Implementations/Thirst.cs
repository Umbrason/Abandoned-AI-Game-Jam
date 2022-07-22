using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thirst : HazardCard
{
    public override bool CanPlay() => PlayerResources.Water > 0;
    public override void OnPlay() => PlayerResources.Water -= 1;
}
