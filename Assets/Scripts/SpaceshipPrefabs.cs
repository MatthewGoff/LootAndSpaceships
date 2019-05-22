using System.Collections.Generic;
using UnityEngine;

public class SpaceshipPrefabs : MonoBehaviour
{
    public static SpaceshipPrefabs Instance;

    public GameObject[] PrefabsArray;

    public Dictionary<SpaceshipModel, GameObject> Prefabs;

    private void Awake()
    {
        Instance = this;

        Prefabs = new Dictionary<SpaceshipModel, GameObject>();
        foreach (GameObject prefab in PrefabsArray)
        {
            Prefabs.Add(Helpers.ParseSpaceshipModel(prefab.name), prefab);
        }
    }
}
