using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    private static Dictionary<string, int> resourceDict;
    private const string MATERIALS_KEY = "materials";
    private const string FOOD_KEY = "food";
    private const string WATER_KEY = "water";


    public static int Materials
    {
        get { return resourceDict[MATERIALS_KEY]; }
        set { resourceDict[MATERIALS_KEY] = value; }
    }
    public static int Food
    {
        get { return resourceDict[FOOD_KEY]; }
        set { resourceDict[FOOD_KEY] = value; }
    }
    public static int Water
    {
        get { return resourceDict[WATER_KEY]; }
        set { resourceDict[WATER_KEY] = value; }
    }
}
