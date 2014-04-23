using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Cyclades.Game;

namespace Shmipl.GameScene
{
	public class main: MonoBehaviour {
		public readonly Shmipl.GameScene.GameController game;
		public static main instance = null;
		private static readonly List<Color> userColors = new List<Color> 
															{Color.green,
															Color.red,
															Color.black,
															Color.blue,
															Color.yellow};

		public main()	{
			instance = this;
			game = new Shmipl.GameScene.GameController();
		}

		public Shmipl.Base.Context context {
			get {
				if (Cyclades.Game.Client.Messanges.cur_player == -1) {
					//Debug.Log("не выбран текущий игрок");
					//TODO!!! тут вот вообще нехорошо! убрать! пользуемся вредной функцией
					return Cyclades.Program.srv.GetContext("Game");
				}
				if (Cyclades.Program.clnts[(int)Cyclades.Game.Client.Messanges.cur_player].GetContext("Game") == null)
					return Cyclades.Program.srv.GetContext("Game"); //TODO - явная недоработка синхронизации!
				return Cyclades.Program.clnts[(int)Cyclades.Game.Client.Messanges.cur_player].GetContext("Game");
			}
		}

		void Awake() {
			StartCoroutine(Shmipl.Base.ThreadSafeMessenger.ReceiveEvent());

			Shmipl.Base.Messenger<string, Hashtable>.AddListener("Shmipl.DeserializeContext", OnContextDeserialize);
			Shmipl.Base.Messenger<string, Hashtable>.AddListener("Shmipl.DoMacros", OnContextChanged);

			Shmipl.Base.Log.PrintDebug = Debug.Log;
			Cyclades.Program.project_path = @"D:\Acies\shmipl\pic2\cs\Cyclades\";	
			Cyclades.Program.Start();

			LoadData("1");
		}

		public void LoadData(string name) {
			Cyclades.Program.srv.GetContext("Game").LoadDataFromFile("Assets\\Data\\" + name + ".txt");
		}

		public Color GetColor(long user) {
			if (user == -1)
				return Color.white;
			else 
				return userColors[(int)user];
		}

		private void OnContextChanged(string context_name, Hashtable msg)
		{
			if (context_name == "Game") {
				Shmipl.Base.ThreadSafeMessenger.SendEvent(() => Shmipl.Base.Messenger.Broadcast("UnityShmipl.UpdateView"));
			}
		}
		
		private void OnContextDeserialize(string context_name, Hashtable msg)
		{
			if (context_name == "Game")	{

			}
		}
	}
}