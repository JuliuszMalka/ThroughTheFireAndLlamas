using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public enum WeaponType {
		Axe = 0,
		Club,
		Dagger,
		Hammer,
		Spear,
		Staff,
		Sword
	}

	public float attackSpeed 		= 0f;
	public int minDamage 			= 0;
	public int maxDamage 			= 0;
	public int minElementalDamage 	= 0;
	public int maxElementalDamage 	= 0;
	public int sellPrice 			= 0;
	public int itemLevel 			= 0;
	public int criticalStrikeChance = 0;
	public float criticalStrikeMultiplier = 0f;
	public ItemManager.DamageType damageType;
	public ItemManager.ElementalDamageType elemType = ItemManager.ElementalDamageType.None;
	public ItemManager.ItemRarity rarityOfItem;
	public string itemName 			= null;

	public virtual void PickUp() {}

	public virtual void Special() {}

	// public void GenerateSellPrice() {
	// 	sellPrice = (int)(basePrice * Mathf.Pow((int)rarityOfItem, Mathf.Sqrt(GameManager.levelTag)));
	// }
	public static void AxeSkill(Vector3 direction) {
		Debug.Log("Invoked that method");
		Debug.Log("Parameter: " + direction.ToString());
	}

	public static void ClubSkill(Vector3 direction) {
		
	}

	public static void DaggerSkill(Vector3 direction) {

	}

	public static void HammerSkill(Vector3 direction) {
		
	}

	public static void SpearSkill(Vector3 direction) {
		
	}

	public static void StaffSkill(Vector3 direction) {
		
	}

	public static void SwordSkill(Vector3 direction) {
		
	}

	

	
}
