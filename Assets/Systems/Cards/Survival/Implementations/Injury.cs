using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Injury : SurvivalCard
{
    public override bool CanPlay() => GameplayManager.IsStructureBuild<FirstAidStation>() && FirstAidStation.uses > 0;
    protected override void PlayWithSelections(params object[] selections)
    {
        GameplayManager.Instance.SurvivalDeck.RemoveCardsFromDeck(this);
        GameplayManager.Instance.SurvivalDeck.DrawCards(1);
        FirstAidStation.uses--;
    }
}
