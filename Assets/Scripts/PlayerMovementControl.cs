using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerMovementControl : MonoBehaviour
{
	public float movementSpeed;
	public float xSpeed;
	public float xForce;
	public float leftBoundary;
	public float rightBoundary;
	public float rotationLerpValue = 0.1f;
	
	private float _swipeSpeed = 5f;

	public Transform lengthEffector;
	public Transform hipTransform;
	public Transform spineTransform;
	public Transform spineBoneParentTransform;
	public GameObject bonePrefab;

	private Animator _animator;
	private static readonly int RunningHash = Animator.StringToHash("ToRun");

	public GameObject scalingBone;

	private RopeController _ropeController;
	private bool _toGrowTall;

	private void Start()
	{
		lengthEffector.position = new Vector3(lengthEffector.position.x,lengthEffector.position.y + 1.3f, lengthEffector.position.z);
		_animator = GetComponent<Animator>();
		_ropeController = GetComponent<RopeController>();
	}

	private void Update()
	{
		
		if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

	#if UNITY_EDITOR
		xForce = Input.GetMouseButton(0) ? Input.GetAxis("Mouse X") * xSpeed : 0;
	#elif UNITY_ANDROID
        if(Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		  {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			xForce = touchDeltaPosition.x*_swipeSpeed*Mathf.Deg2Rad;
          }
		if(Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		  {
			xForce = 0;
          }
	#endif
		
		PlayerMovement();
		if(_toGrowTall)
			_ropeController.UpdateRope();
		
	}

	private void PlayerMovement()
	{
		if (Input.GetMouseButton(0))
		{
			_animator.SetBool(RunningHash, true);
			transform.Translate(
				(Vector3.forward * movementSpeed + new Vector3(xForce * xSpeed, 0f, 0f)) * Time.deltaTime, Space.World);
			lengthEffector.transform.position = new Vector3(lengthEffector.position.x,
				lengthEffector.position.y,transform.position.z);
		}
		else
		{
			_animator.SetBool(RunningHash, false);
		}

		if(transform.position.x < leftBoundary)
			transform.position = new Vector3(leftBoundary,transform.position.y,transform.position.z);
		else if (transform.position.x > rightBoundary)
			transform.position = new Vector3(rightBoundary, transform.position.y, transform.position.z);

		if (xForce < 0f)
			//rotate anticlockwise
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -90, 0), rotationLerpValue);
		else if (xForce > 0f)
			//rotate clockwise
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 90, 0), rotationLerpValue);
		else
			//face forward
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), rotationLerpValue);
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("TestGate"))
		{
			//become tall
			lengthEffector.position += Vector3.up * 3f;
			//spineTransform.position = lengthEffector.position;
			//MakePlayerTall();
			_toGrowTall = true;
			other.enabled = false;
		}
	}

	private void MakePlayerTall()
	{
		var distance = spineTransform.position.y - hipTransform.position.y;
		var transformLocalScale = scalingBone.transform.localScale;
		transformLocalScale.y *= distance;
		scalingBone.transform.localScale = transformLocalScale;
		return;


		/*for (var i = 0; i < distance; i++)
		{
			var spawnedBone = Instantiate(bonePrefab);
			spawnedBone.transform.parent = spineBoneParentTransform;
			spawnedBone.transform.position = spineBoneParentTransform.position - Vector3.up * i;
		}*/
		
	}
}
