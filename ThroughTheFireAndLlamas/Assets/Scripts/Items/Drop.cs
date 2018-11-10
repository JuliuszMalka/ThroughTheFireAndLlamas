using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour {

	public List<GameObject> drops = new List<GameObject>();
	public int lowerDropsBound = 0;
	public int upperDropsBound = 0;
	public GameObject goldPile = null;

	void Awake() {
		int amount = Random.Range(lowerDropsBound, upperDropsBound);
		for (uint i = 0; i < (uint)amount; ++i) {
			int choice = Random.Range(0, 2);
			if (choice == 0) {
				drops.Add(goldPile);
			} else {
				//drops.Add(ItemManager.GenerateWeapon());
			}
		}
	}
}
