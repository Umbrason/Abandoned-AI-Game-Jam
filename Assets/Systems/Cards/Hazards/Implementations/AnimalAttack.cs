using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalAttack : HazardCard
{
    public override void OnPlay()
    {
        if (GameplayManager.IsStructureBuild<Shelter>())
            return;
        GameplayManager.Instance.SurvivalDeck.ShuffleCardsInto(Deck<SurvivalCard>.Section.Hand, new Injury());
    }
}
