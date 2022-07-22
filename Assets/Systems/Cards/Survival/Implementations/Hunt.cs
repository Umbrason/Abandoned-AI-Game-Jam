using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunt : SurvivalCard
{
    const float HURT_CHANCE = 0.2f;

    protected override void PlayWithSelections(params object[] selections)
    {
        PlayerResources.Food += Random.Range(2, 5);
        if (Random.Range(0, 1f) < HURT_CHANCE)
            GameplayManager.Instance.SurvivalDeck.ShuffleCardsInto(Deck<SurvivalCard>.Section.DrawPile, new Injury());
    }
}
