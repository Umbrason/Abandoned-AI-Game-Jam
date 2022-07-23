using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardBase
{
    public virtual bool CanPlay() => true;
    public abstract void OnPlay();
    public virtual void OnDraw() { }
    public virtual void OnEndTurn() { }

    public abstract string Name { get; }
    public abstract string Description { get; }
}
