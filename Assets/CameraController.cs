using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform player;
	private Vector3 _playersLastPosition;
	private float _toMoveDistance;

	private void Start()
	{
		_playersLastPosition = player.position;    
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		_toMoveDistance = player.transform.position.z - _playersLastPosition.z;

		if (!(_toMoveDistance > 0f)) return;

		var moveToPosition = transform.position;
		moveToPosition = new Vector3(moveToPosition.x,moveToPosition.y ,moveToPosition.z + _toMoveDistance);
		transform.position = moveToPosition;

		_playersLastPosition = player.transform.position;
	}
}
