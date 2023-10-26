using System;
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

        // TODO spawn according to loot table
        if (_instance is null) {
            _instance = Instantiate(pickupableHolderTemplate);
            _instance.transform.SetParent(transform);
            _instance.transform.localPosition = new(0, 0, _instance.transform.localPosition.z);
            _instance.GetComponent<PickupableController>().Item = _randomItem();
        }

        _instance.SetActive(true);
    }

    public void Deactivate() {
        if (_looted || _instance is null) return;

        _instance.SetActive(false);
    }

    private Pickupable _randomItem() {
        return lootTable.probabilityMap[0].item;
    }

    public void Loot() {
        _looted = true;
        _instance.SetActive(false);
    }
}
