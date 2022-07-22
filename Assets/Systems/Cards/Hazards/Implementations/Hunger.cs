using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : HazardCard
{
    public override bool CanPlay() => PlayerResources.Food > 0;
    public override void OnPlay() => PlayerResources.Food -= 1;
}
