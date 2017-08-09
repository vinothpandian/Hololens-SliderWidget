using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Assists application developer to connect Slider and its associated object
/// </summary>
public class SliderConnection : MonoBehaviour {

    // Assign the slider to script
    [Tooltip("Drag the slider object to link the values")]
    public GameObject Slider;

    // Variable to store the current value of the linked slider
	public uint Value;

    // Variable to contact the TubeSliderManger script of the linked slider
    private TubeSliderManager linkedSlider;

	// Use this to initialize the slider
	void Awake () {
        // Skip execution if the slider is not assigned
		if(Slider == null)
		{
			return;
		}

        // Contact the TubeSliderManger script of the linked slider and get the current value of slider
        linkedSlider = GameObject.Find(Slider.name).GetComponent<TubeSliderManager>();
        Value = linkedSlider.CurrentValue;

        // Pass the value to any script associated with the current game object
        gameObject.SendMessage("GetSliderValue", Value);
	}
	
	// Update is called once per frame
	void Update () {
        
        // get the current value of slider
        Value = linkedSlider.CurrentValue;

        // Pass the value to any script associated with the current game object
        gameObject.SendMessage("GetSliderValue", Value);
    }
}
