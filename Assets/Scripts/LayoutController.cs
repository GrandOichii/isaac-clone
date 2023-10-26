using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutController : MonoBehaviour
{
    private Dictionary<RoomData, GameObject> _roomsMap = new();
    private RoomData _current;
    public void Load(RoomData rd) {
        _unload();
        _current = rd;
        // add to rooms map
        if (!_roomsMap.ContainsKey(rd)) {
            var go = Instantiate(rd.RoomTemplate);
            _roomsMap.Add(rd, go);
            go.transform.SetParent(transform);
            go.transform.localPosition = new(0, 0, go.transform.localPosition.z);
        }

        // set the room active
        foreach (var r in _roomsMap.Values) {
            r.SetActive(false);
        }
        var room = _roomsMap[rd];
        room.SetActive(true);
    
        // spawn pickupables
        foreach (Transform child in room.GetComponent<RoomInfo>().pickupableSpawnsContainer.transform) {
            child.GetComponent<LootSpawnController>().Activate();
        }
    }

    public void _unload() {
        if (_current is null) return;
        var room = _roomsMap[_current];
        foreach (Transform child in room.GetComponent<RoomInfo>().pickupableSpawnsContainer.transform) {
            child.GetComponent<LootSpawnController>().Deactivate();
        }
    }
}
