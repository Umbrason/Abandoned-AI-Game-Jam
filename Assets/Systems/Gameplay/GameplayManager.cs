using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    #region Singleton
    public static GameplayManager Instance;
    private void OnEnable() => enabled = ((Instance ??= this) == this);
    private void OnDisable() => Instance = ((this == Instance) ? null : Instance);
    #endregion

    private readonly Deck survivalDeck = new Deck();
    private readonly Deck hazardsDeck = new Deck();
    private readonly List<StructureBase> structures = new List<StructureBase>();

    [SerializeField] private DeckVisualization hazardDeckVisualizer;
    [SerializeField] private DeckVisualization survivalDeckVisualizer;
    [SerializeField] private HandVisualization hazardHandVisualizer;
    [SerializeField] private HandVisualization survivalHandVisualizer;

    [SerializeField] private StructureWorldRepresentation fireplaceRep;
    [SerializeField] private StructureWorldRepresentation waterFilterRep;
    [SerializeField] private StructureWorldRepresentation firstAidTableRep;
    [SerializeField] private StructureWorldRepresentation huntingCampRep;
    [SerializeField] private StructureWorldRepresentation shelterRep;

    [SerializeField] private GameObject EndCanvas;
    [SerializeField] private TextMeshProUGUI EndText;
    [SerializeField] private TextMeshProUGUI EndTextSub;

    public Deck SurvivalDeck { get => survivalDeck; }
    public Deck HazardsDeck { get => hazardsDeck; }
    public List<StructureBase> Structures { get => structures; }

    private IEnumerator activeTurnEnumerator;

    public void ShowHand()
    {
        survivalHandVisualizer.gameObject.SetActive(true);
    }

    public void HideHand()
    {
        survivalHandVisualizer.gameObject.SetActive(false);
        survivalHandVisualizer.SetDeck(survivalDeck);
    }

    public static bool IsStructureBuild<T>() where T : StructureBase
    {
        return Instance.Structures.Any(s => s.GetType() == typeof(T));
    }

    public static void BuildStructure(StructureBase structureBase)
    {
        StructureManager.ShowStructure(structureBase);
        Instance.Structures.Add(structureBase);
        PlayerResources.Materials -= structureBase.BuildCosts();
    }

    public static void EndGame(bool success)
    {
        switch (success)
        {
            case true:
                //Victory
                Instance.EndText.SetText("You Win!");
                Instance.EndTextSub.SetText("All hazards have been avoided and rescue will come soon...");
                Instance.EndCanvas.SetActive(true);
                break;
            case false:
                //Gameover
                Instance.EndText.SetText("You Lose!");
                Instance.EndTextSub.SetText("You didn't manage to survive long enough for help to arrive...");
                Instance.EndCanvas.SetActive(true);
                break;
        }
    }

    void Start()
    {
        //3x Fetchwater, 2x Forage, 1x Hunt, 2x ScavangeMaterials, 1x Injury, 1x buildStructure = 10 cards
        survivalDeck.DrawPile = new SurvivalCard[] {            
            new FetchWater(),
            new FetchWater(),
            new FetchWater(),            
            new Forage(),
            new Forage(),            
            new Hunt(),            
            new ScavangeMaterials(),
            new ScavangeMaterials(),            
            new Injury(),            
            new BuildStructureCard(),
        }.OrderBy(card => Random.Range(0, 1f)).ToArray();

        //maybe split into phases and randomize those?
        hazardsDeck.DrawPile = new HazardCard[] {
            new Thirst(),

            new Thirst(),
            new Thirst(),            

            new Hunger(),
            new AnimalAttack(),
            new Thirst(),

            new Thirst(),
            new Thirst(),
            new AnimalAttack(),

            new Thirst(),
            new AnimalAttack(),
            new AnimalAttack(),

            new Hunger(),
            new Hunger(),
            new Thirst(),

            new TropicalRain(),
            new AnimalAttack(),
            new Thirst(),

            new Hunger(),
            new Hunger(),
            new Hunger()            
        };

        StructureManager.worldRepresentations.Add(new Fireplace(), fireplaceRep);
        StructureManager.worldRepresentations.Add(new FirstAidStation(), firstAidTableRep);
        StructureManager.worldRepresentations.Add(new HuntingCamp(), huntingCampRep);
        StructureManager.worldRepresentations.Add(new Shelter(), shelterRep);
        StructureManager.worldRepresentations.Add(new WaterFilter(), waterFilterRep);


        hazardDeckVisualizer?.SetDeck(hazardsDeck);
        hazardHandVisualizer?.SetDeck(hazardsDeck);
        survivalDeckVisualizer?.SetDeck(survivalDeck);
        survivalHandVisualizer?.SetDeck(survivalDeck);
        //first part of the turn
        activeTurnEnumerator = TurnEnumerator();
        activeTurnEnumerator.MoveNext();
    }

    public void EndTurn()
    {
        Instance.activeTurnEnumerator.MoveNext(); //initiate end of turn
    }

    private IEnumerator TurnEnumerator()
    {
        int hazardCount = 0;
        while (true)
        {
            //start Turn
            //refresh hand
            if (hazardsDeck.DrawPile.Length == 0)
                EndGame(true);
            survivalDeck.DrawCards(5, true);
            PlayerResources.Actions = 3;
            //draw hazards
            hazardsDeck.DrawCards(++hazardCount);
            Structures.ForEach(s => s.OnBeginTurn());
            hazardCount = Mathf.Min(hazardCount, 3);
            yield return null;

            //end Turn
            //process negatives of hazards
            foreach (var Hazard in hazardsDeck.Hand)
                hazardsDeck.PlayCard(Hazard);
            if (hazardsDeck.Hand.Length > 0)
                GameplayManager.EndGame(false);
            survivalDeck.DiscardEntireHand();
            hazardsDeck.DiscardEntireHand();
        }
    }
}