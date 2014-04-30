using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Cyclades.Game;
using Shmipl;

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
		private static readonly Dictionary<string, Color> buildColors = new  Dictionary<string, Color>
																{{Cyclades.Game.Constants.buildNone, Color.gray},
																{Cyclades.Game.Constants.buildMarina, Color.blue},
																{Cyclades.Game.Constants.buildFortres, Color.red},
																{Cyclades.Game.Constants.buildUniver, Color.white},
																{Cyclades.Game.Constants.buildTemple, Color.magenta}};


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
				//if (Cyclades.Program.clnts[(int)Cyclades.Game.Client.Messanges.cur_player].GetContext("Game") == null)
					return Cyclades.Program.srv.GetContext("Game"); //TODO - явная недоработка синхронизации!
				//return Cyclades.Program.clnts[(int)Cyclades.Game.Client.Messanges.cur_player].GetContext("Game");
			}
		}

		void Awake() {
			//LoadData("1");

			StartCoroutine(Shmipl.Base.ThreadSafeMessenger.ReceiveEvent());

			Shmipl.Base.Messenger<string, object, Hashtable>.AddListener("Shmipl.DeserializeContext", OnContextDeserialize);
			Shmipl.Base.Messenger<string, object, Hashtable>.AddListener("Shmipl.DoMacros", OnContextChanged);
			Shmipl.Base.Messenger<object, Hashtable>.AddListener("Shmipl.Error", OnError);
			Shmipl.Base.Messenger<object, string>.AddListener("Shmipl.AddContext", OnAddContext);
			Shmipl.Base.Messenger<object, string>.AddListener("Shmipl.RemoveContext", OnRemoveContext);

			Shmipl.Base.Log.PrintDebug = Debug.Log;
			Cyclades.Program.project_path = @"D:\Acies\shmipl\pic2\cs\Cyclades\";	
			Cyclades.Program.Start();
		}

		void OnDestroy() {
			Shmipl.Base.Log.close_all();
		}

		public void LoadData(string name) {
			Cyclades.Program.srv.GetContext("Game").LoadDataFromFile("Assets\\Data\\" + name + ".txt");
			Cyclades.Program.CreateClients();
		}

		public Color GetColor(long user) {
			if (user == -1)
				return Color.white;
			else 
				return userColors[(int)user];
		}

		public Color GetBuildColor(string build) {
			return buildColors[build];
		}

		private void OnContextChanged(string context_name, object to, Hashtable msg) {
			if (context_name == "Game" && (long)msg["to"] == Cyclades.Game.Client.Messanges.cur_player) {
				Debug.Log("change: " + Shmipl.Base.json.dumps(msg));
				Shmipl.Base.ThreadSafeMessenger.SendEvent(() => Shmipl.Base.Messenger.Broadcast("UnityShmipl.UpdateView"));
			}
		}
		
		private void OnContextDeserialize(string context_name, object to, Hashtable msg) {
			if (context_name == "Game" && (long)msg["to"] == 0)	{
				Debug.Log("load: " + Shmipl.Base.json.dumps(msg));
				Shmipl.Base.ThreadSafeMessenger.SendEvent(() => Shmipl.Base.Messenger.Broadcast("UnityShmipl.UpdateView"));
			}
		}

		private void OnError(object to, Hashtable msg) {
			Debug.Log("\tERROR: " + Shmipl.Base.json.dumps(msg));
		}

		private void OnAddContext(object to, string fsm_name) {
			if ((long)to == 0)
				Debug.Log("+FSM: " + fsm_name);
		}

		private void OnRemoveContext(object to, string fsm_name) {
			if ((long)to == 0)
				Debug.Log("-FSM: " + fsm_name);
		}

		public void SendSrv(Hashtable msg) {
			Debug.Log("msg: " + Shmipl.Base.json.dumps(msg));
			Cyclades.Program.SendToSrv(msg);
		}
	}
}