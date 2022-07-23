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
        GameplayManager.Instance.SurvivalDeck.ShuffleNewCardsInto(Deck.Section.Hand, new Injury());
    }

    public override string Name => "Wild Animals";
    public override string Description => "adds an injury to your deck unless you built a Shelter";
}
