using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance;

	public AudioSource miscSounds;
	public AudioClip levelWinSound;
	public AudioClip levelFailSound;
	public AudioClip multiplierAdditionSound;
	public AudioClip multiplierSubractionSound;
	public AudioClip bossKickSound;
	public AudioClip jumpSound;
	public AudioClip obstacleHitSound;
	public AudioClip playerFallingDown;
	public AudioClip movingObstacleHitSound;
	public AudioClip pickUpSound;
	public AudioClip finalJumpSound;

	private void Awake()
	{
		if (Instance)
			Destroy(gameObject);
		else
			Instance = this;
	}

	public void PlaySound(AudioClip audioClip) => miscSounds.PlayOneShot(audioClip);
}
