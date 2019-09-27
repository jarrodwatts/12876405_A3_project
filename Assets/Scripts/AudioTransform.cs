using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent (typeof (AudioSource))]

public class AudioTransform : MonoBehaviour {
    AudioSource audioSource;
    public string selectedDevice;

    public float[] samples = new float[512];
    private float sum;
    private float averageSoundLevel;

    // Start is called before the first frame update
    void Start () {

        //if there is a mic connected
        if (Microphone.devices.Length > 0) {
            Debug.Log ("microhpone detecteD");
            selectedDevice = Microphone.devices[0].ToString ();
            Debug.Log (selectedDevice.ToString ());
            audioSource = GetComponent<AudioSource> ();
            audioSource.clip = Microphone.Start (selectedDevice, true, 10, AudioSettings.outputSampleRate);
            Debug.Log (audioSource);
            Debug.Log (audioSource.clip);
            audioSource.loop = true;
            //audioSource.Play (); //testing mic playback
        } else {
            audioSource = GetComponent<AudioSource> (); //get vroom sound effect levels
        }
    }

    // Update is called once per frame
    void Update () {
        GetSpectrumAudioSource ();
    }

    void GetSpectrumAudioSource () {
        //Using FFTWindow reduces leakage of signals across frequency bands (Unity Documentation)
        //Blackman is W[n] = 0.42 - (0.5 * COS(n/N) ) + (0.08 * COS(2.0 * n/N) ).
        //GetSpectrumData 
        audioSource.GetSpectrumData (samples, 0, FFTWindow.Blackman);
        GetAverage ();
    }

    void GetAverage () {
        for (int i = 0; i < samples.Length; i++) {
            sum += samples[i];
        }

        averageSoundLevel = sum / 512;
        float averageSoundLevelTimesHundredK = averageSoundLevel * 100000;
        //debug statements
        //Debug.Log ("size of array: " + samples.Length);
        //Debug.Log ("sum: " + sum);

        // if (averageSoundLevelTimesHundredK > 0) {
        //     Debug.Log (averageSoundLevelTimesHundredK);
        // }

    }
}