using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class SurvivalCard : CardBase
{
    protected Queue<object> selections;
    public IEnumerator MakeSelections()
    {
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
    }
    protected virtual IEnumerator[] GetSelectors() { return new IEnumerator[0]; }

    public override void OnPlay() => PlayWithSelections(selections);
    protected abstract void PlayWithSelections(params object[] selections);
}
