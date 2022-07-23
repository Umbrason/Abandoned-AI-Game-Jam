using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlayerResources
{
    private static readonly Dictionary<string, int> resourceDict = new Dictionary<string, int>() {
        {MATERIALS_KEY, 0},
        {FOOD_KEY, 0},
        {WATER_KEY, 0},
        {ACTIONS_KEY, 0},
    };

    public static string[] Keys { get => resourceDict.Keys.ToArray(); }
    private const string MATERIALS_KEY = "Materials";
    private const string FOOD_KEY = "Food";
    private const string WATER_KEY = "Water";
    private const string ACTIONS_KEY = "Actions";

    public static event Action<string, int> OnResourceChanged;

    public static bool IsRealKey(string key)
    {
        return resourceDict.ContainsKey(key);
    }

    public static int GetResourceAmount(string key)
    {
        return resourceDict.ContainsKey(key) ? resourceDict[key] : -1;
    }
    public static void SetResourceAmount(string key, int amount)
    {
        if (!resourceDict.ContainsKey(key))
            return;
        resourceDict[key] = amount;
        OnResourceChanged?.Invoke(key, amount);
    }

    public static int Actions
    {
        get { return resourceDict[ACTIONS_KEY]; }
        set { SetResourceAmount(ACTIONS_KEY, value); }
    }
    public static int Materials
    {
        get { return resourceDict[MATERIALS_KEY]; }
        set { SetResourceAmount(MATERIALS_KEY, value); }
    }
    public static int Food
    {
        get { return resourceDict[FOOD_KEY]; }
        set { SetResourceAmount(FOOD_KEY, value); }
    }
    public static int Water
    {
        get { return resourceDict[WATER_KEY]; }
        set { SetResourceAmount(WATER_KEY, value); }
    }
}
