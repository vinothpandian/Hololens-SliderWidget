using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSphere : MonoBehaviour {

    private uint sliderValue;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3((sliderValue / 100f), transform.position.y , transform.position.z);
	}

    void GetSliderValue(uint value)
    {
        sliderValue = value;
    }
}
