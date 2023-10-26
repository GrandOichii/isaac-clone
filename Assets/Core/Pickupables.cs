using UnityEngine;

public class Pickupable : ScriptableObject {
    public Sprite sprite;

    public virtual void OnPickup() {
    }
}