using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData {
    public GameObject RoomTemplate { get; }
    public RoomData(GameObject roomTemplate) {
        RoomTemplate = roomTemplate;
    }   
}

public class FloorController : MonoBehaviour
{
    private static Dictionary<DoorDir, Vector2> _dirMap = new() {
        {DoorDir.NORTH, new Vector2(0, 1)},
        {DoorDir.SOUTH, new Vector2(0, -1)},
        {DoorDir.WEST, new Vector2(-1, 0)},
        {DoorDir.EAST, new Vector2(1, 0)},
    };

    public GameObject layout;
    public Floor floorData;
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
    private FloorTemplate _template;
    private int _pX;
    private int _pY;

    private RoomData?[,] _rData;    

    void Start()
    {
        _doorControllers = new();
        foreach (var door in doors)
            _doorControllers.Add(door.GetComponent<Door>());

        _generateFloor();
        _rData = new RoomData?[_template.Height, _template.Width];
        _generateRoomData(true);
        _loadCurrentRoom();
    }

    void _generateFloor() {
        _template = new FloorTemplate(".###!\n..@..\n..#..\n.$#*.\n..#..");
        _pX = _template.SpawnX;
        _pY = _template.SpawnY;
    }

    void _generateRoomData(bool isSpawn=false) {
        if (_rData[_pY, _pX] is not null) {
            return;
        }

        var layoutI = Random.Range(0, floorData.roomLayouts.Count);
        if (isSpawn) layoutI = 0;

        // TODO use the template
        _rData[_pY, _pX] = new RoomData(floorData.roomLayouts[layoutI]);
    }

    void _loadCurrentRoom() {
        // load doors
        foreach (var d in doors) {
            var door = d.GetComponent<Door>();
            door.State = DoorState.WALL;
            var diff = _diffFromDir(door.doorDirection);
            var newPos = new Vector2(_pY - diff[1], _pX + diff[0]);
            if (newPos[0] >= _template.Height || newPos[1] >= _template.Width || newPos[0] < 0 || newPos[1] < 0) {
                continue;
            }
            var next = _template.Rooms[(int)newPos[0], (int)newPos[1]];
            if (next is null) {
                continue;
            }
            door.State = DoorState.OPEN;
        }

        _generateRoomData();

        // load prefab variant
        layout.GetComponent<LayoutController>().Load(_rData[_pY, _pX]);
    }

    private static Vector2 _diffFromDir(DoorDir dir) => _dirMap[dir];

    public void SetDoorTriggersEnabled(bool v) {
        foreach (var dr in _doorControllers)
            dr.triggerCollider.enabled = v;
    }

    public void OnPlayerDoorInteract(DoorDir dir) {
        var t = Camera.main.transform;
        var diff = _diffFromDir(dir);
        var newPos = new Vector2(t.position.x, t.position.y) + diff * new Vector2(roomSizeX, roomSizeY);
        var target = new Vector3(newPos.x, newPos.y, t.position.z);

        doorsContainer.transform.position = newPos;
        player.transform.position = preloadedRooms[PreloadI].GetComponent<RoomController>().GetSpawnLocation(dir);

        _pX += (int)diff[0];
        _pY -= (int)diff[1];
        _loadCurrentRoom();

        LeanTween.move(Camera.main.gameObject, target, .1f);
        ++PreloadI;
    }

    public void OnPlayerEnterPreloadArea(DoorDir dir) {
        var t = preloadedRooms[(PreloadI + 1) % preloadedRooms.Count].transform;
        var newPos = new Vector2(t.position.x, t.position.y) + _diffFromDir(dir) * new Vector2(roomSizeX, roomSizeY);

        preloadedRooms[PreloadI].transform.position = new Vector3(newPos.x, newPos.y, t.position.z);
    }
}
