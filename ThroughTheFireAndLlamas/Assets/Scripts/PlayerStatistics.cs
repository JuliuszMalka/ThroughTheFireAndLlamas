using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

public class PlayerStatistics {

	private static PlayerStatistics instance = null;
	public static PlayerStatistics GetInstance() {
		if (instance == null) instance = new PlayerStatistics();
		return instance;
	}

	public struct Attributes {
		public uint Strength;
		public uint Dexterity;
		public uint Vitality;
		public uint Intelligence;
	}

	public Dictionary<string, bool> flags = new Dictionary<string, bool>();
	public float health = 0f;
	public float maxHealth = 0f;
	public float mana = 0f;
	public float maxMana = 0f;
	public float endurance = 0f;
	public float maxEndurance = 0f;
	public float speed = 0f;
	public int level = 0;
	public int attributePoints = 0;
	public int skillPoints = 0;
	public int experience = 0;
	public int expToLevelUp = 0;
	public int[] levelUpValues = null;
	public Attributes playerAttributes = new Attributes();
	public List<MethodInfo> weaponSkills = new List<MethodInfo>();
	public Weapon.WeaponType typeOfWeapon = Weapon.WeaponType.Axe;

	/*lista skilli jako jedno z pól*/ /*lista/tablica delegat na funkcje skilli*/

	public PlayerStatistics() {
		flags["isHit"] = false;
		maxHealth = 150f;
		speed = 2f;
		health = maxHealth;
		mana = maxMana;
		endurance = maxEndurance;
	}


	public static void ReduceHealth(float value) {
		instance.health -= value;
	}

	public static void RestoreHealth(float value) {
		instance.health += value;
	}

	public static void LevelUp() {
		instance.experience = 0;
		++instance.skillPoints;
		instance.expToLevelUp = instance.levelUpValues[++instance.level];
		if (instance.level % 2 == 0) {
			++instance.attributePoints;
		}
	}

	public void GetSkills() { 
		MethodInfo[] methods = System.Type.GetType("Weapon").GetMethods(
			BindingFlags.Public | 
			BindingFlags.Instance | 
			BindingFlags.Static
		); //ściągnij info o metodach z klasy weapon
		foreach (MethodInfo method in methods) {
			Regex reg = new Regex(@"[a-zA-Z]*Skill\b");//wzór do szukania metod kończących się na "Skill" i mających cokolwiek wczesniej
			Match match = reg.Match(method.Name);
			if (match.Success) {
				weaponSkills.Add(method); 
			}
		}

	}


}
