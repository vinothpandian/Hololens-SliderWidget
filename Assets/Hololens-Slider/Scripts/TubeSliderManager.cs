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

    private Vector3 lineVector;

    private bool isSliderManipulationTriggered;
    private Vector3 lastDeltaPosition;



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

        button.transform.position = startPos + (-button.transform.up.normalized * (((float)CurrentValue / (float)SliderRange) + 2f * diff) * (TotalDistance - 2f * diff));
        
        lineVector = startPos - endPos;

        

        button.GetComponent<Renderer>().material.color = ButtonColorOffFocus;

    }

    public Color buttonColorOffFocus
    {
        get { return ButtonColorOffFocus; }
        set
        {
            ButtonColorOffFocus = value;
            for (int i = 0; i < 4; ++i)
                ButtonColor.ButtonColorOffFocusArr[i] = ButtonColorOffFocus[i];
        }
    }

    public Color buttonColorOnFocus
    {
        get { return ButtonColorOnFocus; }
        set
        {
            ButtonColorOnFocus = value;
            for (int i = 0; i < 4; ++i)
                ButtonColor.ButtonColorOnFocusArr[i] = ButtonColorOnFocus[i];
        }
    }

    public float RangePosition { get; private set; }

    public void ManipulationStarted(ManipulationEventData eventData)
    {

        if (!isSliderManipulationTriggered)
        {
            button.GetComponent<Renderer>().material.color = ButtonColorOnFocus;

            InputManager.Instance.PushModalInputHandler(button);

            rb = button.GetComponent<Rigidbody>();

            lastPosition = button.transform.position;

            lastDeltaPosition = eventData.CumulativeDelta;

            setDisplay(SliderMinimumLabel, SliderMaximumLabel, CurrentValue.ToString());

            isSliderManipulationTriggered = true;
        }
    }

    public void ManipulationUpdated(ManipulationEventData eventData)
    {

        if (isSliderManipulationTriggered)
        {

            button.GetComponent<Renderer>().material.color = ButtonColorOnFocus;

            Vector3 newPosition;

            Vector3 mvmt = Vector3.Project(eventData.CumulativeDelta, lineVector.normalized);

            float deltaMovement = Vector3.Distance(mvmt, lastDeltaPosition);

            float angleOfMovement = Vector3.Angle(eventData.CumulativeDelta, lineVector.normalized);

            Debug.DrawLine(mvmt, lastDeltaPosition, Color.red, 2, false);

            if (angleOfMovement < 90)
            {
                newPosition = lastPosition + (button.transform.up.normalized * deltaMovement);
            }
            else
            {
                newPosition = lastPosition + (button.transform.up.normalized * -deltaMovement);
            }

            float angleMinBound = Vector3.Angle(lineVector.normalized, startPos - newPosition);
            float angleMaxBound = Vector3.Angle(lineVector.normalized, endPos - newPosition);


            if (angleMinBound < 90 && angleMaxBound > 90)
            {
                button.transform.position = newPosition;
            }

            //  Value calculation
            CurrentDistance = Vector3.Distance(startPos, buttonPivot.transform.position);

            CurrentValue = (uint)Mathf.RoundToInt((((CurrentDistance / (TotalDistance - 2f * diff) ) ) * SliderRange));

            setDisplay(SliderMinimumLabel, SliderMaximumLabel, CurrentValue.ToString());
        }

    }

    public void ManipulationCompleted(ManipulationEventData eventData)
    {
        if (isSliderManipulationTriggered)
        {
            button.GetComponent<Renderer>().material.color = ButtonColorOffFocus;
            setDisplay("", "", "");

            InputManager.Instance.PopModalInputHandler();

            isSliderManipulationTriggered = false;
        }

    }

    public void ManipulationCanceled(ManipulationEventData eventData)
    {
        if (isSliderManipulationTriggered)
        {
            button.GetComponent<Renderer>().material.color = ButtonColorOffFocus;
            setDisplay("", "", "");

            InputManager.Instance.PopModalInputHandler();

            isSliderManipulationTriggered = false;
        }
    }

    public void ButtonOnFocus()
    {
        button.GetComponent<Renderer>().material.color = ButtonColorOnFocus;
        button.GetComponentInChildren<TextMesh>().text = CurrentValue.ToString();

    }

    public void ButtonOffFocus()
    {
        if (!isSliderManipulationTriggered)
        {
            button.GetComponent<Renderer>().material.color = ButtonColorOffFocus;

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
public class Stats
{
    public float[] ButtonColorOffFocusArr;
    public float[] ButtonColorOnFocusArr;
    public Stats()
    {
        ButtonColorOffFocusArr = new float[4];
        ButtonColorOnFocusArr = new float[4];
    }
}