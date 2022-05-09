using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementControl : MonoBehaviour
{
	public static PlayerMovementControl Instance;
	
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

	private Animator _animator;
	private static readonly int RunningHash = Animator.StringToHash("ToRun");

	public GameObject scalingBone;

	private RopeController _ropeController;
	private bool _toGrowTall;
	private bool _toShrink;

	public List<Transform> startingPositions;
	public List<Transform> endingPositions;
	public List<CylinderGeneration> cylinderList;
	public GameObject spherePrefab;
	public List<Transform> spheres;
	
	private CylinderGeneration _spineCylinder;

	private void Awake()
	{
		if (Instance)
			Destroy(gameObject);
		else
			Instance = this;
	}

	private void Start()
	{
		//lengthEffector.position = new Vector3(lengthEffector.position.x,lengthEffector.position.y + 1.1f, lengthEffector.position.z);
		_animator = GetComponent<Animator>();
		_ropeController = GetComponent<RopeController>();
		spheres = new List<Transform>();
		//GenerateAllTheCylinders
		GenerateCylinders();

		_spineCylinder = scalingBone.GetComponent<CylinderGeneration>();
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
		if (_toGrowTall)
		{
			_ropeController.UpdateRope(hipTransform.transform,spineTransform.transform,_spineCylinder);
		}

		if (_toShrink)
		{
			_ropeController.UpdateRope(hipTransform.transform,spineTransform.transform,_spineCylinder);

		}
			
	}

	private void PlayerMovement()
	{
		if (Input.GetMouseButton(0))
		{
			_animator.SetBool(RunningHash, true);
			transform.Translate(
				(Vector3.forward * movementSpeed + new Vector3(xForce * xSpeed, 0f, 0f)) * Time.deltaTime, Space.World);
			/*lengthEffector.transform.Translate(
				(Vector3.forward * movementSpeed + new Vector3(xForce * xSpeed, 0f, 0f)) * Time.deltaTime, Space.Self);  */
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
		if (other.CompareTag("TallGate"))
		{
			MakeThePlayerTall();
		}
		else if (other.CompareTag("BuffGate"))
		{
			BuffThePlayer();
		}
		else if (other.CompareTag("ShortGate"))
		{
			MakePlayerShort();
		}
		
		other.enabled = false;
	}

	public void MakeThePlayerTall()
	{
		MoveTheTorsoUp();
		_toGrowTall = true;
	}

	public void MakePlayerShort()
	{
		MoveTheTorsoDown();
		_toGrowTall = false;
		_toShrink = true;
	}

	public void BuffThePlayer()
	{
		for (var i = 0; i < startingPositions.Count; i++)
		{
			var cylinderInitialScale = cylinderList[i].transform.localScale;
			var sphereInitialScale = spheres[i].transform.localScale;
			cylinderList[i].gameObject.transform.DOScale(cylinderInitialScale + new Vector3(2f, 2f, 0), 2f);
			spheres[i].transform.DOScale(sphereInitialScale + Vector3.one * 2f, 2f);
		}

		var spineCylinder = _spineCylinder.transform.localScale;
		_spineCylinder.transform.DOScale(new Vector3( 2f, 2f,0) + spineCylinder, 2f);
	}

	public void DeBuffThePlayer()
	{
		for (var i = 0; i < startingPositions.Count; i++)
		{
			var cylinderInitialScale = cylinderList[i].transform.localScale;
			var sphereInitialScale = spheres[i].transform.localScale;
			cylinderList[i].gameObject.transform.DOScale(cylinderInitialScale - new Vector3(1.25f, 1.25f, 0), 2f);
			spheres[i].transform.DOScale(sphereInitialScale - Vector3.one * 1.25f, 2f);
		}

		var spineCylinder = _spineCylinder.transform.localScale;
		_spineCylinder.transform.DOScale(spineCylinder - new Vector3( 1.25f, 1.25f,0), 2f);
	}

	public void ShrinkThePlayer()
	{
		DeBuffThePlayer();
		ShrinkThePlayer();
	}

	private void MoveTheTorsoUp()
	{
		lengthEffector.transform.DOMoveY(lengthEffector.position.y + 3f, 2f).SetEase(Ease.OutExpo);
		_toGrowTall = false;
	}

	private void MoveTheTorsoDown()
	{
		lengthEffector.transform.DOMoveY(lengthEffector.position.y - 1.5f, 2f).SetEase(Ease.OutExpo);
		_toShrink = false;
	}

	private void GenerateCylinders()
	{
		var maxIterations = startingPositions.Count;
		for (var i = 0; i < maxIterations; i++)
		{
			var sphere = Instantiate(spherePrefab, startingPositions[i]);
			spheres.Add(sphere.transform);
			_ropeController.UpdateRope(startingPositions[i], endingPositions[i], cylinderList[i]);
			print(i);
		}
	}
}
