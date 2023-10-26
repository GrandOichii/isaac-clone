using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupableController : MonoBehaviour
{
    private SpriteRenderer _sprite;

    private Pickupable _item;
    public Pickupable Item {
        get => _item;
        set {
            _item = value;
            GetComponent<SpriteRenderer>().sprite = value.sprite;
        }
    }

    void Start() {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void Pickup(PlayerMovement player) {
        // TODO fix? should even happen, the object is not active
        if (Item == null) return;

        Item.OnPickup(player);
        transform.parent.GetComponent<LootSpawnController>().Loot();
    }
}
