using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestBoss : MonoBehaviour {

	public enum Attack {
		None = -1,
		Slam,
		HomingMagicProj,
		AcidCloud
	}

	public List<Attack> attacks = new List<Attack>();
	public GameObject projectile = null;
	public GameObject player = null;
	public float slamRange = 0f;
	public float slamDamage = 0f;
	public float damage = 0f;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < System.Enum.GetNames(typeof(Attack)).Length + 2; ++i) {
			for (int j = 0; j < i + 2; ++j) {
				attacks.Add((Attack)i);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Melee() {
		float dist = 2f;
		Vector3 direction = (player.transform.position - this.transform.position).normalized;
		RaycastHit[] hits = Physics.RaycastAll(
			this.transform.position,
			direction, 
			dist
		);
		foreach (RaycastHit hit in hits) {
			if (hit.transform != null) {
				if (hit.transform.gameObject.CompareTag("Player") && !PlayerStatistics.GetInstance().flags["isHit"]) {
					PlayerStatistics.GetInstance().ReduceHealth(this.damage);
				}
			}
		}
	}

	void Slam() {
		Vector3 playerPos = player.transform.position;
		Collider[] caughtObjects = Physics.OverlapSphere(
			playerPos,
			this.slamRange,
			-1
		);
		caughtObjects.ToList().ForEach(obj => {
			if (obj.gameObject.CompareTag("Player") && !PlayerStatistics.GetInstance().flags["isHit"]) {
				PlayerStatistics.GetInstance().ReduceHealth(this.slamDamage);
			}
		});
	}

	void HomingMagicProjectile() {
		Instantiate(projectile, this.transform.position, Quaternion.identity);
	}
}
