using UnityEngine;

[CreateAssetMenu(fileName ="Coin")]
public class CoinData : Pickupable {
    public int amount;

    public override void OnPickup()
    {
        // TODO do stuff
    }
}