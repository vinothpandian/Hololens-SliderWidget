using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("ManStart!");
        SendMessageUpwards("ManipulationStarted", eventData);
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        Debug.Log("ManUpdate!");
        SendMessageUpwards("ManipulationUpdated", eventData);

    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        Debug.Log("ManCompl!");
        SendMessageUpwards("ManipulationCompleted", eventData);

    }

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        Debug.Log("ManCan!");
        SendMessageUpwards("ManipulationCanceled", eventData);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }


}
