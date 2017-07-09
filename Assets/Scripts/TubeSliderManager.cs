using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TubeSliderManager : MonoBehaviour, IManipulationHandler
{

	[SerializeField]
	public int SliderMinimumValue = 0;

	[SerializeField]
	public int SliderMaximumValue = 100;

	[SerializeField]
	public static int CurrentValue = 10;

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

	private int SliderRange;
	private float TotalDistance;
	private float CurrentDistance;

	private bool isManipulationTriggered;


	void Awake()
	{
		isManipulationTriggered = false;

		if (GameObject.Find ("Button") != null) {
			button = GameObject.Find ("Button");
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
		if (GameObject.Find ("Button") != null) 
		{
			leftHolder = GameObject.Find ("LeftHolder");
			rightHolder = GameObject.Find ("RightHolder");
			button = GameObject.Find ("Button");
			button.GetComponent<Renderer> ().material.color = ButtonColorOnFocus;

			SliderRange = SliderMaximumValue - SliderMinimumValue;
			TotalDistance = Vector3.Distance (leftHolder.transform.position, rightHolder.transform.position);

			InputManager.Instance.PushModalInputHandler(button);

			rb = button.GetComponent<Rigidbody>();

			lastPosition = button.transform.position;

			isManipulationTriggered = true;
		}
	}

	public void OnManipulationUpdated(ManipulationEventData eventData)
	{
		
		if (isManipulationTriggered) 
		{
			button.GetComponent<Renderer> ().material.color = ButtonColorOnFocus;

			Drag (eventData.CumulativeDelta);

			CurrentDistance = Vector3.Distance (leftHolder.transform.position, button.transform.position);

			CurrentValue = Mathf.RoundToInt(((CurrentDistance / TotalDistance) * SliderRange));

			if (CurrentValue <= SliderRange / 2) {
				CurrentValue -= Mathf.RoundToInt (button.transform.localScale.x);
			} else {
				CurrentValue += Mathf.RoundToInt (button.transform.localScale.x);
			}

			leftHolder.GetComponentInChildren<TextMesh> ().text = SliderMinimumValue.ToString();
			rightHolder.GetComponentInChildren<TextMesh> ().text = SliderMaximumValue.ToString();
			button.GetComponentInChildren<TextMesh> ().text = CurrentValue.ToString();
		}
			
	}

	public void OnManipulationCompleted(ManipulationEventData eventData)
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
	}

	public void ButtonOffFocus() 
	{
		if (!isManipulationTriggered) 
		{
			button.GetComponent<Renderer> ().material.color = ButtonColorOffFocus;
		}
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