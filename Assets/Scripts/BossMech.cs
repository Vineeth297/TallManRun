using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMech : MonoBehaviour
{
	
	[SerializeField] private Animator bossAnimator;
	private static readonly int FallingHash = Animator.StringToHash("Fall");
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			SoundManager.Instance.PlaySound(SoundManager.Instance.bossKickSound);
			bossAnimator.SetTrigger(FallingHash);
		}
	}
}
