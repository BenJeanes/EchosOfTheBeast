using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneInput : MonoBehaviour
{
    //Declare Global Variables
    
    //End Result - The volume of the microphone's input, but normalized
    public static float normalizedMicrophoneInput;
    //Microphone reference
    private string inputDevice;
    //Microphone Input
    private AudioClip audioClip;
    //Sample Rate
    int sampleWindow = 128;
    //Test Variable
    public float soundLevel;
    //Test Light
    public Light light;
    //Test Intensity
    private float intensity = 0;
    public float multiplier = 1;

	// Use this for initialization
	void Start ()
    {
		if(inputDevice == null)
        {
            inputDevice = Microphone.devices[0];
            audioClip = Microphone.Start(inputDevice, true, 999, 44100);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        float incrementalMultiplier = 0.005f;

        //Get Returned Sound Level for the Light Object
        normalizedMicrophoneInput = MaxVolume();
        soundLevel = normalizedMicrophoneInput;

        /*
        if (intensity < soundLevel)
        {
            intensity = soundLevel * 5;
        }
        else
        {
            intensity -= 0.01f;
        }
        */

        //If the Input Sound Level is higher than X threshold, the intensity variable becomes soundLevel (normalised between 0 and 1) times the base multiplier
        if (soundLevel > (intensity/multiplier))
        {
            intensity = soundLevel * multiplier;
        }

        //If intensity is less than 0, stop reducing intensity
        else if(intensity <= 0)
        {
            //Do nothing
        }
        //Slowly reduce the intensity variable
        else
        {
           
            intensity -= (intensity * (incrementalMultiplier + 0.005f)) + 0.005f;
        }

        Debug.Log("Final Intensity Value = " + intensity + ", Sound Level = " + soundLevel);
        
        //Set the intensity property to the Light Objects Intensity property
        light.intensity = intensity;
	}

    //Get a Normalised Volume Peak from the Sampled Audio Clip
    float MaxVolume()
    {
        //Float to return
        float maxVolume = 0;
        //Array of Samples from the Audio Input
        float[] clipSampleData = new float[sampleWindow];
        //Position of the Sample
        int micPosition = Microphone.GetPosition(inputDevice) - (sampleWindow + 1);

        //Error Checking
        if (micPosition < 0)
        {
            return 0;
        }

        //Populating the Array from the audio input
        audioClip.GetData(clipSampleData, micPosition);
        
        //For each sample
        for (int i = 0; i < sampleWindow; i++)
        {
            //Get the peak volume by Squaring the Data Sample (Normalizing)
            float wavePeak = clipSampleData[i] * clipSampleData[i];
            
            //If the volume isn't about a certain threshold
            if(wavePeak < 0.001)
            {
                wavePeak = 0;
            }

            //Else if the volume is above the previously sampled volume, save it.
            if (maxVolume < wavePeak)
            {
                maxVolume = wavePeak;
            }
        }
        
        //return the float
        return maxVolume;
    }
}
