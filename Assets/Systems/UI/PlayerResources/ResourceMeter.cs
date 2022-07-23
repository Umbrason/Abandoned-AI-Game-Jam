using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceMeter : MonoBehaviour
{
    private string resourceKey;

    public string ResourceKey { get => resourceKey; set => SetResourceKey(value); }

    void OnEnable() => PlayerResources.OnResourceChanged += OnResourceChanged;
    void OnDisable() => PlayerResources.OnResourceChanged -= OnResourceChanged;

    public TextMeshProUGUI text;

    private void SetResourceKey(string resourceKey)
    {
        if (!PlayerResources.IsRealKey(resourceKey))
            return;
        this.resourceKey = resourceKey;
        OnResourceChanged(resourceKey, PlayerResources.GetResourceAmount(resourceKey));
    }

    private void OnResourceChanged(string resource, int amount)
    {
        if (resource != resourceKey)
            return;
        text?.SetText($"{resourceKey}: {amount}");
    }


}
