﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class data {
	public static Shmipl.Base.Context context = new Shmipl.Base.Context();
	public static long cur_player = 0;

	static data()	{
		LoadData("1");
	}

	public static void LoadData(string name) {
		context.LoadDataFromFile("Assets\\Data\\" + name + ".txt");
	}
}
