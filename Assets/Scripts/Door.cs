using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorDir {
    NORTH,
    SOUTH,
    WEST,
    EAST
}

public enum DoorState {
    WALL,
    OPEN,
    LOCKED
}

public class Door : MonoBehaviour
{
    public DoorDir doorDirection;

    public BoxCollider2D wallCollider;
    public BoxCollider2D triggerCollider;
    public SpriteRenderer wallTexture;


    public DoorState _state;
    public DoorState State {
        get => _state;
        set {
            _state = value;

            // if wall, enable wall collider and wall sprite
            // if open, disable wall collider and enable open wall sprite
            // if locked, enable wall collider and locked sprite

            var isWall = _state == DoorState.WALL;
            wallCollider.enabled = isWall;
            wallTexture.enabled = isWall;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        State = DoorState.WALL;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
