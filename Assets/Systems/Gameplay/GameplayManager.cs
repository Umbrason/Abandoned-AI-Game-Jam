using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    #region Singleton
    public static GameplayManager Instance;
    private void OnEnable() => enabled = ((Instance ??= this) == this);
    private void OnDisable() => Instance = ((this == Instance) ? null : Instance);
    #endregion


    private readonly Deck<SurvivalCard> survivalDeck = new Deck<SurvivalCard>();
    private readonly Deck<HazardCard> hazardsDeck = new Deck<HazardCard>();
    private readonly List<StructureBase> structures = new List<StructureBase>();

    public Deck<SurvivalCard> SurvivalDeck { get => survivalDeck; }
    public Deck<HazardCard> HazardsDeck { get => hazardsDeck; }
    public List<StructureBase> Structures { get => structures; }

    public static bool IsStructureBuild<T>() where T : StructureBase
    {
        return Instance.Structures.Any(s => s.GetType() == typeof(T));
    }

    public static void EndGame(bool success)
    {
        switch (success)
        {
            case true:
                //Victory

                break;
            case false:
                //Gameover

                break;
        }
    }

    void Start()
    {
        //first part of the turn
        TurnEnumerator().MoveNext();
    }

    private IEnumerator TurnEnumerator()
    {
        //start Turn
        //refresh hand
        survivalDeck.DiscardCards(survivalDeck.Hand);
        survivalDeck.DrawCards(5);
        //draw hazards
        hazardsDeck.DrawCards(3);
        yield return null;

        //end Turn
        //process negatives of hazards
        hazardsDeck.DiscardEntireHand();
    }

}