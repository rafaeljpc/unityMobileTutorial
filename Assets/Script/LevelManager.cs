using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	private static LevelManager instance;
	public static LevelManager Instance{get{return instance; }}

	public GameObject pauseMenu;

	private float startTime;
	public float silverTime;
	public float goldTime;

	public void TogglePauseMenu() {
		pauseMenu.SetActive (!pauseMenu.activeSelf);
	}

	public void ToMenu() {
		SceneManager.LoadScene ("MainMenu");
	}

	void Start() {
		instance = this;
		pauseMenu.SetActive (false);
		startTime = Time.time;
	}

	public void Victory() {
		float duration = Time.time - startTime;

		if (duration < goldTime) {
			GameManager.Instance.currency += 50;
		}
		else if (duration < silverTime) {
			GameManager.Instance.currency += 25;
		}
		else {
			GameManager.Instance.currency += 10;
		}
		GameManager.Instance.Save ();

		string saveString = "";
		LevelData ld = new LevelData (SceneManager.GetActiveScene ().name);
		if (ld.BestTime < duration || ld.BestTime == 0.0f)
			saveString += ld.BestTime.ToString ();
		else
			saveString += duration.ToString ();
		saveString += "&" + silverTime.ToString ();
		saveString += "&" + goldTime.ToString ();
		PlayerPrefs.SetString (SceneManager.GetActiveScene ().name, saveString);
	}
}
