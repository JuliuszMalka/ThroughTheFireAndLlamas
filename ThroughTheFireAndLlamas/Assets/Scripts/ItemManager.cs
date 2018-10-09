using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class ItemManager : MonoBehaviour {

	public enum DamageType { //typ obrażeń fizycznych zadawanych przez broń
		Blunt = 0,
		Slash,
		Thrust
	}

	public enum ElementalDamageType { //typ obrażeń od elementów (jeśli broń takowe posiada)
		None = -1,
		Fire,
		Cold,
		Lightning,
		Acid
	}

	public enum ItemRarity { //rzadkość przedmiotu
		Normal = 1,
		Magic,
		Rare,
		Legendary
	}

	public struct ItemData { //struktura od wartości w broniach które nie będą się zmieniać
		public uint weight;
		public uint basePrice;
		public float range;
		public Sprite weaponSprite;
	}
	
	public Dictionary<string, List<string>> prefixesAndSuffixes = new Dictionary<string, List<string>>(); //prefixy i suffixy do broni
	public Dictionary<DamageType, List<string>> itemNames = new Dictionary<DamageType, List<string>>();//nazwy broni dla każdego typu
	public Dictionary<string, ItemData> dataOfItems = new Dictionary<string, ItemData>();//dane dla każdej z broni
	public Sprite[] sprites = null;//sprite'y dla broni
	//public Dictionary<DamageType, Dictionary<string, Dictionary<string, List<string>>>> itemNames = new Dictionary<DamageType, Dictionary<string, Dictionary<string, List<string>>>>();

	void Start () {
		PlayerStatistics.GetInstance().GetSkills();
		//LoadFromJson();
		//Debug.Log(GenerateWeaponName(DamageType.Blunt));
	}

	void LoadFromJson() {
		using (StreamReader reader = new StreamReader("JsonFiles/weaponNames.json")) {
			itemNames = JsonConvert.DeserializeObject<Dictionary<DamageType, List<string>>>(reader.ReadToEnd());
		}
		using (StreamReader reader = new StreamReader("JsonFiles/prefixesAndSuffixes.json")) {
			prefixesAndSuffixes = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(reader.ReadToEnd());
		}
	}

	// void SerializeWeaponNames() { /*tymczasowa funkcja do wczytania nazw do Jsona*/
	// 	FileStream fs = new FileStream("test.json", FileMode.OpenOrCreate);
	// 	using (StreamWriter writer = new StreamWriter(fs)) {
	// 		writer.WriteLine(JsonConvert.SerializeObject(itemNames, Formatting.Indented));
	// 	}
	// 	fs.Close();
	// }	

	void CreateWeaponData() { /*zrobić rozpiskę i wrzucić do pliku tekstowego, potem będziemy z niego wczytywać */
		int lineCount = CountFileLines("WeaponGeneration/itemData.txt");
		ItemData[] data = new ItemData[lineCount];
		using (StreamReader reader = new StreamReader("WeaponGeneration/itemData.txt")) {
			for (int i = 0; i < lineCount; ++i) {
				string[] temp = reader.ReadLine().Split();
				data[i].weight = System.Convert.ToUInt32(temp[1]);
				data[i].basePrice = System.Convert.ToUInt32(temp[2]);
				data[i].range = System.Convert.ToSingle(temp[3]);
				dataOfItems.Add(temp[0], data[i]);
			}
		}
	}

	void CreateDictionary() {
		DamageType[] damageTypeNames = (System.Enum.GetValues(typeof(DamageType))) as DamageType[];

		var files = Directory.GetFiles("WeaponNames");
		int fileCount = files.Length, namesIndex = 0;

		for (int i = 0; i < fileCount; ++i) {
			List<string> names = new List<string>();
			int lineCount = CountFileLines(files[i]);
			using (StreamReader reader = new StreamReader(files[i])) {
				for (int j = 0; j < lineCount; ++j) {
					names.Add(reader.ReadLine());
				}
			}
			itemNames.Add(damageTypeNames[namesIndex++], names);
		}
	}

	void SaveToJson() {
		using (StreamWriter writer = new StreamWriter("JsonFiles/prefixesAndSuffixes.json")) {
			writer.WriteLine(JsonConvert.SerializeObject(prefixesAndSuffixes, Formatting.Indented));
		}
		using (StreamWriter writer = new StreamWriter("JsonFiles/weaponNames.json")) {
			writer.WriteLine(JsonConvert.SerializeObject(itemNames, Formatting.Indented));
		}
	}

	void GetPrefixesAndSuffixes() {
		prefixesAndSuffixes["prefix"] = new List<string>();
		prefixesAndSuffixes["suffix"] = new List<string>();
		prefixesAndSuffixes["legendaries"] = new List<string>();
		using (StreamReader reader = new StreamReader("WeaponGeneration/prefixes.txt")) {
			while (!reader.EndOfStream) prefixesAndSuffixes["prefix"].Add(reader.ReadLine());
		}
		using (StreamReader reader = new StreamReader("WeaponGeneration/suffixes.txt")) {
			while (!reader.EndOfStream) prefixesAndSuffixes["suffix"].Add(reader.ReadLine());
		}
		using (StreamReader reader = new StreamReader("WeaponGeneration/legendaries.txt")) {
			while (!reader.EndOfStream) prefixesAndSuffixes["legendaries"].Add(reader.ReadLine());
		}
	}

	int CountFileLines(string filename) {
		int count = 0;
		using (StreamReader reader = new StreamReader(filename)) {
			while (!reader.EndOfStream) {
				++count; 
				reader.ReadLine();
			}
		}
		return count;
	}

	string GenerateWeaponName(DamageType typeOfDamage) {
		string weaponName = "";
		weaponName += prefixesAndSuffixes["prefix"][Random.Range(0, prefixesAndSuffixes["prefix"].Count)];
		weaponName += " ";
		weaponName += itemNames[typeOfDamage][Random.Range(0, itemNames[typeOfDamage].Count)];
		weaponName += " ";
		weaponName += prefixesAndSuffixes["suffix"][Random.Range(0, prefixesAndSuffixes["suffix"].Count)];
		return weaponName;
	}

	// void CreateSpritesDictionary() {
	// 	List<string> tempNamesList = new List<string>();
	// 	foreach (DamageType key in itemNames.Keys) {
	// 		foreach (string str in itemNames[key]) {
	// 			tempNamesList.Add(str);
	// 		}
	// 	}
	// 	tempNamesList.Sort();
	// 	List<Sprite> tempSpritesList = sprites.ToList();
	// 	tempSpritesList.Sort();
	// 	for (int i = 0; i < tempNamesList.Count; ++i) {
	// 		weaponSprites.Add(tempNamesList[i], tempSpritesList[i]);
	// 	}
	// }

	public GameObject GenerateWeapon() { //dorobić: generowanie sell price'a, obrażenia względem itemlevela/levelTaga
		GameObject weapon = new GameObject();
		SpriteRenderer weaponSpriteRenderer = weapon.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
		Weapon weaponScript = weapon.AddComponent(typeof(Weapon)) as Weapon;

		int rarity = (int)(Random.value * 100f);
		if (rarity > 35) weaponScript.rarityOfItem = ItemRarity.Normal;
		else if (rarity <= 35 && rarity > 15) weaponScript.rarityOfItem = ItemRarity.Magic;
		else if (rarity <= 15 && rarity > 3) weaponScript.rarityOfItem = ItemRarity.Rare;
		else weaponScript.rarityOfItem = ItemRarity.Legendary; 

		//weaponScript.GenerateSellPrice();
		weaponScript.damageType = (DamageType)Random.Range(0, System.Enum.GetNames(typeof(DamageType)).Length);

		string weaponName = GenerateWeaponName(weaponScript.damageType);

		//weaponSpriteRenderer.sprite = weaponSprites[weaponName];
		weaponScript.itemName = weaponName;
		weapon.name = weaponName;

		int elementalDamageChance = Random.Range(1, 101);
		if (elementalDamageChance < 25) {
			weaponScript.elemType = (ElementalDamageType)Random.Range(0, System.Enum.GetNames(typeof(ElementalDamageType)).Length - 1);
		} 
		weapon.SetActive(false);
		return weapon;
	}
}
