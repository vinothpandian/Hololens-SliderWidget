using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveButton : MonoBehaviour {
	public float thrust;
	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		float xValue = Input.GetAxis ("Horizontal") * thrust;
			
		//Vector3 movement = new Vector3 (xValue * thrust, 0.0f, 0.0f);

		rb.AddForce(transform.forward * xValue);

		Debug.Log (xValue);
	}
}
