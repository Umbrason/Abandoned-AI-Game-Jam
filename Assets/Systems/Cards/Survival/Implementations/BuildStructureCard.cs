using System.Collections;
using UnityEngine;
///<summary>used for building/upgrading structures</summary>
public class BuildStructureCard : SurvivalCard
{
    public override bool CanPlay() => base.CanPlay() && StructureManager.CanBuildAny();
    
    IEnumerator SelectBuilding()
    {
        StructureManager.EnterPreviewMode();
        StructureBase sBase = null;
        while (true)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Camera.main.farClipPlane, LayerMask.GetMask("Default")) && Input.GetMouseButtonDown(0))
            {                
                var structureRep = hit.collider.GetComponentInParent<StructureWorldRepresentation>();
                if (structureRep)
                {
                    sBase = StructureManager.GetStructure(structureRep);
                    break;
                }
            }
            yield return null;
        }
        StructureManager.ExitPreviewMode();
        yield return sBase;
    }

    protected override IEnumerator[] GetSelectors() { return new IEnumerator[] { SelectBuilding() }; }
    protected override void PlayWithSelections(params object[] selections)
    {
        var structure = selections[0] as StructureBase;
        GameplayManager.BuildStructure(structure);
    }

    public override string Name => "Build Structure";
    public override string Description => "Use scavanged materials to build a structure.\nStructures help you survive.";
}
