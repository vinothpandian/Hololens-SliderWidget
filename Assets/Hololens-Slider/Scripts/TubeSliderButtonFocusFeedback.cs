using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeSliderButtonFocusFeedback : MonoBehaviour, IFocusable {


	public void OnFocusEnter() 
	{

		SendMessageUpwards ("ButtonOnFocus");
	}
	public void OnFocusExit() 
	{
		SendMessageUpwards ("ButtonOffFocus");
	}
		

}
