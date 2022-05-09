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
			other.transform.DOJump(jumpTarget.position, 5f, 1, 3f);
			// other.GetComponent<PlayerMovementControl>().lengthEffector.transform.DOJump(jumpTarget.position, 5f, 1, 3f).SetEase(Ease.InCubic);
		}
	}
}
