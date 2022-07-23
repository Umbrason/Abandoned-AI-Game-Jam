using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesVisualizer : MonoBehaviour
{
    private readonly Queue<ResourceMeter> resourceMeters = new Queue<ResourceMeter>();
    [SerializeField] private GameObject template;
    private void OnEnable()
    {
        foreach (var key in PlayerResources.Keys)
            resourceMeters.Enqueue(SpawnResourceMeter(key));
    }

    private void OnDisable()
    {
        while (resourceMeters.Count > 0)
            Destroy(resourceMeters.Dequeue());
    }

    private ResourceMeter SpawnResourceMeter(string resourceKey)
    {
        var instance = Instantiate(template, transform);
        var meter = instance.GetComponent<ResourceMeter>();
        meter.ResourceKey = resourceKey;
        return meter;
    }
}
