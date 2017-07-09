using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TubeSliderManager : MonoBehaviour, IManipulationHandler
{

	public void OnManipulationStarted(ManipulationEventData eventData)
	{
		

		if (GameObject.FindGameObjectWithTag ("SliderButton") != null) 
		{
			GameObject go = GameObject.FindGameObjectWithTag ("SliderButton");
			InputManager.Instance.PushModalInputHandler(go);
			go.GetComponent<DragButton>().DragStart(eventData.CumulativeDelta);
		}

	}

	public void OnManipulationUpdated(ManipulationEventData eventData)
	{
		if (GameObject.FindGameObjectWithTag ("SliderButton") != null) 
		{
			GameObject go = GameObject.FindGameObjectWithTag ("SliderButton");
			go.GetComponent<DragButton>().Drag(eventData.CumulativeDelta);
		}

	}

	public void OnManipulationCompleted(ManipulationEventData eventData)
	{
		InputManager.Instance.PopModalInputHandler();
	}

	public void OnManipulationCanceled(ManipulationEventData eventData)
	{
		InputManager.Instance.PopModalInputHandler();
	}


}
