using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

	public float gameTimer = 120f; 
	public static int levelTag = 0;
	public string[] scenes = null;
    public bool gameStarted = false;
    public GameObject[] obstaclesPrefabs = null; //przeszkody jak np. beczki, skrzynie itp.
	public GameObject[] skillEffectsPrefabs = null;
    public GameObject[] worldMapMatrix = null;

    private void Start()
    {
        //filling scene list
        //int sceneCount = SceneManager.sceneCountInBuildSettings;
        //scenes = new string[sceneCount];
        //for( int i = 0; i < sceneCount; ++i) scenes[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
    }

    void Update() {
		if (gameStarted && gameTimer > 0f) {
			gameTimer -= Time.deltaTime;
            if(gameTimer <= 0f)
            {
                FinalMatch();
            }
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
				Instantiate(obstaclesPrefabs[Random.Range(0, obstaclesPrefabs.Length)], spawnPoint.transform.position, Quaternion.identity);
			}
		}
	}

	public void FinalMatch()
    {
        Debug.Log("It's a final Countdown!");
	}
}
