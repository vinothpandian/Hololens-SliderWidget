using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderConnection : MonoBehaviour {

	[Tooltip("Drag the slider object to link the values")]
	public GameObject Slider;

	public static uint Value;

	// Use this for initialization
	void Awake () {
		if(Slider == null)
		{
			return;
		}
			
		Value = Slider.GetComponent<TubeSliderManager> ().CurrentValue;
	}
	
	// Update is called once per frame
	void Update () {
		Value = Slider.GetComponent<TubeSliderManager> ().CurrentValue;
	}
}
