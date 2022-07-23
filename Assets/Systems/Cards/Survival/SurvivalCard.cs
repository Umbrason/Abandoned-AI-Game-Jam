using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SurvivalCard : CardBase
{
    public override bool CanPlay()
    {
        return PlayerResources.Actions > 0;
    }
    protected Queue<object> selections;
    public IEnumerator MakeSelections()
    {
        GameplayManager.Instance.HideHand();
        selections = new Queue<object>();
        var Selectors = GetSelectors();
        while (selections.Count < Selectors.Length)
        {
            var selector = Selectors[selections.Count];
            object result = null;
            while (selector.MoveNext())
            {
                result = selector.Current;
                yield return null;
            }
            selections.Enqueue(result);
            yield return null;
        }
        PlayWithSelections(selections.ToArray());
        GameplayManager.Instance.ShowHand();
    }
    protected virtual IEnumerator[] GetSelectors() { return new IEnumerator[0]; }

    public override void OnPlay()
    {
        PlayerResources.Actions--;
        GameplayManager.Instance.StartCoroutine(MakeSelections());
    }
    protected abstract void PlayWithSelections(params object[] selections);
}
