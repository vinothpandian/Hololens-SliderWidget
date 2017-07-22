using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour {

    private AudioSource audioSource;
    private uint sliderValue;

    // Use this for initialization
    void Awake () {
        
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = sliderValue / 100f;
	}
	
	// Update is called once per frame
	void Update () {
        audioSource.volume = sliderValue / 100f;
    }

    void GetSliderValue(uint value)
    {
        sliderValue = value;
    }
}
