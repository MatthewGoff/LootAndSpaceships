using System.Collections.Generic;
using UnityEngine;

public class SpaceshipPrefabs : MonoBehaviour
{
    public static SpaceshipPrefabs Instance;

    public GameObject[] PrefabsArray;

    public Dictionary<HullModel, GameObject> Prefabs;

    private void Awake()
    {
        Instance = this;

        Prefabs = new Dictionary<HullModel, GameObject>();
        foreach (GameObject prefab in PrefabsArray)
        {
            Prefabs.Add(Helpers.ParseHullModel(prefab.name), prefab);
        }
    }
}
