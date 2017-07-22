using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour {

    private uint sliderValue;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 ( transform.position.x, (sliderValue / 50.0f), transform.position.z);
	}

    void GetSliderValue(uint value)
    {
        sliderValue = value;
    }
}
