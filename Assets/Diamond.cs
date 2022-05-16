using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Collector"))
		{
			
			GameManager.Instance.PickUpTheDiamond();
			gameObject.SetActive(false);			
		}

	}
}
