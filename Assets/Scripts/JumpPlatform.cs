using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
	[SerializeField] private Transform jumpTarget;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			PlayerMovementControl.Instance.PlayJumpAnimation();
			PlayerMovementControl.Instance.RestrictPlayerInput();
			other.transform.DOJump(jumpTarget.position, 5f, 1, 3f)
				.OnComplete(() =>
				{
					print("Jump");
					PlayerMovementControl.Instance.StopJumpAnimation();
					PlayerMovementControl.Instance.ResetPlayerInput();
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
