using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LightningArc : MonoBehaviour {

	public Transform player = null;
	public int maxChainCount = 3;

	void Awake() {
		player = GameObject.FindWithTag("Player").transform;
	}

	void Update() {
		Debug.DrawRay(this.transform.position, gameObject.GetComponent<PlayerBehaviour>().lookDirection.normalized * 4f, Color.red, 5f);
	}

	

	// GameObject[] CheckForEnemies() {
	// 	Vector3 checkLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	// 	checkLocation.y = 0f;
	// 	float range = 40f;
	// 	LayerMask temp = -1;
	// 	Collider[] caughtObjects = Physics.OverlapSphere(
	// 		player.position,
	// 		range,
	// 		temp,
	// 		QueryTriggerInteraction.UseGlobal
	// 	);
	// 	if (caughtObjects.Length <= 0) return null;
	// 	List<GameObject> enemies = caughtObjects.
	// 		Where(obj => obj.gameObject.CompareTag("Enemy") || obj.gameObject.CompareTag("Player")).
	// 		Select(x => x.gameObject).
	// 		ToList();

	// 	List<GameObject> closestEnemies = new List<GameObject>();
	// 	for (int i = 0; i < (enemies.Count >= maxChainCount ? maxChainCount : enemies.Count); i = 0) {
	// 		GameObject tempObj = FindMinDistancedObject(player, enemies);
	// 		closestEnemies.Add(tempObj);
	// 		enemies.Remove(tempObj);
	// 	}
	// 	return closestEnemies.ToArray();
	// }

	// GameObject FindMinDistancedObject(Transform origin, List<GameObject> list) {
	// 	if (list.Count <= 0) return null;
	// 	if (list.Count == 1) return list[0];
	// 	float minDist = Vector3.Distance(origin.position, list[0].transform.position);
	// 	GameObject tempObj = list[0];
	// 	for (int i = 1; i < (list.Count >= maxChainCount ? maxChainCount : list.Count); ++i) {
	// 		float temp = Vector3.Distance(origin.position, list[i].transform.position);
	// 		if (temp < minDist) {
	// 			temp = minDist;
	// 			tempObj = list[i];
	// 		}
	// 	}
	// 	return tempObj;
	// }

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
	}
}
