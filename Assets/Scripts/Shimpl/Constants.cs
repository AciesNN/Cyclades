using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace  Cyclades.Game
{
	static public class Constants
	{
		//константы

		//1. боги
		public static readonly string godAppolon = "A";
		public static readonly string godZeus = "Z";
		public static readonly string godSophia = "S";
		public static readonly string godMars = "M";
		public static readonly string godPoseidon = "P";
		public static readonly string godNone = "";

		public static readonly List<string> gods = new List<string>() {godZeus, godSophia, godMars, godPoseidon, godAppolon};

		//2. стоимости
		public static readonly List<long> cardPrice = new List<long>() { 4, 3, 2 };
		public static readonly List<long> armyPrice = new List<long>() {0, 2, 3, 4};
		public static readonly List<long> navyPrice = new List<long>() {0, 1, 2, 3};
		public static readonly List<long> priestPrice = new List<long>() {0, 4};
		public static readonly List<long> philosopherPrice = new List<long>() {0, 4};
		public static readonly long philosophToMetro = 4;
        public static readonly long buildingsToMetro = 4; 
        public static readonly long buildingCost = 2;
		public static readonly long moveArmyCost = 1;
		public static readonly long moveNavyCost = 1;
		public static readonly long changeCardCost = 1;
        public static readonly long sellCost = 2;


		//3. стартовые количества
		public static readonly long startGold = 5;

		//4. здания
		public static readonly string buildUniver = godSophia;
		public static readonly string buildFortres = godMars;
		public static readonly string buildMarina = godPoseidon;
		public static readonly string buildTemple = godZeus;
		public static readonly string buildNone = "";

		public static readonly List<string> buildings = new List<string>() {buildTemple, buildUniver, buildFortres, buildMarina};

		//5. армия и флот
		public static readonly long maxArmyCount = 8;
		public static readonly long maxNavyCount = 8;
		public static readonly long maxNavyMove = 3;
        public static readonly long minotaurForce = 2;

		//6. карты
		public static readonly string cardNone = "";
		public static readonly string cardMermaid = "Mer";
		public static readonly string cardPegus = "Peg";
		public static readonly string cardGigant = "Gig";
		public static readonly string cardChimera = "Chm";
		public static readonly string cardCyclope = "Cyc";
		public static readonly string cardSphinx = "Sph";
		public static readonly string cardSylph = "Syl";
		public static readonly string cardHarpy = "Har";
		public static readonly string cardGriffon = "Gri";
		public static readonly string cardMoyra = "Moy";
		public static readonly string cardSatyr = "Sat";
		public static readonly string cardDryads = "Dry";
		public static readonly string cardKraken = "Kra";
		public static readonly string cardMinotaur = "Min";
		public static readonly string cardChiron = "Chr";
		public static readonly string cardGorgon = "Gor";
		public static readonly string cardPolypheme = "Pol";

		public static readonly List<string> cards = new List<string>() 
		{
			cardMermaid,
			cardPegus,
			cardGigant,
			cardChimera,
			cardCyclope,
			cardSphinx,
			cardSylph,
			cardHarpy,
			cardGriffon,
			cardMoyra,
			cardSatyr,
			cardDryads,
			cardKraken,
			cardMinotaur,
			cardChiron,
			cardGorgon,
			cardPolypheme
		};

		//7. кубик боя
		public static readonly List<long> dice = new List<long>() { 0, 1, 1, 2, 2, 3 };
			
		//8. Доходы на Апполоне
		public static readonly long apolloSmallIncome = 1L;
		public static readonly long apolloBigIncome = 4L;


	}
}

