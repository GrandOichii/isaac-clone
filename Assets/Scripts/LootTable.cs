using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Drop {
    public Pickupable item;
    public int chance;
}

[CreateAssetMenu(fileName = "Loot table")]
public class LootTable : ScriptableObject
{
    public float chanceForNothing;
    public List<Drop> probabilityMap;
}
