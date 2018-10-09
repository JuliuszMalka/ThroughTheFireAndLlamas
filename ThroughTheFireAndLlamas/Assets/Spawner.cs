using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject itemToSpawn = null;
	private WaitForSeconds waitTime = new WaitForSeconds(0.5f);

	// Use this for initialization
	void Start () {
		StartCoroutine("ObjectSpawner");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator ObjectSpawner() {
		Vector3 temp;
		while (true) {
			temp = new Vector3(this.transform.position.x + Random.Range(2f, 4f), 0f, this.transform.position.z + Random.Range(2f, 4f));
			Instantiate(itemToSpawn, temp, Quaternion.identity);
			yield return waitTime;
		}
	}
}
