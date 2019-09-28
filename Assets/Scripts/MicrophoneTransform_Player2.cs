using System.Collections;
using UnityEngine;

public class MicrophoneTransform_Player2 : MonoBehaviour {

    public float testSound;
    public static float MicLoudness;
    private string _device;
    private AudioClip _clipRecord;
    private int _sampleWindow = 128;
    private bool _isInitialized;

    void InitMic () {
        if (_device == null) {
            if (Microphone.devices.Length > 1) {
                _device = Microphone.devices[1];
                Debug.Log ("Player 2 Input Device is: " + Microphone.devices[1]);
                _clipRecord = Microphone.Start (_device, true, 999, 44100);
            }
        }
    }

    float LevelMax () {
        if (Microphone.devices.Length > 1) {
            float levelMax = 0;
            float[] waveData = new float[_sampleWindow];
            int micPosition = Microphone.GetPosition (null) - (_sampleWindow + 1);
            if (micPosition < 0) {
                return 0;
            }
            _clipRecord.GetData (waveData, micPosition);
            for (int i = 0; i < _sampleWindow; ++i) {
                float wavePeak = waveData[i] * waveData[i];
                if (levelMax < wavePeak) {
                    levelMax = wavePeak;
                }
            }
            return levelMax;
        }
        else {
            return 0;
        }
    }

    void Update () {
        MicLoudness = LevelMax ();
        testSound = MicLoudness;
    }

    void OnEnable () {
        InitMic ();
    }

}