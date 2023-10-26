using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutController : MonoBehaviour
{
    private Dictionary<RoomData, GameObject> _roomsMap = new();
    public void Load(RoomData rd) {
        if (!_roomsMap.ContainsKey(rd)) {
            var go = Instantiate(rd.RoomTemplate);
            _roomsMap.Add(rd, go);
            go.transform.SetParent(transform);
            go.transform.localPosition = new(0, 0, go.transform.localPosition.z);
        }

        foreach (var room in _roomsMap.Values) {
            room.SetActive(false);
        }

        _roomsMap[rd].SetActive(true);
    }
}
