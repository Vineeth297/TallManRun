using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform player;
	private Vector3 _playersLastPosition;
	private float _toMoveDistanceInZ;
	private float _toMoveDistanceInY;
	private float _toMoveDistanceInX;

	private void Start()
	{
		_playersLastPosition = player.position;    
	}

	// Update is called once per frame
	private void LateUpdate()
	{
		_toMoveDistanceInZ = player.transform.position.z - _playersLastPosition.z;
		_toMoveDistanceInY = player.transform.position.y - _playersLastPosition.y;
		_toMoveDistanceInX = player.transform.position.x - _playersLastPosition.x;

		if (!(_toMoveDistanceInZ > 0f)) return;

		var moveToPosition = transform.position;
		moveToPosition = new Vector3(moveToPosition.x,moveToPosition.y + _toMoveDistanceInY,moveToPosition.z + _toMoveDistanceInZ);
		
		transform.position = moveToPosition;
		
		_playersLastPosition = player.transform.position;
	}
}
