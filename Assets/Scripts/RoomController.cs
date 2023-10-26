using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<GameObject> spawns;

    private Dictionary<DoorDir, GameObject> _spawnMap;
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnMap = new();
        foreach (var spawn in spawns) {
            var dir = spawn.GetComponent<PlayerSpawn>().fromDirection;
            _spawnMap[dir] = spawn;
        }
        
    }

    public Vector3 GetSpawnLocation(DoorDir fromDir) {
        return _spawnMap[fromDir].transform.position;
    }
}
