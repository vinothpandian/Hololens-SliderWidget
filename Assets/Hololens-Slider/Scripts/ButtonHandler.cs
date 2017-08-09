using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Implements the Manipulation handler and focusable handler from HoloToolkit and passes the event to the TubeSliderManager on each event trigger
/// </summary>
public class ButtonHandler : MonoBehaviour, IFocusable, IManipulationHandler
{


	public void OnFocusEnter() 
	{

		SendMessageUpwards ("ButtonOnFocus");
	}
	public void OnFocusExit() 
	{
		SendMessageUpwards ("ButtonOffFocus");
	}

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        SendMessageUpwards("ManipulationStarted", eventData);
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        SendMessageUpwards("ManipulationUpdated", eventData);

    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        SendMessageUpwards("ManipulationCompleted", eventData);

    }

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        SendMessageUpwards("ManipulationCanceled", eventData);
    }


}
