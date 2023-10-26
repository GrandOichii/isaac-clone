using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public float roomSizeX = 18.45f;
    public float roomSizeY = 10f;

    public List<GameObject> doors;
    public GameObject doorsContainer;
    public GameObject player;

    public List<GameObject> preloadedRooms;
    private int _preloadI = 1;
    private int PreloadI {
        get => _preloadI;
        set {
            _preloadI = value;
            var c = preloadedRooms.Count;
            if (_preloadI < 0) _preloadI = c - 1;
            if (_preloadI >= c) _preloadI = 0;
        }
    }

    private List<Door> _doorControllers;

    void Start()
    {
        _generateFloor();

        _doorControllers = new();
        foreach (var door in doors)
            _doorControllers.Add(door.GetComponent<Door>());

        ShuffleStates();
    }

    void _generateFloor() {
        var template = new FloorTemplate(".###!\n..@..\n..#..\n.$#*.\n..#..");
        
    }

    void ShuffleStates() {
        foreach (var dc in _doorControllers) {
            var state = (DoorState)Random.Range(1, 2);
            dc.State = state;
        }

    }

    private static Dictionary<DoorDir, Vector2> _dirMap = new() {
        {DoorDir.NORTH, new Vector2(0, 1)},
        {DoorDir.SOUTH, new Vector2(0, -1)},
        {DoorDir.WEST, new Vector2(-1, 0)},
        {DoorDir.EAST, new Vector2(1, 0)},
    };

    private static Vector2 _diffFromDir(DoorDir dir) => _dirMap[dir];

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDoorTriggersEnabled(bool v) {
        foreach (var dr in _doorControllers)
            dr.triggerCollider.enabled = v;
    }

    public void OnPlayerDoorInteract(DoorDir dir) {
        var t = Camera.main.transform;
        var newPos = new Vector2(t.position.x, t.position.y) + _diffFromDir(dir) * new Vector2(roomSizeX, roomSizeY);
        var target = new Vector3(newPos.x, newPos.y, t.position.z);

        doorsContainer.transform.position = target;
        player.transform.position = preloadedRooms[PreloadI].GetComponent<RoomController>().GetSpawnLocation(dir);
        
        LeanTween.move(Camera.main.gameObject, target, .1f);
        ++PreloadI;
    }

    public void OnPlayerEnterPreloadArea(DoorDir dir) {
        var t = preloadedRooms[(PreloadI + 1) % preloadedRooms.Count].transform;
        var newPos = new Vector2(t.position.x, t.position.y) + _diffFromDir(dir) * new Vector2(roomSizeX, roomSizeY);

        preloadedRooms[PreloadI].transform.position = new Vector3(newPos.x, newPos.y, t.position.z);
    }
}
