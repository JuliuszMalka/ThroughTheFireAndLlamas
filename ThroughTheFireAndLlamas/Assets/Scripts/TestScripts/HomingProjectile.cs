using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour {

	public Transform target = null;
	public float perishTimer = 3f;
	public float speed = 0f;
	public float damage = 0f;
	public float range = 0f;
	// Use this for initialization

	void OnDestroy() {
		/*odpalić animację*/
		Explode();
	}

	void Start () {
		target = GameObject.FindWithTag("Player").transform;
		Destroy(this.gameObject, perishTimer);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = Vector3.MoveTowards(target.position, this.transform.position, speed * Time.deltaTime);
	}

	void Explode() {
		Collider[] caughtObjects = Physics.OverlapSphere(this.transform.position, this.range, -1);
		foreach (var obj in caughtObjects) {
			if (obj.gameObject.CompareTag("Player") && !PlayerStatistics.GetInstance().flags["isHit"]) {
				PlayerStatistics.GetInstance().ReduceHealth(this.damage);
				PlayerStatistics.GetInstance().ToggleFlag("isHit", true);
			} 
		}
	}
}
