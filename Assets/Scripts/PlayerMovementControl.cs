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
	public bool toSprint;
	
	private float _swipeSpeed = 5f;

	public Transform lengthEffector;
	public Transform hipTransform;
	public Transform spineTransform;
	public GameObject head;

	private Animator _animator;
	private static readonly int RunningHash = Animator.StringToHash("ToRun");
	private static readonly int JumpingHash = Animator.StringToHash("ToJump");
	private static readonly int SkiddingHash = Animator.StringToHash("ToSkid");
	private static readonly int SprintHash = Animator.StringToHash("Sprint");
	private static readonly int FinalJumpHash = Animator.StringToHash("FinalJump");
	private static readonly int WinHash = Animator.StringToHash("FinalLanding");
	private static readonly int KickBossHash = Animator.StringToHash("KickBoss");

	public GameObject scalingBone;

	private RopeController _ropeController;
	private bool _toGrowTall;
	private bool _toShrink;
	public bool isAtSprintingPhase;

	public List<Transform> startingPositions;
	public List<Transform> endingPositions;
	public List<CylinderGeneration> cylinderList;
	public GameObject spherePrefab;
	public List<Transform> spheres;
	public List<Transform> spherePositions;

	public Material cylinderMaterial;
	public Color cylinderPositiveColor;
	public Color cylinderNegativeColor;
	
	public Color cylinderDefaultColor;

	private CylinderGeneration _spineCylinder;

	public float minimumScaleValue = 0.01f;
	public float minimumPositionDifferenceValue = 1f;

	public static Sequence ExistingSequence;
	private void Awake()
	{
		if (Instance)
			Destroy(gameObject);
		else
			Instance = this;
		
		DOTween.SetTweensCapacity(1000,30);
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
		print(_spineCylinder);
		cylinderMaterial.color = cylinderDefaultColor;

	}

	private void Update()
	{
		
		if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

//	#if UNITY_EDITOR
		xForce = Input.GetMouseButton(0) ? Input.GetAxis("Mouse X") * xSpeed : 0;
	/*#elif UNITY_ANDROID
        if(Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		  {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			xForce = touchDeltaPosition.x*_swipeSpeed*Mathf.Deg2Rad;
          }
		if(Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		  {
			xForce = 0;
          }
	#endif*/
		
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
		if (toSprint)
		{
			_animator.SetTrigger(SprintHash);
			transform.Translate(
				(Vector3.forward * movementSpeed + new Vector3(xForce * xSpeed, 0f, 0f))
				* Time.deltaTime, Space.World);
		}
		else if (Input.GetMouseButton(0))
		{
			_animator.SetBool(RunningHash, true);
			transform.Translate(
				(Vector3.forward * movementSpeed + new Vector3(xForce * xSpeed, 0f, 0f)) 
				* Time.deltaTime, Space.World);
			/*lengthEffector.transform.Translate(
				(Vector3.forward * movementSpeed + new Vector3(xForce * xSpeed, 0f, 0f)) * Time.deltaTime, Space.Self);  
		*/
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
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -90, 0), rotationLerpValue * Time.deltaTime);
		else if (xForce > 0f)
			//rotate clockwise
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 90, 0), rotationLerpValue * Time.deltaTime);
		else
			//face forward
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), rotationLerpValue * Time.deltaTime);
		
	}

	public void MakeThePlayerTall(int factor)
	{
		MoveTheTorsoUp(factor);
		_toGrowTall = true;
	}

	public void MakePlayerShort(int factor)
	{
		MoveTheTorsoDown(factor);
		_toGrowTall = false;
		_toShrink = true;
	}

	public void BuffThePlayer(int factor)
	{
		ChangeToPositiveColor();
		var buffValue = factor * 0.1f;
		print(buffValue);
		for (var i = 0; i < startingPositions.Count; i++)
		{
			var cylinderInitialScale = cylinderList[i].transform.localScale;
			var sphereInitialScale = spheres[i].transform.localScale;
			cylinderList[i].gameObject.transform.DOScale(cylinderInitialScale + new Vector3(buffValue, buffValue, 0), 2f);
			spheres[i].transform.DOScale(sphereInitialScale + Vector3.one * buffValue, 2f);
		}
		for (var i = 0; i < spheres.Count; i++)
		{
			var sphereInitialScale = spheres[i].transform.localScale;
			spheres[i].transform.DOScale(sphereInitialScale + Vector3.one * buffValue, 2f);
		}

		var spineCylinder = _spineCylinder.transform.localScale;
		_spineCylinder.transform.DOScale(new Vector3( buffValue, buffValue,0) + spineCylinder, 2f).OnComplete(ChangeToDefaultColor);
	}

	public void DeBuffThePlayer(int factor)
	{
		
		var buffValue = factor * 0.1f;
		var spineCylinder = _spineCylinder.transform.localScale;
		print(spineCylinder);
		if (MinimumScaleThresholdHasCrossed(spineCylinder, buffValue))
		{
			print("GameOver");
			return;
		}
		
		_spineCylinder.transform.DOScale(spineCylinder - new Vector3( buffValue, buffValue,0), 0.7f);
		
		for (var i = 0; i < cylinderList.Count; i++)
		{
			var cylinderInitialScale = cylinderList[i].transform.localScale;
			// if(_lastScalingCylinderTweens[i].IsActive()) _lastScalingCylinderTweens[i].Kill();
			// _lastScalingCylinderTweens[i] = 
			cylinderList[i].transform.DOScale(cylinderInitialScale - new Vector3(buffValue, buffValue, 0), 0.7f);
		}

		for (var i = 0; i < spherePositions.Count; i++)
		{
			var sphereInitialScale = spheres[i].transform.localScale;
			// if(_lastScalingSphereTweens[i].IsActive()) _lastScalingSphereTweens[i].Kill();
			// _lastScalingSphereTweens[i] = 
			spheres[i].transform.DOScale(sphereInitialScale - Vector3.one * buffValue, 1f);
		}
	}

	public void ShrinkThePlayer(int factor)
	{
		DeBuffThePlayer(factor);
		MakePlayerShort(factor);
	}

	private void MoveTheTorsoUp(int factor)
	{
		ChangeToPositiveColor();
		var buffValue = factor * 0.1f;
		lengthEffector.transform.DOMoveY(lengthEffector.position.y + buffValue, 1f).SetEase(Ease.OutExpo);
		_toGrowTall = false;
		Invoke(nameof(ChangeToDefaultColor),0.5f);
	}

	private void MoveTheTorsoDown(int factor)
	{		
		var buffValue = factor * 0.1f;
		if (MinimumHeightThresholdHasCrossed(lengthEffector.transform.position, buffValue))
		{
			print("GameOver");
			return;
		}
		else
			lengthEffector.transform.DOMoveY(lengthEffector.position.y - buffValue, 0.7f).SetEase(Ease.OutExpo);
		_toShrink = false;
	}

	private void GenerateCylinders()
	{
		var maxIterations = startingPositions.Count;
		for (var i = 0; i < maxIterations; i++)
		{
			_ropeController.UpdateRope(startingPositions[i], endingPositions[i], cylinderList[i]);
		//	print(i);
		}

		foreach (var position in spherePositions)
		{
			var sphere = Instantiate(spherePrefab, position);
			spheres.Add(sphere.transform);
		}
	}

	private bool MinimumScaleThresholdHasCrossed(Vector3 spineCylinder, float variableScaleValue)
	{
		var scale = spineCylinder.x - variableScaleValue;
		print(scale);
		
		var x = scale <= minimumScaleValue;
		if (x)
		{
			ResetPlayerInput();
			DisappearAllTheCylinders();
			head.transform.parent = null;
			head.GetComponent<Rigidbody>().isKinematic = false;
			head.SetActive(true);
			if (!isAtSprintingPhase)
			{
				GameManager.Instance.ShowLosePanel();
			}
			else if (isAtSprintingPhase)
			{
				GameManager.Instance.ShowWinPanel();

			}
			return x;
		}
		else
		{
			print(scale <= minimumScaleValue);
			return x;
		}
	}

	private bool MinimumHeightThresholdHasCrossed(Vector3 lengthEffectorPosition, float variablePositionValue)
	{
		var yPos = lengthEffectorPosition.y - variablePositionValue;
		return yPos <= minimumPositionDifferenceValue;
	}
	public void PlayJumpAnimation()
	{
		_animator.SetBool(JumpingHash, true);
	}

	public void StopJumpAnimation()
	{
		_animator.SetBool(JumpingHash, false);
	}

	public void PlaySprintAnimation()
	{
		_animator.SetTrigger(SprintHash);
		xSpeed = 0f;
		movementSpeed = 5f;
	}
	
	public void PlayKickAnimation()
	{
		_animator.SetTrigger(KickBossHash);
	}

	public void PlayWinAnimation()
	{
		_animator.SetTrigger(WinHash);
	}

	public void RestrictPlayerInput()
	{
		xSpeed = 0f;
		movementSpeed = 0f;
	}

	public void ResetPlayerInput()
	{
		xSpeed = 1.75f;
		movementSpeed = 4.25f;
	}

	public void DisappearAllTheCylinders()
	{
		var maxIterations = startingPositions.Count;
		for (var i = 0; i < maxIterations; i++) cylinderList[i].gameObject.SetActive(false);

		foreach (var sphere in spheres) sphere.gameObject.SetActive(false);
	}

	public void JumpKarao(Vector3 jumpTargetPosition)
	{
		SoundManager.Instance.PlaySound(SoundManager.Instance.finalJumpSound);
		transform.DOJump(jumpTargetPosition, 5f, 1, 3f)
			.OnComplete(() =>
			{
				toSprint = false;
				RestrictPlayerInput();
				transform.DOMove(new Vector3(transform.position.x, 0f, transform.position.z), 1f).OnComplete(
					() =>
					{
						PlayWinAnimation();
						GameManager.Instance.ShowWinPanel();
					});
			});
	}

	public void ChangeToPositiveColor()
	{
		cylinderMaterial.color = cylinderPositiveColor;
	}

	public void ChangeToDefaultColor()
	{
		cylinderMaterial.color = cylinderDefaultColor;
	}
}
