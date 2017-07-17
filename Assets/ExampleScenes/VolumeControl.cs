using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour {

    private AudioSource audioSource;

	// Use this for initialization
	void Awake () {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = SliderConnection.Value / 100f;
	}
	
	// Update is called once per frame
	void Update () {
        audioSource.volume = SliderConnection.Value / 100f;
    }
}
