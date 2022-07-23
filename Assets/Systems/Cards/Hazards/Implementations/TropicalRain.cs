public class TropicalRain : HazardCard
{
    public override void OnPlay()
    {
        PlayerResources.Water += 2;
        if (GameplayManager.IsStructureBuild<Fireplace>())
            return;
        PlayerResources.Food = 0;        
    }

    public override string Name => "Tropical Storm";
    public override string Description => "+2 Water\n Loose all food unless you built a fireplace";
}
