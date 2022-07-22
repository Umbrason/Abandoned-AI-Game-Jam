using System.Collections;
///<summary>used for building/upgrading structures</summary>
public class BuildStructureCard : SurvivalCard
{

    IEnumerator SelectBuilding()
    {
        StructureBase sBase = null;
        yield return sBase;
    }

    protected override IEnumerator[] GetSelectors() { return new IEnumerator[] { SelectBuilding() }; }
    protected override void PlayWithSelections(params object[] selections)
    {
        GameplayManager.Instance.Structures.Add((StructureBase)selections[0]);
    }
}
