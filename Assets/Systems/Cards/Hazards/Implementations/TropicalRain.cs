public class TropicalRain : HazardCard
{
    public override void OnPlay()
    {
        PlayerResources.Water += 2;
        if (GameplayManager.IsStructureBuild<Fireplace>())
            return;
        PlayerResources.Food = 0;        
    }
}
