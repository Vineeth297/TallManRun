using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
	private bool _isAtExtreme;
	private void Update()
	{
		if (!_isAtExtreme)
		{
			transform.position = Vector3.MoveTowards(transform.position,transform.position + Vector3.down * 3f, Time.deltaTime * 3f);
			if (transform.position.y <= -2f)
				_isAtExtreme = true;
		}
		else 
		{
			transform.position = Vector3.MoveTowards(transform.position,transform.position + Vector3.up * 3f, Time.deltaTime * 3f);
			if (transform.position.y >= 0f)
				_isAtExtreme = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		PlayerMovementControl.Instance.MakePlayerShort(1);
		PlayerMovementControl.Instance.DeBuffThePlayer(1);
	}
}
