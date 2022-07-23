using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StructureManager
{
    public static readonly Dictionary<StructureBase, StructureWorldRepresentation> worldRepresentations = new Dictionary<StructureBase, StructureWorldRepresentation>();

    public static StructureBase GetStructure(StructureWorldRepresentation worldRepresentation)
    {
        return worldRepresentations.SingleOrDefault(x => x.Value == worldRepresentation).Key;
    }

    public static bool CanBuildAny() => worldRepresentations.Any(structurePair => !structurePair.Key.IsBuilt && structurePair.Key.BuildCosts() <= PlayerResources.Materials);


    public static void EnterPreviewMode()
    {
        foreach (var structurePair in worldRepresentations)
        {
            if (!structurePair.Key.IsBuilt && structurePair.Key.BuildCosts() <= PlayerResources.Materials)
                structurePair.Value.EnablePreview();
        }
    }

    public static void ExitPreviewMode()
    {
        foreach (var worldRepresentation in worldRepresentations)
        {
            worldRepresentation.Value.DisablePreview();
        }
    }

    public static void ShowStructure(StructureBase structure)
    {
        worldRepresentations[structure].Show();
    }

}
