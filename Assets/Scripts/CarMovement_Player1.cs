using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement_Player1 : MonoBehaviour {
    private Vector2 movement;
    private float movementSqrMagnitude;
    public float carSpeed = 0.8f;
    public float rotationSpeed = 200.0f;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        CarPosition();
        CarRotation();
    }

    void CarPosition () {
        // Get the vertical axis.
        // The value is in the range -1 to 1
        float translationVert = Input.GetAxis ("Vertical") * carSpeed;
        // Make it move 10 meters per second instead of 10 meters per frame
        translationVert *= Time.deltaTime;
        // Move translation along the object's y-axis
        transform.Translate (0, translationVert, 0);
    }

    void CarRotation () {
        // Get the horizontal axis.
        // The value is in the range -1 to 1
        float rotation = Input.GetAxis ("Horizontal") * rotationSpeed;

        // Make it rotate 10 meters per second instead of 10 meters per frame
        rotation *= Time.deltaTime;

        // Rotate around our z-axis
        transform.Rotate (0, 0, rotation);
    }
}