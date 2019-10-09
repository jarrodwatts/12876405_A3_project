using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement_Player2 : MonoBehaviour {
    private Vector2 movement;
    private float movementSqrMagnitude;
    public float carSpeed;
    public float rotationSpeed = 200.0f;

    public Text player2LapsText;
    private int player2Laps;

    //get microphone input from other class
    public MicrophoneTransform_Player2 microphoneTransformerPlayerTwo;

    public float bonusSpeed;

    public AudioSource source;
    public AudioClip oofAudioClip;

    // Start is called before the first frame update
    void Start () {
        player2Laps = 0;
        SetLapsText ();

        microphoneTransformerPlayerTwo = gameObject.GetComponent<MicrophoneTransform_Player2> ();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        source = audioSources[0];

        oofAudioClip = audioSources[0].clip;

    }

    // Update is called once per frame
    void Update () {
        CarPosition ();
        CarRotation ();
        GetBonusSpeed ();
    }

    void OnTriggerEnter2D (Collider2D other) {

        if (other.gameObject.CompareTag ("LapLine")) {
            player2Laps = player2Laps + 1;
            SetLapsText ();
        }
    }

    void OnCollisionEnter2D (Collision2D other) {
        if (other.gameObject.CompareTag ("Track")) {
            source.PlayOneShot (oofAudioClip);
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
        if (player2Laps == 10) {
            Debug.Log("Game Over - Blue (Player 2) Wins!");
        }
    }

    void GetBonusSpeed () {
        //get the sound level from the other class 
        bonusSpeed = microphoneTransformerPlayerTwo.testSound;

        addBonusSpeed ();
    }

    void addBonusSpeed () {
        carSpeed = 0.6f + bonusSpeed / 2; //this is just a algorith for speed that feels controllable
    }
}