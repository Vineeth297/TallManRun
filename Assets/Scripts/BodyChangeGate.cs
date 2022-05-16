using System;
using TMPro;
using UnityEngine;

public class BodyChangeGate : MonoBehaviour
{
	private PlayerMovementControl _player;
	public enum ChangeType
	{
		Tall,
		Shrink,
		Fat,
		Skinny
	}

	public GameObject tallSprite;
	public GameObject shortSprite;
	public GameObject fatSprite;
	public GameObject skinnySprite;

	public MeshRenderer meshRenderer;
	
	public ChangeType changeType = ChangeType.Tall;
	public TMP_Text text;
	public int factor;

	private bool _hasBeenUsed;

	public Color positiveColor;
	public Color negativeColor;

	private SoundManager _sound;
	

	private void Start()
	{
		_sound = SoundManager.Instance;
		
		_player = PlayerMovementControl.Instance;
		if (changeType == ChangeType.Fat || changeType == ChangeType.Tall)
		{
			meshRenderer.material.color = positiveColor;
			transform.GetChild(0).GetComponent<MeshRenderer>().material.color = positiveColor;
		}
		else
		{
			meshRenderer.material.color = negativeColor;
			transform.GetChild(0).GetComponent<MeshRenderer>().material.color = negativeColor;
		}

	}
	private void OnTriggerEnter(Collider other)
	{
		if (_hasBeenUsed) return;

		if (other.CompareTag("Player"))
		{
			switch (changeType)
			{
				case ChangeType.Tall:
				{
					_player.MakeThePlayerTall(factor);
					_sound.PlaySound(_sound.multiplierAdditionSound);
					break;
				}
				case ChangeType.Shrink:
				{
					_sound.PlaySound(_sound.multiplierSubractionSound);
					_player.MakePlayerShort(factor);
					break;
				}
				case ChangeType.Fat:
				{
					_sound.PlaySound(_sound.multiplierAdditionSound);
					_player.BuffThePlayer(factor);
					break;
				}
				case ChangeType.Skinny:
				{
					_sound.PlaySound(_sound.multiplierSubractionSound);
					_player.DeBuffThePlayer(factor);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		_hasBeenUsed = true;
	}

	private void OnValidate()
	{
		if (changeType == ChangeType.Tall)
		{
			tallSprite.SetActive(true);
			shortSprite.SetActive(false);
			fatSprite.SetActive(false);
			skinnySprite.SetActive(false);
			name = text.text = " + " + factor;
		//	meshRenderer.material.color = Color.HSVToRGB(6, 197, 255);

		}
		else if (changeType == ChangeType.Shrink)
		{
			tallSprite.SetActive(false);
			shortSprite.SetActive(true);
			fatSprite.SetActive(false);
			skinnySprite.SetActive(false);
			name = text.text = " - " + factor;
			//meshRenderer.material.color = Color.red;
		}
		else if (changeType == ChangeType.Fat)
		{
			tallSprite.SetActive(false);
			shortSprite.SetActive(false);
			fatSprite.SetActive(true);
			skinnySprite.SetActive(false);
			name = text.text = " + " + factor;
		//	meshRenderer.material.color = Color.HSVToRGB(6, 197, 255);
		}
		else if (changeType == ChangeType.Skinny)
		{
			tallSprite.SetActive(false);
			shortSprite.SetActive(false);
			fatSprite.SetActive(false);
			skinnySprite.SetActive(true);
			name = text.text = " - " + factor;
			//meshRenderer.material.color = Color.red;
		}
		else
		{
			name = name;
		}
	}
}
