using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// TubeSliderManager class provides customizability to the slider widget and implements the logic behind slider movement
/// </summary>
public class TubeSliderManager : MonoBehaviour
{
    // Stores Slider Minimium value and its label
    [SerializeField]
    public uint SliderMinimumValue = 0;
    public string SliderMinimumLabel;

    // Stores Slider Maximum value and its label
    [SerializeField]
    public uint SliderMaximumValue = 100;
    public string SliderMaximumLabel;

    // Stores the current value of the slider button position
    public uint CurrentValue = 50;

    // Set it as false to the current value of slider on top of the slider button
    public bool isCurrentValueToBeDisplayed = true;

    // Stores the button's basic color and its color on focus and off focus
    public Stats ButtonColor;
    public Color ButtonColorOffFocus;
    public Color ButtonColorOnFocus;

    // Rigidbody object to manipulate slider button value
    private Rigidbody rb;

    // Gameobjects declaration and theri label
    private GameObject leftHolder;
    private GameObject rightHolder;
    private GameObject button;
    private GameObject buttonPivot;
    private string leftLabel;
    private string rightLabel;
    private string buttonLabel;

    // Checks whether the slider 
    private bool isSliderManipulationTriggered;

    // Slider range as per user definition
    private uint SliderRange;

    // variables for calculation of slider position
    private Vector3 start;
    private Vector3 end;
    private Vector3 sliderVector;
    private Vector3 prevPosition;
    private Vector3 movementDistance;
    private Vector3 newPosition;
    private float angleMinBound;
    private float angleMaxBound;
    private Vector3 newPositionVector;
    private float diff;


    /// <summary>
    /// Awake function is automatically called when the object is instantiated.
    /// </summary>
    void Awake()
    {
        // Set the slider manipulation as false
        isSliderManipulationTriggered = false;

        // Loop throught the children of slider and select the left and right holder game objects and slider button game object
        foreach (Transform child in transform)
        {
           
            switch(child.tag)
            {
                case "LeftHolder": leftHolder = child.gameObject;
                    break;
                case "RightHolder":
                    rightHolder = child.gameObject;
                    break;
                case "SliderButton":
                    button = child.gameObject;
                    break;
            }
        }

        // Calculate the slider range in user defined range
        SliderRange = SliderMaximumValue - SliderMinimumValue;

        // set the end points slider button position from left and right holder values
        start = leftHolder.transform.position;
        end = rightHolder.transform.position;

        // Vector that represents the slider (magnitude is size)
        sliderVector = end - start;

        // Calculate the difference between button's real center and the button's left most point
        diff = button.GetComponent<BoxCollider>().bounds.size.x;

        // assign the button position in slider to the user defined current value
        button.transform.position = start + (-button.transform.up.normalized * (((float)CurrentValue / (float)SliderRange)) * (sliderVector.magnitude));
  
        // render the color of button to Button off color defined by user
        button.GetComponent<Renderer>().material.color = ButtonColorOffFocus;

    }


    /// <summary>
    /// Creates color picker in unity inspector to get button off focus color
    /// </summary>
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

    /// <summary>
    /// Creates color picker in unity inspector to get button on focus color
    /// </summary>
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

    /// <summary>
    /// Function is called when the manipulation event starts
    /// </summary>
    public void ManipulationStarted(ManipulationEventData eventData)
    {
        // Start manipulation only if the slider manipulation is already not triggered
        if (!isSliderManipulationTriggered)
        {
            // set the button color to on focus color
            button.GetComponent<Renderer>().material.color = ButtonColorOnFocus;

            // Push the button instance to HoloToolkit InputManager to get the hololens inputs
            InputManager.Instance.PushModalInputHandler(button);

            // get the rigidbody object of the button
            rb = button.GetComponent<Rigidbody>();

            // save the position of the button as previous position
            prevPosition = button.transform.position;

            // Display the slider labels
            setDisplay(SliderMinimumLabel, SliderMaximumLabel, CurrentValue.ToString());

            // Set the slider manipulation event triggered as true
            isSliderManipulationTriggered = true;
        }
    }

    /// <summary>
    /// Function is called when the manipulation event is in process
    /// </summary>
    public void ManipulationUpdated(ManipulationEventData eventData)
    {
        // Check whether the manipulation event is going on
        if (isSliderManipulationTriggered)
        {
            // Keep the button color as on focus color
            button.GetComponent<Renderer>().material.color = ButtonColorOnFocus;

            // Find the button movement by projecting the delta vector provided by HoloToolkit and the slider vector and calculate the new position
            movementDistance = Vector3.Project(eventData.CumulativeDelta, sliderVector.normalized);
            newPosition = prevPosition + movementDistance;

            // Change the new position as a vector
            newPositionVector = newPosition - start;

            // Calculate the angles between the button's new position and the slider start and end. This determines the position of button is to the left or right of the slider
            angleMinBound = AngleDir(transform.forward, newPositionVector, transform.up);
            angleMaxBound = AngleDir(transform.forward, end - newPosition, transform.up);

            // If the button is not to the left of start or not to the right of end the update the button position in the slider else skip
            if (angleMinBound != -1f && angleMaxBound != -1f)
            {
                button.transform.position = newPosition;
                CurrentValue = (uint)Mathf.RoundToInt((float)SliderRange * ((newPositionVector.magnitude) / (sliderVector.magnitude)));
            }

            // Display the slider labels
            setDisplay(SliderMinimumLabel, SliderMaximumLabel, CurrentValue.ToString());
        }

    }

    /// <summary>
    /// Function is called when the manipulation event ends
    /// </summary>
    public void ManipulationCompleted(ManipulationEventData eventData)
    {
        // Check whether the manipulation event was in process
        if (isSliderManipulationTriggered)
        {
            // set the button color to off focus color and stop displaying the slider labels
            button.GetComponent<Renderer>().material.color = ButtonColorOffFocus;
            setDisplay("", "", "");

            // Release the button object from input handler
            InputManager.Instance.PopModalInputHandler();

            // Assign the slider manipulation as false
            isSliderManipulationTriggered = false;
        }

    }

    /// <summary>
    /// Function is called if the manipulation is cancelled as hololens loses track of hand
    /// </summary>
    public void ManipulationCanceled(ManipulationEventData eventData)
    {
        if (isSliderManipulationTriggered)
        {
            // set the button color to off focus color and stop displaying the slider labels
            button.GetComponent<Renderer>().material.color = ButtonColorOffFocus;
            setDisplay("", "", "");

            // Release the button object from input handler
            InputManager.Instance.PopModalInputHandler();

            // Assign the slider manipulation as false
            isSliderManipulationTriggered = false;
        }
    }

    /// <summary>
    /// Function is called if the button is on focus. This sets the color and displays the current value of the slider
    /// </summary>
    public void ButtonOnFocus()
    {
        button.GetComponent<Renderer>().material.color = ButtonColorOnFocus;
        if (isCurrentValueToBeDisplayed)
        {
            button.GetComponentInChildren<TextMesh>().text = CurrentValue.ToString();
        }

    }

    /// <summary>
    /// Function is called if the button is on focus. This sets the color as off focus and removes the current value of the slider display
    /// </summary>
    public void ButtonOffFocus()
    {
        if (!isSliderManipulationTriggered)
        {
            button.GetComponent<Renderer>().material.color = ButtonColorOffFocus;

        }

        button.GetComponentInChildren<TextMesh>().text = "";
    }

    /// <summary>
    /// Function takes minimum, maximum and current value as strings and displays it on top of left, right and button holders
    /// </summary>
    public void setDisplay(string min, string max, string current)
    {
        if(isCurrentValueToBeDisplayed)
        {
            leftHolder.transform.parent.GetComponentInChildren<TextMesh>().text = min;
            rightHolder.transform.parent.GetComponentInChildren<TextMesh>().text = max;
            button.GetComponentInChildren<TextMesh>().text = current;
        }
        
    }


    /// <summary>
    /// Function provided by Unity community to find whether a point lies to the left or right of another point
    /// Link: https://forum.unity3d.com/threads/left-right-test-function.31420/
    /// </summary>
    /// <param name="fwd">Forward vector</param>
    /// <param name="targetDir">Target point</param>
    /// <param name="up">Up Vector</param>
    /// <returns></returns>
    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }
}


/// <summary>
/// Creates color picker in unity inspector to get button on focus color
/// </summary>
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