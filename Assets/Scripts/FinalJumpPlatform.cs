using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FinalJumpPlatform : MonoBehaviour
{
	[SerializeField] private Transform jumpTarget;
	
	[SerializeField] private Transform cameraPosition;
	
	private Camera _camera;

	private void Start()
	{
		_camera = Camera.main;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			PlayerMovementControl.Instance.PlayJumpAnimation();
			PlayerMovementControl.Instance.RestrictPlayerInput();
			SoundManager.Instance.PlaySound(SoundManager.Instance.jumpSound);
			PlayerMovementControl.ExistingSequence = other.transform.DOJump(jumpTarget.position, 5f, 1, 3f)
				.OnComplete(() =>
				{
					print("Jump");
					PlayerMovementControl.Instance.StopJumpAnimation();
					PlayerMovementControl.Instance.ResetPlayerInput();
					PlayerMovementControl.Instance.PlaySprintAnimation();
					PlayerMovementControl.Instance.isAtSprintingPhase = true;
					_camera.transform.DOMoveX(cameraPosition.position.x + 2.25f, 1f);
					_camera.transform.DORotate(new Vector3(10.657f, -16.413f, 0f), 1f);
					PlayerMovementControl.Instance.toSprint = true;
					PlayerMovementControl.Instance.xSpeed = 0f;
				});
			/*
			var lEffector = other.GetComponent<PlayerMovementControl>().lengthEffector.transform;
			lEffector.DOLocalJump( jumpTarget.position + Vector3.up * lEffector.position.y, 5f, 1, 3f); //.SetEase(Ease.InCubic);
		*/
			/*other.GetComponent<PlayerMovementControl>().lengthEffector.transform
					.DOLocalJump(new Vector3(jumpTarget.position.x,other.GetComponent<PlayerMovementControl>().lengthEffector.transform.position.y,jumpTarget.position.z ), 5f, 1, 3f); //.SetEase(Ease.InCubic);
			*/
		}
	}
}
