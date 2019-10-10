using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
    This class is for Player 1 (The Red Car) - contains all the controls and interactions of the Red Car
    Including the speed manipulation through microphone input,
    Playing sounds via trigger and collision events,
    and Updating the laps through trigger events.
 */
public class CarMovement_Player1 : MonoBehaviour {
    private Vector2 movement;
    private float movementSqrMagnitude;
    public float carSpeed;
    public float rotationSpeed = 200.0f;

    public Text player1LapsText;
    private int player1Laps;

    //Get microphone input from other class
    public MicrophoneTransform microphoneTransformer;

    public float bonusSpeed;

    private GameObject trackGameObject;

    //Set out audio layout so that oof sound can play onCollision
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

        //Create an array of AudioSources since if we're using GetComponents then we can grab multiple of the same type
        AudioSource[] audioSources = GetComponents<AudioSource>();
        source = audioSources[0];
        vroomAudioClip = audioSources[1].clip;

        oofAudioClip = audioSources[0].clip;

    }

    // Update is called once per frame
    void Update () {
        //Call each method we would like to have happen once per frame
        CarPosition ();
        CarRotation ();
        GetBonusSpeed ();
    }

    //Triggered when the Lap Line Collider is passed through by a car
    void OnTriggerEnter2D (Collider2D other) {

        if (other.gameObject.CompareTag ("LapLine")) {
            player1Laps = player1Laps + 1;
            SetLapsText ();
        }
    }

    //Triggered when the Car hits an area of the track - play oof sound effect
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

    //Change the Laps above the track to increment by 1 for player 1 - this method is called when the lap line is scrossed in onTriggerEnter2D.
    void SetLapsText () {
        player1LapsText.text = "Player 1: " + player1Laps.ToString ();
        if (player1Laps == 10) {
            Debug.Log("Game Over - Red (Player 1) Wins!");
            SceneManager.LoadScene("EndGameScene", LoadSceneMode.Single); //end the game by swapping to the endgamescene at 10 laps 
        }
    }

    //Use the microphoneTransformer script's shared variable to gather how loud player 1's input device is, add that to speed.
    void GetBonusSpeed () {
        //get the sound level from the other class 
        bonusSpeed = microphoneTransformer.testSound;

        addBonusSpeed ();
    }

    //Add the microphone input level to speed variable
    void addBonusSpeed () {
        carSpeed = 0.6f + bonusSpeed / 2; //this is just a algorith for speed that feels controllable
    }
}