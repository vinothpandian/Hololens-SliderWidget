using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragButton : MonoBehaviour {

	[SerializeField]
	float DragSpeed = 1.5f;

	[SerializeField]
	float DragScale = 1.5f;

	[SerializeField]
	float MaxDragDistance = 3f;

	private Rigidbody rb;

	Vector3 lastPosition;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	public void DragStart(Vector3 position)
	{
		lastPosition = transform.position;	
	}

	public void Drag(Vector3 position)
	{
		var targetPosition = lastPosition + position * DragScale;
		if (Vector3.Distance(lastPosition, targetPosition) <= MaxDragDistance)
		{
			rb.velocity = (targetPosition	- transform.position) * DragSpeed;
		}
	}
		
}
