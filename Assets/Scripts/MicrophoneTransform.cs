using System.Collections;
using UnityEngine;

/**
    This class is for the Microphone Input capture for player 1.
    In essence, we are constantly capturing input levels of the first/default input device that the user has on their computer.
    Then we convert that to a small float that we can add to the speed of the player 1 (red) car in a field called bonusSpeed.

    NOTE: This code is largely inspired by this Stack Overflow thread: 
    https://stackoverflow.com/questions/37443851/unity3d-check-microphone-input-volume
    Stack Overflow: Unity3d: Check Microphone Input Volume : Asked 3 years, 4 months ago.

    I have tried to add as much commenting as possible to show that I understand the code - may seem a little excessive but this is why :).
 */
public class MicrophoneTransform : MonoBehaviour {

    public float testSound; //The variable that we share with CarMovement_Player1 to add to bonus sound
    public static float MicLoudness; //Since this is static we can't share it, however it's basically the same as above.
    private string _device; //The microphone device that this user has, is set below by using Unity's Microphone class. Set to the 0th microphone that is available.
    private AudioClip _clipRecord; //Where we store the actual input audio that is coming through the microphone in an input clip
    private int _sampleWindow = 128; //This number must be a power of 2, (i.e. 128, 256, 512) etc, I chose 128 since it's easier to manage and we don't really need that much precision; as we just want peaks.

    //This is where we initialise the microphone device - we want to choose the first available microphone for player 1.
    void InitMic () {
        //device should always be null unless a user has somehow set it already
        if (_device == null) {
            //check if there is actually ANY microphone available, if not then this whole class is not going to do anything which is fine - there will just be no bonus speed feature for users with no microphones.
            if (Microphone.devices.Length != 0) {
                //Set the _device variable to the 0th microphone available on this device/pc
                _device = Microphone.devices[0];
                //Debug statement to see what input device is being used for player 1.
                Debug.Log ("Player 1 Input Device is: " + Microphone.devices[0]);
                //Start recording and storing the recording in our clipRecord variable
                _clipRecord = Microphone.Start (_device, true, 999, 44100); //these parameters are saying, use the device that we declared above (0th mic), loop it continuously so we can keep recording while user plays. 999 seconds is irrelevant since we are looping
                //... and 44100 is the frequency/sample rate that we are recording at.
            }
        }
    }

    //The way that sound works is there are different frequencies that we hear at, and essentially each different type of sound that we hear can be mapped out into a series of different levels/pitches.
    //What this method is doing is, breaking down the input that we are capturing from the microphone, and putting it into an array of floats called waveData.
    //This float is 128 in length, because we can split each frequency/pitch into an amount.
    //For example... if i was screaming really high pitched into the mic, our higher end floats in the array would be very high,
    //and if i was speaking in a very low voice, the lower end floats would be very high, while the higher end floats wouldn't.
    float LevelMax () {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow]; //set the length of waveData to be 128 

        //Set the micPosition so we can provide this information below (in GetData)
        int micPosition = Microphone.GetPosition (null) - (_sampleWindow + 1);

        //If there is no mic then exit with 0 otherwise we are crashing
        if (micPosition < 0) {
            return 0;
        }

        //Now we fill the actual array waveData with the input coming from mic in micPostion set above
        _clipRecord.GetData (waveData, micPosition);

        //Loop through 1 - 128 to fill out the waveData arrray
        for (int i = 0; i < _sampleWindow; ++i) {
            //Here is where we are getting the bonus speed from, we grab the highest volume out of the 128 floats in waveData, and constantly compare it with a simple search algorithm
            float wavePeak = waveData[i] * waveData[i]; //generate wave peak 
            if (levelMax < wavePeak) { //compare wave peak with what our current wave peak is
                levelMax = wavePeak; //if it's bigger then we have a new winner
            }
        }
        return levelMax;
    }

    //constantly provide a new max peak each frame
    void Update () {
        MicLoudness = LevelMax ();
        testSound = MicLoudness; //provide the testSound variable to the other classes (Cars) to add to bonusSpeed.
    }

    //when it starts up set up the microphone, if there isn't one then theres nothing going to happen.
    void OnEnable () {
        InitMic ();
    }

}