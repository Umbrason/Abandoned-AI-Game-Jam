using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Injury : SurvivalCard
{
    public override bool CanPlay() => base.CanPlay() && GameplayManager.IsStructureBuild<FirstAidStation>() && FirstAidStation.uses > 0;
    protected override void PlayWithSelections(params object[] selections)
    {
        GameplayManager.Instance.SurvivalDeck.RemoveCardsFromDeck(this);
        GameplayManager.Instance.SurvivalDeck.DrawCards(2);
        PlayerResources.Actions += 2;
        FirstAidStation.uses--;
    }

    public override string Name => "Injury";
    public override string Description => "Can only be played if you built a first aid station. \n+2 Card \n+2 Action \nRemove this card from your deck";
}
