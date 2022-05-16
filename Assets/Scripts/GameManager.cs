using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public GameObject winPanel;
	public GameObject losePanel;

	public float winPanelDelayInSeconds = 1.5f;
	public float losePanelDelayInSeconds = 1.5f;

	public TMP_Text diamondCountText;
	public int diamondCount;
	public TMP_Text levelNumberText;
	private void Awake()
	{
		if (Instance)
			Destroy(gameObject);
		else
			Instance = this;
	}

	private void Start()
	{
		levelNumberText.text = "LEVEL " + (SceneManager.GetActiveScene().buildIndex + 1);
	}
	public void ShowWinPanel()
	{
		Invoke(nameof(WinPanel), winPanelDelayInSeconds);
	}
	
	public void ShowLosePanel()
	{
		Invoke(nameof(LosePanel), losePanelDelayInSeconds);
	}
	
	private void WinPanel()
	{
		winPanel.SetActive(true);
	}

	private void LosePanel()
	{
		losePanel.SetActive(true);
	}
	
	public void NextButton()
	{
		if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCount)
		{
			SceneManager.LoadScene(Random.Range(1, SceneManager.sceneCount));
		}
		else
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void ReloadButton() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

	public void PickUpTheDiamond()
	{
		diamondCount += 1;
		diamondCountText.text = diamondCount.ToString();
	}
}
