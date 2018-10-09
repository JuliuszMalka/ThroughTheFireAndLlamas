using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

	public float gameTimer = 600f; 
	public static int levelTag = 0;
	public string[] scenes = null;
	public GameObject[] obstacles = null; //przeszkody jak np. beczki, skrzynie itp.
	public bool gameStarted = false;

	void Update() {
		if (gameStarted && gameTimer > 0f) {
			gameTimer -= Time.deltaTime;
		}
	}

	public void ChangeLevel() {
		++levelTag;
		SceneManager.LoadScene(scenes[Random.Range(0, scenes.Length)]);
		GenerateLevelObjects();
	}

	public void GenerateLevelObjects() {
		GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("ObstacleSpawnPoint");
		foreach (GameObject spawnPoint in spawnPoints) {
			int randomChance = (int)(Random.value * 100f);
			if (randomChance <= 40) {
				Instantiate(obstacles[Random.Range(0, obstacles.Length)], spawnPoint.transform.position, Quaternion.identity);
			}
		}
	}

	public void FinalMatch() {

	}
}
