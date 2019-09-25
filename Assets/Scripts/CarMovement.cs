using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour {
    private Vector2 movement;
    private float movementSqrMagnitude;
    public float carSpeed = 0.80f;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        GetMovementInput ();
        CarPosition ();
        CarRotation ();
        EngineAnimation ();
        EngineAudio ();
    }

    void GetMovementInput () {
        movement = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));

        Vector2.ClampMagnitude (movement, 1.0f);
        movementSqrMagnitude = movement.sqrMagnitude;
    }

    void CarPosition () {
        transform.Translate (movement * carSpeed * Time.deltaTime, Space.World);
    }

    void CarRotation () {
        // transform.Rotate (0, 0, Input.GetAxis ("Horizontal"), Space.World);
    }

    void EngineAnimation () {

    }

    void EngineAudio () {

    }
}