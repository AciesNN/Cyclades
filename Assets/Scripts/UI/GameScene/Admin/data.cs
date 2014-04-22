using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class data {
	public static Shmipl.Base.Context context = new Shmipl.Base.Context();
	private static readonly List<Color> userColors = new List<Color> 
														{Color.green,
														Color.red,
														Color.black,
														Color.blue,
														Color.yellow};

	static data()	{
		LoadData("1");
	}

	public static void LoadData(string name) {
		context.LoadDataFromFile("Assets\\Data\\" + name + ".txt");
	}

	public static Color GetColor(long user) {
		if (user == -1)
			return Color.white;
		else 
			return userColors[(int)user];
	}
}
