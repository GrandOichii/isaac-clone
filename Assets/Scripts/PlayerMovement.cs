using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DoorInteractEvent : UnityEvent<DoorDir> {}

[Serializable]
public class NumberedStatChanged : UnityEvent<string, int> {}

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D body;
    private Vector2 _moveDir;
    public LayerMask doorInteractLayer;

    public DoorInteractEvent OnDoorInteract;
    public DoorInteractEvent PreloadRoomInteract;
    public NumberedStatChanged NumberedStatChanged;

    private int _coins;
    public int Coins {
        get => _coins;
        set {
            _coins = value;
            if (_coins < 0) _coins = 0;
            NumberedStatChanged.Invoke("coins", _coins);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Coins = 0;
    }

    void Update() {
        ProcessInputs(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        _moveDir = new Vector2(moveX, moveY).normalized;
    }

    void Move() {
        body.velocity = _moveDir * new Vector2(moveSpeed, moveSpeed);
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("doors")) {
            OnDoorInteract.Invoke(collider.gameObject.GetComponent<Door>().doorDirection);
            return;
        }
        if (collider.gameObject.CompareTag("preload-room")) {
            PreloadRoomInteract.Invoke(collider.gameObject.transform.parent.gameObject.GetComponent<Door>().doorDirection);
            return;
        }
        if (collider.gameObject.CompareTag("pickup")) {
            _pickup(collider);
        }
        // transform.position = Camera.main.transform.position;
    }

    private void _pickup(Collider2D collider) {
        var pController = collider.GetComponent<PickupableController>();
        pController.Pickup(this);
    }
}
