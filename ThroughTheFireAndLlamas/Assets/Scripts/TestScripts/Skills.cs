using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Skills : MonoBehaviour{

	public float fireballRange = 0f;
	public float fireballDamage = 0f;
	public float arcDamage = 0f;
	public const int maxChainCount = 3;
	public Transform player = null;
	public GameObject fireballPrefab = null;

	void Awake() {
		player = this.transform;
	}

	public void ShootFireball() {
		Vector3 temp = player.gameObject.GetComponent<PlayerBehaviour>().lookDirection.normalized;
		float angle = Mathf.Atan2(temp.z, temp.x);
		Instantiate(fireballPrefab, this.transform.position, Quaternion.AngleAxis(angle, Vector3.up));
	}

	public void Explode() {
		LayerMask temp = -1;
		Collider[] caughtItems = Physics.OverlapSphere(
			this.transform.position,
			this.fireballRange,
			temp
		);

		caughtItems.
			Where(obj => obj.gameObject.CompareTag("Enemy") || obj.gameObject.CompareTag("Player")).
			ToList().
			ForEach(character => {
				if (character.gameObject.CompareTag("Player") && !PlayerStatistics.GetInstance().flags["isHit"]) {
					PlayerStatistics.GetInstance().ReduceHealth(this.fireballDamage);
					PlayerStatistics.GetInstance().ToggleFlag("isHit", true);
					PlayerStatistics.GetInstance().SetTimer("hit");
				} else if (character.gameObject.CompareTag("Enemy")) {
					/*zmniejszyć hp przeciwnika*/
				}
			});

	}

	public void CastLightning() {
		/*metoda do wrzucenia w dszefo*/
		GetClosestEnemies().ForEach(enemy => {
			if (enemy.gameObject.CompareTag("Player") && !PlayerStatistics.GetInstance().flags["isHit"]) {
				PlayerStatistics.GetInstance().ReduceHealth(this.arcDamage);
				PlayerStatistics.GetInstance().ToggleFlag("isHit", true);
				PlayerStatistics.GetInstance().SetTimer("hit");
			} else if (enemy.gameObject.CompareTag("Enemy")) {
				/*redukcja życia u przeciwników*/ 
			}
		});
		PlayerStatistics.GetInstance().SetTimer("arc");
	}

	public List<GameObject> GetClosestEnemies() {
		Vector3 frontOfPlayer = gameObject.GetComponent<PlayerBehaviour>().lookDirection.normalized;
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		if (enemies == null) {
			enemies = GameObject.FindGameObjectsWithTag("Player");
		}

		List<GameObject> frontEnemies = enemies.
			Where(enemy => Vector3.Dot(frontOfPlayer.normalized, enemy.transform.position.normalized) < 0f).
			ToList();
		List<GameObject> closestEnemies = new List<GameObject>();
		for (int i = 0; i < (frontEnemies.Count >= 3 ? 3 : frontEnemies.Count); ++i) {
			closestEnemies.Add(GetMinDistance(player, frontEnemies));
		}
		return closestEnemies;
	}

	public GameObject GetMinDistance(Transform origin, List<GameObject> list) {
		if (list.Count <= 0) return null;
		if (list.Count == 1) return list[0];
		float minDist = Vector3.Distance(origin.position, list[0].transform.position);
		GameObject minObj = list[0];
		for (int i = 1; i < list.Count; ++i) {
			float temp = Vector3.Distance(origin.position, list[i].transform.position);
			if (temp < minDist) {
				minDist = temp;
				minObj = list[i];
			}
		}
		list.Remove(minObj);
		return minObj;
	}
	
}
