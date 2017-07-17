using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TubeSliderManager : MonoBehaviour
{

	[SerializeField]
	public uint SliderMinimumValue = 0;
    public string SliderMinimumLabel;

	[SerializeField]
	public uint SliderMaximumValue = 100;
    public string SliderMaximumLabel;

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
    private GameObject buttonPivot;

    private string leftLabel;
	private string rightLabel;
	private string buttonLabel;

	private uint SliderRange;
	private float TotalDistance;
	private float CurrentDistance;
    private Vector3 startPos;
    private Vector3 endPos;
    private float diff;


    private bool isSliderManipulationTriggered;


	void Awake()
	{
		isSliderManipulationTriggered = false;

        leftHolder = GameObject.FindGameObjectWithTag("LeftHolder");
        rightHolder = GameObject.FindGameObjectWithTag("RightHolder");
        button = GameObject.FindGameObjectWithTag("SliderButton");
        buttonPivot = GameObject.FindGameObjectWithTag("ButtonPivot");

        SliderRange = SliderMaximumValue - SliderMinimumValue;

        startPos = leftHolder.transform.position;
        endPos = rightHolder.transform.position;

        TotalDistance = Vector3.Distance(startPos, endPos);

        diff = Vector3.Distance(button.transform.position, buttonPivot.transform.position);

        button.transform.position = startPos + (-button.transform.up.normalized * (((float)CurrentValue / (float)SliderRange)+2f*diff) * TotalDistance);

        button.GetComponent<Renderer>().material.color = ButtonColorOffFocus;

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

	public void ManipulationStarted(ManipulationEventData eventData)
	{
       
		if (!isSliderManipulationTriggered) 
		{
            button.GetComponent<Renderer> ().material.color = ButtonColorOnFocus;

			InputManager.Instance.PushModalInputHandler(button);

			rb = button.GetComponent<Rigidbody>();

			lastPosition = buttonPivot.transform.position;

            setDisplay(SliderMinimumLabel, SliderMaximumLabel, CurrentValue.ToString());

            isSliderManipulationTriggered = true;
		}
	}

	public void ManipulationUpdated(ManipulationEventData eventData)
	{
		
		if (isSliderManipulationTriggered) 
		{
			
			button.GetComponent<Renderer> ().material.color = ButtonColorOnFocus;

			Drag (eventData.CumulativeDelta);

			CurrentDistance = Vector3.Distance (startPos, buttonPivot.transform.position);

			CurrentValue = (uint) Mathf.RoundToInt(((CurrentDistance / TotalDistance) * SliderRange));

            setDisplay(SliderMinimumLabel, SliderMaximumLabel, CurrentValue.ToString());
        }
			
	}

	public void ManipulationCompleted(ManipulationEventData eventData)
	{
		if (isSliderManipulationTriggered) 
		{
			button.GetComponent<Renderer> ().material.color = ButtonColorOffFocus;
            setDisplay("", "", "");

			InputManager.Instance.PopModalInputHandler();

			isSliderManipulationTriggered = false;
		}

	}

	public void ManipulationCanceled(ManipulationEventData eventData)
	{
		if (isSliderManipulationTriggered) 
		{
			button.GetComponent<Renderer> ().material.color = ButtonColorOffFocus;
            setDisplay("", "", "");

            InputManager.Instance.PopModalInputHandler();

			isSliderManipulationTriggered = false;
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
		if (!isSliderManipulationTriggered) 
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