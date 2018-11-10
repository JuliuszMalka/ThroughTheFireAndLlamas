using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

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

	public struct LevelInfo {
		public uint AttributePoints;
		public uint ExpToLevelUp;
		public uint PlayerLevel;
		public uint SkillPoints;
		public uint CurrentExperience;
	}

	public struct Stats {
		public float Health;
		public float MaxHealth;
		public float Mana;
		public float MaxMana;
		public float Endurance;
		public float MaxEndurance;
		public float Speed;
	}

	public enum PlayerClass {
		Knight = 0,
		Wizard,
		Barbarian,
		Thief
	}

	//atrybuty gracza
	public uint[] levelUpValues 		= null;
	public LevelInfo levelInfo 			= new LevelInfo();
	public Attributes playerAttributes 	= new Attributes();
	public Stats playerStats 			= new Stats();
	public ulong gold 					= 0;

	//zmienić typ wartości z 'object' na węzeł drzewa po zaimplementowaniu
	public Dictionary<string, System.Action<Pair<Vector3, Vector3>>> allocatedSkills = new Dictionary<string, System.Action<Pair<Vector3, Vector3>>>();//TESTOWE: słownik z metodami od skilli
	public Dictionary<string, bool> flags 									  		 = new Dictionary<string, bool>();//słownik z flagami
	public Dictionary<Weapon.WeaponType, MethodInfo> weaponSkills 			  		 = new Dictionary<Weapon.WeaponType, MethodInfo>();//metody od skilli broni
	public Dictionary<PlayerClass, Attributes> startingAttributes 			  		 = new Dictionary<PlayerClass, Attributes>();//startowe atrybuty wszystkich klas
	public Dictionary<string, float> defaultTimerValues 					  		 = new Dictionary<string, float>();//wartości liczników i cooldownów które będą przypisywane do właściwych timerów
	public Dictionary<string, float> timers 								  		 = new Dictionary<string, float>();//właściwe timery
	public Weapon equippedWeapon 											  		 = null;//referencja do wyekwipowanej broni
	public Weapon.WeaponType typeOfWeapon 									  		 = Weapon.WeaponType.Axe;//typ wyekwipowanej broni (do skilli broni)

	public PlayerClass playerClass;//wybrana przez gracza klasa

	private PlayerStatistics() {
		FlagsInit();
		CooldownsInit();
		StartingAttributesInit();
		AttributesInit(PlayerClass.Wizard); //testowo klasa Wizard
		StatsInit();
		MagicSkillTreeInit();
		MeleeSkillTreeInit();
		PassiveSkillTreeInit();
	}

	~PlayerStatistics() {//destruktor: czyścimy wszystkie słowniki, listy itd. (świeże bułki, wędliny itd.)
		allocatedSkills.Clear();
		flags.Clear();
		weaponSkills.Clear();
		startingAttributes.Clear();
		defaultTimerValues.Clear();
		timers.Clear();
		System.Array.Clear(levelUpValues, 0, levelUpValues.Length);
		levelInfo = default(LevelInfo);
		playerStats = default(Stats);
		playerAttributes = default(Attributes);
	}

	public void FlagsInit() { //wczytywanie flag z pliku json
		using (StreamReader reader = new StreamReader("JsonFiles/flags.json")) {//jeśli zrobimy using jak tutaj, to nie trzeba wołać metody StreamReader.Close() żeby zamknąć strumień, zamknie automatycznie po wyjściu z usinga
			flags = JsonConvert.DeserializeObject<Dictionary<string, bool>>(reader.ReadToEnd());//przypisujemy do flags wynik deserializacji (jako generyk podajemy typ obiektu który deserializujemy) i podajemy zawartość pliku do funkcji
		}
	}

	public void SerializeFlags() { //zrzut słownika z flagami do pliku razem ze wcześniejszą jego inicjalizacją
		flags["isHit"] = false;
		flags["poisoned"] = false;
		flags["onFire"] = false;

		FileStream fs = new FileStream("JsonFiles/flags.json", FileMode.OpenOrCreate);//z parametrem FileMode.OpenOrCreate otworzy plik, jeśli istnije, w przeciwnym wypadku utworzy nowy
		using (StreamWriter writer = new StreamWriter(fs)) {
			writer.WriteLine(JsonConvert.SerializeObject(flags, Formatting.Indented));//funkcja SerializeObject zwraca stringa reprezentującego zserializowany obiekt, który zapisujemy do pliku obiektem klasy StreamWriter
		}
		fs.Close();//zamykamy FileStream po bożemu
	}

	public void SerializeStats() {//zrzut startowych statystyk (zdrowie, itd.) do pliku
		Stats startingStats = new Stats();
		startingStats.Health = startingStats.MaxHealth = 150f;
		startingStats.Mana = startingStats.MaxMana = 150f;
		startingStats.Endurance = startingStats.MaxEndurance = 150f;
		startingStats.Speed = 5f;
		FileStream fs = new FileStream("JsonFiles/playerStats.json", FileMode.OpenOrCreate);//same as above
		using (StreamWriter writer = new StreamWriter(fs)) {
			writer.WriteLine(JsonConvert.SerializeObject(startingStats, Formatting.Indented));
		}
		fs.Close();
	}

	public void StatsInit() {
		using (StreamReader reader = new StreamReader("JsonFiles/playerStats.json")) {
			playerStats = JsonConvert.DeserializeObject<Stats>(reader.ReadToEnd());
		}
	}

	public void AttributesInit(PlayerClass pClass) {
		if (!System.Enum.IsDefined(typeof(PlayerClass), pClass)) throw new System.InvalidOperationException();//ziuuuuuuuuuuu wyjątkiem, niech cierpią
		playerAttributes = startingAttributes[pClass];//przypisanie odpowiednich statystyk graczowi na podstawie jego klasy
	}

	public void SaveCooldownValues() {//zrzut timerów do pliku
		defaultTimerValues["hit"] = 0.4f;
		defaultTimerValues["fireball"] = 5f;
		defaultTimerValues["arc"] = 10f;

		FileStream fs = new FileStream("JsonFiles/cooldowns.json", FileMode.OpenOrCreate);
		using (StreamWriter writer = new StreamWriter(fs)) {
			writer.WriteLine(JsonConvert.SerializeObject(defaultTimerValues, Formatting.Indented));
		}
		fs.Close();
	}

	public void CooldownsInit() {//same as above
		using (StreamReader reader = new StreamReader("JsonFiles/cooldowns.json")) {
			defaultTimerValues = JsonConvert.DeserializeObject<Dictionary<string, float>>(reader.ReadToEnd());
		}
		foreach (var entry in defaultTimerValues) {//inicjalizujemy wszystko na 0, żeby móc od razu używać na początku gry
			timers[entry.Key] = 0f;
		}
	}

	public void StartingAttributesInit() {//same as above
		using (StreamReader reader = new StreamReader("JsonFiles/startingAttributes.json")) {
			startingAttributes = JsonConvert.DeserializeObject<Dictionary<PlayerClass, Attributes>>(reader.ReadToEnd());
		}
	}

	public void SerializeStartingAttributes() {//zrzut startowych statystyk dla wszystkich klas do pliku
		Attributes knightStats = new Attributes();
		Attributes wizardStats = new Attributes();
		Attributes barbarianStats = new Attributes();
		Attributes thiefStats = new Attributes();
		Dictionary<PlayerClass, Attributes> tempAttributes = new Dictionary<PlayerClass, Attributes>();//tymczasowy słownik który to trzyma żeby to potem zrzucić
		
		//setting the attributes for Wizard class
		wizardStats.Intelligence = 15;
		wizardStats.Strength = 8;
		wizardStats.Vitality = 11;
		wizardStats.Dexterity = 6;
		//setting the attributes for Knight class
		knightStats.Intelligence = 8;
		knightStats.Strength = 12;
		knightStats.Vitality = 15;
		knightStats.Dexterity = 5;
		//setting the attributes for Barbarian class 
		barbarianStats.Intelligence = 5;
		barbarianStats.Strength = 18;
		barbarianStats.Vitality = 12;
		barbarianStats.Dexterity = 5;
		//setting the attributes for Thief class
		thiefStats.Intelligence = 9;
		thiefStats.Strength = 6;
		thiefStats.Vitality = 10;
		thiefStats.Dexterity = 15;
		//przypisanie poszczególnych struktur do odpowiedniego klucza
		tempAttributes[PlayerClass.Wizard] = wizardStats;
		tempAttributes[PlayerClass.Knight] = knightStats;
		tempAttributes[PlayerClass.Barbarian] = barbarianStats;
		tempAttributes[PlayerClass.Thief] = thiefStats;

		FileStream fs = new FileStream("JsonFiles/startingAttributes.json", FileMode.OpenOrCreate);
		using (StreamWriter writer = new StreamWriter(fs)) {
			writer.WriteLine(JsonConvert.SerializeObject(tempAttributes, Formatting.Indented));
		}
		fs.Close();
	}

	// public void SaveStartingAttributes() {
	// 	FileStream fs = new FileStream("JsonFiles/startingAttributes.json", FileMode.OpenOrCreate);
	// 	using (StreamWriter writer = new StreamWriter(fs)) {
	// 		writer.WriteLine(JsonConvert.SerializeObject(startingAttributes), Formatting.Indented);
	// 	}
	// 	fs.Close();
	// }

	public void IncreaseAttribute(string fieldName) {//zapnijcie pasy, bo będzie jazda. metoda od zwiększania wartości danego atrybutu, identyfikator to string z nazwą zmiennej w tej strukturze
		FieldInfo attribute = playerAttributes.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);//ściągamy informacje o danym polu ze struktury, jak nie ma takiego pola zwraca null
		if (attribute == null) throw new System.NullReferenceException();//jedyny słuszny wyjątek, dont even deal with that
		uint value = (uint)attribute.GetValue(playerAttributes);//ściągamy wartość pola

		object temp = playerAttributes;//yyyyhhh, boxing, bo trzeba przypisać strukturę ze względu na to że jest value typem i nadpisać ją nową

		attribute.SetValue(temp, ++value);//zmieniamy wartość w tempie
		playerAttributes = (Attributes)temp;//nadpisujemy strukturę z atrybutami
	}

	public bool ToggleFlag(string key, bool value) {//ustawiamy flagę w słowniku
		if (!flags.ContainsKey(key)) return false;
		flags[key] = value;
		return true;
	}

	public void ReduceHealth(float value) {//redukujemy ilość obecnego zdrowia
		playerStats.Health -= value;
		if (playerStats.Health < 0f) playerStats.Health = 0f;//jeśli zeszłoby poniżej zera to ustawiamy na 0
	}

	public void RestoreHealth(float value) {//jak u góry tylko na odwrót
		playerStats.Health += value;
		if (playerStats.Health > playerStats.MaxHealth) playerStats.Health = playerStats.MaxHealth;//jeśli HP przekroczyłoby maxHealth to ustawiamy na maxHealth
	}

	public void ReduceMana(float value) {//tak samo jak z życiem
		playerStats.Mana -= value;
		if (playerStats.Mana < 0f) playerStats.Mana = 0f;
	}

	public void RestoreMana(float value) {//jak z życiem
		playerStats.Mana += value;
		if (playerStats.Mana > playerStats.MaxMana) playerStats.Mana = playerStats.MaxMana;
	}

	public void IncreaseGoldCount(uint value) {//zwiększamy ilość golda
		gold += value;
	}

	public void DecreaseGoldCount(uint value) {//analogicznie jak wyżej
		if (value > gold) return;
		gold -= value;
	}

	public void IncreaseExperience(uint value) {//zwiększamy ilość expa
		levelInfo.CurrentExperience += value;
		if (levelInfo.CurrentExperience >= levelInfo.ExpToLevelUp) {//jeśli expa wyjebało poza skalę to zerujemy w pizdu i levelujemy
			LevelUp();
		}
	}

	public void LevelUp() {//LEVELUP!
		levelInfo.CurrentExperience = 0;//zerujemy expa, zaczynamy od "nowa"
		++levelInfo.SkillPoints;//dodajemy skillpoint
		levelInfo.ExpToLevelUp = levelUpValues[++levelInfo.PlayerLevel];//ustawiamy nową wartość do wylewelowania

		if (levelInfo.PlayerLevel % 2 == 0) {//jeśli level jest parzysty to dodajemy punkt atrybutu
			++levelInfo.AttributePoints;
		}

	}

	public void GetWeaponSkills() { //zapiąć pasy, kolejna jazda bez trzymanki. ściągamy skille specjalne broni
		MethodInfo[] methods = System.Type.GetType("Weapon").GetMethods(
			BindingFlags.Public | 
			BindingFlags.Instance | 
			BindingFlags.Static
		); //ściągnij info o metodach z klasy weapon (flagi ściągają metody publiczne i statyczne)
		Weapon.WeaponType[] enumValues = System.Enum.GetValues(typeof(Weapon.WeaponType)) as Weapon.WeaponType[];//ściągamy wartości enuma o typie WeaponType z klasy Weapon, ściąga je jako inty więc rzutujemy na enuma
		int index = 0;
		foreach (MethodInfo method in methods) {//ziuuuuu po obiektach
			Regex reg = new Regex(@"[a-zA-Z]*Skill\b");//wzór do szukania metod kończących się na "Skill" i mających cokolwiek wczesniej//bez '@' nie zadziała
			Match match = reg.Match(method.Name);//"matchujemy" wzór z danym stringiem
			if (match.Success) {//sprawdzamy sukces
				weaponSkills.Add(enumValues[index++], method); //dodajemy do słownika jeśli się udało
			}
		}
	}

	public void AddSkill(System.Action<Pair<Vector3, Vector3>> skill, string key) {
		allocatedSkills[key] = new System.Action<Pair<Vector3, Vector3>>(skill);
	}

	public void SetTimer(string timerName) {
		timers[timerName] = defaultTimerValues[timerName];
	}

	public void MagicSkillTreeInit() {

	}

	public void MeleeSkillTreeInit() {

	}

	public void PassiveSkillTreeInit() {

	}
	/*ogarnąć po zaimplementowaniu drzewa*/
	// public void AllocateSkill(object treenode, short ID) {//zamienić typ "object" na typ "Tree"
	// 	allocatedSkills.Add(ID, typeof(object).GetMethod("NodeSkill", BindingFlags.Public | BindingFlags.Instance));		
	// }


}
