using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureBase
{
    public StructureBase() { }
    private bool isBuilt;
    private bool isDamaged;
    private int upgradeStage;

    public bool IsBuilt { get => isBuilt; }
    public bool IsDamaged { get => isDamaged; }
    public int UpgradeStage { get => upgradeStage; }

    public abstract int BuildCosts();
    public virtual void OnBuild() { }
    public virtual void OnEndTurn() { }
    public virtual void OnBeginTurn() { }

}

