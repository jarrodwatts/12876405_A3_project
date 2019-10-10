using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
    This class is for Player 2 (The Blue Car) - contains all the controls and interactions of the Red Car
    Including the speed manipulation through microphone input,
    Playing sounds via trigger and collision events,
    and Updating the laps through trigger events.
 */
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

    //In comparison to red car, this only has the oof sound effect
    //However it's a bit more consistent if we use an array in case we want to add more stuff in the future.
    public AudioSource source;
    public AudioClip oofAudioClip;

    // Start is called before the first frame update
    void Start () {
        player2Laps = 0;
        SetLapsText ();

        microphoneTransformerPlayerTwo = gameObject.GetComponent<MicrophoneTransform_Player2> ();

        AudioSource[] audioSources = GetComponents<AudioSource> ();
        //Since there is no audio sound effect from this car engine, we only have the oof Sound effect
        source = audioSources[0];

        //meaning that oof is set to [0] and that there is no [1] object like there is for the car revving sounds in Red car / player 1.
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
            player2Laps = player2Laps + 1;
            SetLapsText ();
        }
    }

    //Triggered when the Car hits an area of the track - play oof sound effect
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
            Debug.Log ("Game Over - Blue (Player 2) Wins!");
            SceneManager.LoadScene("EndGameScene", LoadSceneMode.Single); //end the game by swapping to the endgamescene at 10 laps 
        }
    }

    //Use the microphoneTransformer script's shared variable to gather how loud player 2's input device is, add that to speed.
    //If there is no second input device, this error is handled in the MicrophoneTransform_player2 class.
    void GetBonusSpeed () {
        //get the sound level from the other class 
        bonusSpeed = microphoneTransformerPlayerTwo.testSound;

        addBonusSpeed ();
    }

    //Add the microphone input level to speed variable
    void addBonusSpeed () {
        carSpeed = 0.6f + bonusSpeed / 2; //this is just a algorith for speed that feels controllable
    }
}