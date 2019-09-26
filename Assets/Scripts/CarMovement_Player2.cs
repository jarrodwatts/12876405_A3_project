using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement_Player2 : MonoBehaviour {
    private Vector2 movement;
    private float movementSqrMagnitude;
    public float carSpeed = 0.8f;
    public float rotationSpeed = 200.0f;

    public Text player2LapsText;
    private int player2Laps;

    // Start is called before the first frame update
    void Start () {
        player2Laps = 0;
        SetLapsText ();
    }

    // Update is called once per frame
    void Update () {
        CarPosition ();
        CarRotation ();
    }

    void OnTriggerEnter2D (Collider2D other) {

        if (other.gameObject.CompareTag ("LapLine")) {
            player2Laps = player2Laps + 1;
            SetLapsText ();
        }
    }

    void CarPosition () {
        // Get the vertical axis.
        // The value is in the range -1 to 1
        float translationVert = Input.GetAxis ("Vertical_Player2") * carSpeed;
        // Make it move 10 meters per second instead of 10 meters per frame
        translationVert *= Time.deltaTime;
        // Move translation along the object's y-axis
        transform.Translate (0, translationVert, 0);
    }

    void CarRotation () {
        // Get the horizontal axis.
        // The value is in the range -1 to 1
        float rotation = Input.GetAxis ("Horizontal_Player2") * rotationSpeed;

        // Make it rotate 10 meters per second instead of 10 meters per frame
        rotation *= Time.deltaTime;

        // Rotate around our z-axis
        transform.Rotate (0, 0, rotation);
    }

    void SetLapsText () {
        player2LapsText.text = "Player 2: " + player2Laps.ToString ();
    }
}