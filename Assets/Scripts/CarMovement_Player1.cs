using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement_Player1 : MonoBehaviour {
    private Vector2 movement;
    private float movementSqrMagnitude;
    public float carSpeed;
    public float rotationSpeed = 200.0f;

    public Text player1LapsText;
    private int player1Laps;

    //get microphone input from other class
    public MicrophoneTransform microphoneTransformer;

    public float bonusSpeed;

    private GameObject trackGameObject;

    public AudioSource source;
    public AudioClip vroomAudioClip;
    public AudioClip oofAudioClip;

    // Start is called before the first frame update
    void Start () {
        player1Laps = 0;
        SetLapsText ();

        //make the link from microphonetransform to this script
        //as they are both attached in theory we can use GetComponent
        microphoneTransformer = gameObject.GetComponent<MicrophoneTransform> ();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        source = audioSources[0];
        vroomAudioClip = audioSources[1].clip;

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
            player1Laps = player1Laps + 1;
            SetLapsText ();
        }
    }

    void OnCollisionEnter2D (Collision2D other) {
        if (other.gameObject.CompareTag ("Track")) {
            source.PlayOneShot(oofAudioClip);
        }
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

    void SetLapsText () {
        player1LapsText.text = "Player 1: " + player1Laps.ToString ();
        if (player1Laps == 10) {
            Debug.Log("Game Over - Red (Player 1) Wins!");
        }
    }

    void GetBonusSpeed () {
        //get the sound level from the other class 
        bonusSpeed = microphoneTransformer.testSound;

        addBonusSpeed ();
    }

    void addBonusSpeed () {
        carSpeed = 0.6f + bonusSpeed / 2; //this is just a algorith for speed that feels controllable
    }
}