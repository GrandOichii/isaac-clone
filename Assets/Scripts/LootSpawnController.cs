using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawnController : MonoBehaviour
{
    public GameObject pickupableHolderTemplate;
    private GameObject _instance;
    public LootTable lootTable;

    private bool _looted = false;

    public void Activate() {
        if (_looted) return;

        // spawn according to loot table
        if (_instance is null) {
            _instance = Instantiate(pickupableHolderTemplate);
            _instance.transform.SetParent(transform);
            _instance.transform.localPosition = new(0, 0, _instance.transform.localPosition.z);
            var item = _randomItem();
            if (item == null) {
                _looted = true;
                return;
            }
            _instance.GetComponent<PickupableController>().Item = item;
        }

        _instance.SetActive(true);
    }

    public void Deactivate() {
        if (_looted || _instance is null) return;

        _instance.SetActive(false);
    }

    private Pickupable? _randomItem() {
        var count = lootTable.chanceForNothing;
        foreach (var pair in lootTable.probabilityMap)
            count += pair.chance;
        var v = Random.Range(0, count + 1);
        if (v <= lootTable.chanceForNothing) {
            return null;
        }
        v -= lootTable.chanceForNothing;
        foreach (var pair in lootTable.probabilityMap) {
            if (v > pair.chance) {
                v -= pair.chance;
                continue;
            }

            return pair.item;
        }

        // TODO should happen, just in case
        return null;
    }

    public void Loot() {
        _looted = true;
        _instance.SetActive(false);
    }
}
