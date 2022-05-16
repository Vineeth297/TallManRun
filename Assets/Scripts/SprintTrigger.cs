using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SprintTrigger : MonoBehaviour
{
	[SerializeField] private Transform cameraPosition;
	
	private Camera _camera;

	private void Start()
	{
		_camera = Camera.main;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		
		print("Sprint");
		PlayerMovementControl.Instance.PlaySprintAnimation();
		_camera.transform.DOMove(cameraPosition.position, 1f);
		_camera.transform.DORotate(new Vector3(10.657f, -16.413f, 0f), 1f);
		PlayerMovementControl.Instance.toSprint = true;
		PlayerMovementControl.Instance.xSpeed = 0f;
	}
}
