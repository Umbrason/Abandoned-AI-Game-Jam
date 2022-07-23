using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunt : SurvivalCard
{
    const float HURT_CHANCE = 0.2f;

    protected override void PlayWithSelections(params object[] selections)
    {
        PlayerResources.Food += 2;
        if (Random.Range(0, 1f) < HURT_CHANCE && !GameplayManager.IsStructureBuild<HuntingCamp>())
            GameplayManager.Instance.SurvivalDeck.ShuffleNewCardsInto(Deck.Section.DrawPile, new Injury());
    }
    public override string Name => "Hunt";
    public override string Description => "+2 food\nHas a 20% chance to\nadd an injury to your deck";
}
