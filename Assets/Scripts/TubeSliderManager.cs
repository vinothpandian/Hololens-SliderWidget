using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TubeSliderManager : MonoBehaviour, IManipulationHandler
{

	[SerializeField]
	public uint SliderMinimumValue = 0;

	[SerializeField]
	public uint SliderMaximumValue = 100;

	public uint CurrentValue = 4;

	[SerializeField]
	float DragSpeed = 1.5f;

	[SerializeField]
	float DragScale = 1.5f;

	[SerializeField]
	float MaxDragDistance = 3f;

	public Stats ButtonColor;
	public Color ButtonColorOffFocus;
	public Color ButtonColorOnFocus;

	private Rigidbody rb;

	Vector3 lastPosition;

	private GameObject leftHolder;
	private GameObject rightHolder;
	private GameObject button;

    private string leftLabel;
	private string rightLabel;
	private string buttonLabel;

	private uint SliderRange;
	private float TotalDistance;
	private float CurrentDistance;

	private bool isManipulationTriggered;


	void Awake()
	{
		isManipulationTriggered = false;

		if (GameObject.FindGameObjectWithTag ("SliderButton") != null) {
			button = GameObject.FindGameObjectWithTag ("SliderButton");
			button.GetComponent<Renderer> ().material.color = ButtonColorOffFocus;
		}
			
	}

	public Color buttonColorOffFocus {
		get { return ButtonColorOffFocus; }
		set { ButtonColorOffFocus = value; 
			for(int i = 0; i < 4; ++i)
				ButtonColor.ButtonColorOffFocusArr[i] = ButtonColorOffFocus[i]; }
	}

	public Color buttonColorOnFocus {
		get { return ButtonColorOnFocus; }
		set { ButtonColorOnFocus = value; 
			for(int i = 0; i < 4; ++i)
				ButtonColor.ButtonColorOnFocusArr[i] = ButtonColorOnFocus[i]; }
	}

	public void OnManipulationStarted(ManipulationEventData eventData)
	{
		if (GameObject.FindGameObjectWithTag ("SliderButton") != null) 
		{
			leftHolder = GameObject.FindGameObjectWithTag ("LeftHolder");
			rightHolder = GameObject.FindGameObjectWithTag ("RightHolder");
			button = GameObject.FindGameObjectWithTag ("SliderButton");

            button.GetComponent<Renderer> ().material.color = ButtonColorOnFocus;

			InputManager.Instance.PushModalInputHandler(button);

			rb = button.GetComponent<Rigidbody>();

			lastPosition = button.transform.position;

            setDisplay(SliderMinimumValue.ToString(), SliderMaximumValue.ToString(), CurrentValue.ToString());

            isManipulationTriggered = true;
		}
	}

	public void OnManipulationUpdated(ManipulationEventData eventData)
	{
		
		if (isManipulationTriggered) 
		{
			
			button.GetComponent<Renderer> ().material.color = ButtonColorOnFocus;


			SliderRange = SliderMaximumValue - SliderMinimumValue;

			Vector3 startPos = leftHolder.transform.position;
			Vector3 endPos = rightHolder.transform.position;

			TotalDistance = Vector3.Distance (startPos, endPos);

			Drag (eventData.CumulativeDelta);

			CurrentDistance = Vector3.Distance (startPos, button.transform.position);

			CurrentValue = (uint) Mathf.RoundToInt(((CurrentDistance / TotalDistance) * SliderRange));

			if (CurrentValue <= SliderRange / 2) {
				CurrentValue -= (uint) Mathf.RoundToInt (button.transform.localScale.x);
			} else {
				CurrentValue += (uint) Mathf.RoundToInt (button.transform.localScale.x);
			}

            setDisplay(SliderMinimumValue.ToString(), SliderMaximumValue.ToString(), CurrentValue.ToString());
        }
			
	}

	public void OnManipulationCompleted(ManipulationEventData eventData)
	{
		if (isManipulationTriggered) 
		{
			button.GetComponent<Renderer> ().material.color = ButtonColorOffFocus;
            setDisplay("", "", "");

			InputManager.Instance.PopModalInputHandler();

			isManipulationTriggered = false;
		}

	}

	public void OnManipulationCanceled(ManipulationEventData eventData)
	{
		if (isManipulationTriggered) 
		{
			button.GetComponent<Renderer> ().material.color = ButtonColorOffFocus;
			leftHolder.GetComponentInChildren<TextMesh> ().text = "";
			rightHolder.GetComponentInChildren<TextMesh> ().text = "";
			button.GetComponentInChildren<TextMesh> ().text = "";

			InputManager.Instance.PopModalInputHandler();

			isManipulationTriggered = false;
		}
	}

	public void Drag(Vector3 position)
	{
		var targetPosition = lastPosition + position * DragScale;
		if (Vector3.Distance(lastPosition, targetPosition) <= MaxDragDistance)
		{
			rb.velocity = (targetPosition	- button.transform.position) * DragSpeed;
		}
	}

	public void ButtonOnFocus() 
	{
		button.GetComponent<Renderer> ().material.color = ButtonColorOnFocus;
        button.GetComponentInChildren<TextMesh>().text = CurrentValue.ToString();

    }

	public void ButtonOffFocus() 
	{
		if (!isManipulationTriggered) 
		{
			button.GetComponent<Renderer> ().material.color = ButtonColorOffFocus;
            
        }

        button.GetComponentInChildren<TextMesh>().text = "";
    }

    public void setDisplay(string min, string max, string current)
    {
        leftHolder.transform.parent.GetComponentInChildren<TextMesh>().text = min;
        rightHolder.transform.parent.GetComponentInChildren<TextMesh>().text = max;
        button.GetComponentInChildren<TextMesh>().text = current;
    }

}


[System.Serializable]
public class Stats {
	public float[] ButtonColorOffFocusArr;
	public float[] ButtonColorOnFocusArr;
	public Stats()
	{
		ButtonColorOffFocusArr = new float[4];
		ButtonColorOnFocusArr = new float[4];
	}
}