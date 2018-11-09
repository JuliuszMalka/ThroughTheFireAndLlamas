using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour {

	public uint goldValue = 0;

	void Start() {
		/* wylosować wartość golda */
	}

	void OnDestroy() {
		/*odpalić animację podnoszenia*/
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Player")) {
			PlayerStatistics.GetInstance().IncreaseGoldCount(this.goldValue);
			Destroy(this.gameObject);
		}
	}
}
