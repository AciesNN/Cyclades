using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Shmipl.Base;

namespace Cyclades.Game
{
	public enum Phase {
		AuctionPhase,
		TurnPhase,
		SubPhase
	}

	public static class Library
	{
		#region Реализация

		#region Вспомогательные
		public static Phase GetPhase(IContextGet r) {
			string cur_state = r.GetStr("/cur_state");

			if (cur_state.StartsWith("Auction"))
				return Phase.AuctionPhase;
			else if (cur_state.StartsWith("Turn"))
				return Phase.TurnPhase;

			return Phase.SubPhase;
		}

		public static long GetCurrentPlayer(IContextGet r) {
			//TODO некрасивый хардкод, по идее бы код никак не должен опираться на структуру FSM
			if (r.GetStr("/cur_state") == "Turn.Fight.ArmyWaitDeffender")
				return r.GetLong("/fight/army/deffender/player");
			if (r.GetStr("/cur_state") == "Turn.Fight.NavyWaitDeffender")
				return r.GetLong("/fight/navy/deffender/player");

			if (GetPhase(r) == Phase.AuctionPhase)
				return r.GetLong("/auction/current_player");
			else if (GetPhase(r) == Phase.TurnPhase)
				return r.GetLong("/turn/current_player");

			return -1;
		}
		#endregion

		#region Аукцион
		/*возвращает реальную цену ставки для игрока с учетом количества жрецов*/
		public static long Auction_GoldForBet(IContextGet r, long bet, long player) {
			return Math.Max (bet - r.GetLong ("/markers/priest/[{0}]", player), 1L);
		}

		/*возвращает для игрока номер бога на аукционе на котором находится текущая ставка игрока*/
		public static int Auction_GetCurrentGodBetForPlayer(IContextGet r, long player) {
			List<long> bets = r.GetList<long>("/auction/bets/[*]/[{0}]", player);
			for (int i = 0; i < bets.Count; ++i)
				if (bets [i] > 0)
					return i; 
			return -1;
		}

		/*возвращает для игрока размер его текущей ставки, на каком бы боге она не была сделана*/
		public static long Auction_GetCurrentBetForPlayer(IContextGet r, long player) {
			int currentGodBet = Auction_GetCurrentGodBetForPlayer (r, player);
			if (currentGodBet == -1)
				return 0L;

			long bet = r.GetLong ("/auction/bets/[{0}]/[{1}]", currentGodBet, player);
			return bet;
		}

		/*возвращает номер бога на аукционе по его строковому представленю*/
		public static int Auction_GetGodNumber(IContextGet r, string god) {
			List<string> god_order = r.GetList<string>("/auction/gods_order");
			int god_number = god_order.IndexOf (god);
			return god_number;
		}

		/*возвращает строковое представление бога по его номеру на аукционе*/
		public static string Auction_GetGodByNumber(IContextGet r, int god_number) {
			if (god_number == -1)
				return Constants.godNone;
			string god = r.GetStr ("/auction/gods_order/[" + god_number + "]");
			return god;
		}		

		/*возвращает игрока ставка которого перебита на каком-нибудь из богов, кроме Апполона, разумеется*/
		public static int Aiction_GetBeatenBetPlayer(IContextGet r) {
			long players_number = r.GetLong ("/players_number");

			//находим бога, на котором есть две ставки и возвращаем игрока минимальной ставки
			List<object> bets = r.GetList ("/auction/bets");
			IEnumerable<int> bitten_gods = from god_nomber in Range (bets)
				                           where god_nomber < players_number - 1 
			                               		&& 
			                               		((List<object>)bets [god_nomber])
													.FindAll((o) => { return ((long)o > 0); }
												).Count > 1
			                               select god_nomber;
			if (!bitten_gods.Any())
				return -1;

			List<object> god_bets = (List<object>)bets[bitten_gods.First()];
			IEnumerable<int> players_bets = from player_nomber in Range (god_bets)
				                            where (long)god_bets[player_nomber] != 0
			                                orderby (long)god_bets[player_nomber]
			                                select player_nomber;

			return players_bets.First();
		}

		/*возвращает игрока сделавшего максимальную ставку на бога и -1 если ставок нет*/
		public static int Aiction_GetCurrentBetPlayerForGod (IContextGet r, long god) {
			List<long> god_bets = r.GetList<long> ("/auction/bets/[{0}]", god);
			long max_bet = god_bets.Max();
			if (max_bet == 0)
				return -1;
			return god_bets.IndexOf (max_bet);
		}

		/*получает всех игроков, сделавших ставку на данного бога в порядке возрастания ставки (нужно для отображения ставок на Апполоне)*/
		/*@view*/
		public static List<int> Auction_GetAllOrderBetPlayersForGod(IContextGet r, long god) {
			List<long> god_bets = r.GetList<long>("/auction/bets/[{0}]", god);
			IEnumerable<int> res = from player in Range(god_bets)
				                    where god_bets[player] > 0
			                        orderby god_bets[player]
			                        select player;
			return res.ToList();
		}
		#endregion

		#region Ход 
		/*получить текущий доход игрока*/
		public static long GetIncome(IContextGet r, long player) {

			long res = 0L;

			//1. кол-во рогов на островах пользователя
			List<long> island_owners = r.GetList<long>("/map/islands/owners");
			List<long> island_horns = r.GetList<long>("/map/islands/horn");

			res += 
				(from island_index in Library.Range(island_owners)
				where island_owners[island_index] == player
				select island_horns[island_index])
				.Sum();

			//2. кол-во клеток с кораблями и рогами
			List<object> ships_coords = r.GetList("/map/seas/ships/coords");
			List<long> ships_owners = r.GetList<long>("/map/seas/ships/owners");
			var player_distinct_ships = (from ship_number in Range(ships_owners)
										 where ships_owners[ship_number] == player
										 select new { 
											x = (long)  ((List<object>)ships_coords[ship_number])[0],
											y = (long)  ((List<object>)ships_coords[ship_number])[1]
										 }).Distinct();

			List<object> horn_coords_list = r.GetList("/map/seas/horns");
			var horn_coords =   (from horn_number in Range(horn_coords_list)
								select new	{
									x = (long) ((List<object>)horn_coords_list[horn_number])[0],
									y = (long) ((List<object>)horn_coords_list[horn_number])[1]
								});

			res +=
				player_distinct_ships.Join(horn_coords,
				pl_sh => pl_sh,
				h_c => h_c,
				(pl_sh, h_c) => true)
				.Count();			

			return res;

		}
		#endregion

		#region Карта
		/*получить все номера остовов игрока*/
		public static List<int> Map_GetIslandsByOwner(IContextGet r, long player) {
			List<long> island_owners = r.GetList<long> ("/map/islands/owners");
			IEnumerable<int> res = from island_number in Range(island_owners)
								   where island_owners[island_number] == player
								   select island_number;
			return res.ToList();
		}

		/*получить все острова-соседи точки моря*/
		public static List<int> Map_GetNeiborIslandsByMapPos(IContextGet r, long x, long y) {
			List<object> neibours = Map_GetPointNeighbors(r, x, y);
			List<int> islands = new List<int> ();
			foreach (List<long> neibour in neibours) {
				int island = Map_GetIslandByPoint (r, neibour[0], neibour[1]);
				if (island != -1 && islands.IndexOf (island) == -1)
					islands.Add (island);
			}

			return islands;
		}

		/*определяет кол-во зданий определенного типа на острове*/
		public static int Map_GetBuildCountAtIsland(IContextGet r, long island, string build_type)
		{

			int res = 0;

			//1. количество зданий
			List<string> buildings = r.GetList<string>("/map/islands/buildings/[{0}]", island);
			res +=
				(from build in buildings
				 where build == build_type
				 select 1).Sum();

			//2. +1 за метрополию
			bool is_metro = r.GetBool("/map/islands/is_metro/[{0}]", island);
			if (is_metro)
				res += 1;

			return res;
		}

		/*определяет, принадлежит ли точка карте*/
		public static bool Map_IsPointOnMap(IContextGet r, long x, long y) 	{

			if (x < 0)
				return false;
			if (y < 0)
				return false;

			long sizeY = r.GetLong("/map/size_y");
			if (y > Map_GetYLimit(sizeY))
				return false;
			long sizeX = r.GetLong("/map/size_x");
			if (x > Map_GetXLimit(sizeX, sizeY, y))
				return false;

			return true;

		}

		/*@clear*/
		public static long Map_GetXLimit(long sizeX, long sizeY, long y) {
			return sizeX + sizeY - 1 - Math.Abs(y + 1 - sizeY) - 1;
		}

		/*@clear*/
		public static long Map_GetYLimit(long sizeY) {
			return sizeY*2 - 1;
		}

		/*возвращает номера острова по координатам, или -1, если точка - море*/
		public static int Map_GetIslandByPoint(IContextGet r, long x, long y) {
			List<object> islands = r.GetList("/map/islands/coords");
			int i = 0;
			foreach (List<object> islands_coords in islands) {
				foreach(List<object> coords in islands_coords) {
					if ((long)coords[0] == x && (long)coords[1] == y)
						return i;
				}
				++i;
			}

			return -1;
		}

		/*количество кораблей в точке с заданными координатами*/
		public static int Map_GetShipCountByPoint(IContextGet r, long x, long y) {
			List<object> ships_coords = r.GetList("/map/seas/ships/coords");
			int res =	(from ship_number in Range(ships_coords)
						where  (long) ((List<object>)ships_coords[ship_number])[0] == x
							&& (long) ((List<object>)ships_coords[ship_number])[1] == y
						select true)
						.Count();
			return res;
		}

		/*являются ли соседями две точки*/
		public static bool Map_IsPointsNeibours(IContextGet r, long x1, long y1, long x2, long y2)	{
			if (x1 == x2 && y1 == y2)
				return false; //TODO возможно стоит вызвать и кавалерию?
			
			if (Math.Abs(x1 - x2) > 1 || Math.Abs(y1 - y2) > 1)
				return false;
			
			if (y1 == y2)
				return true;

			if (x1 == x2)
				return true;

			long sizeY = r.GetLong("/map/size_y");
			if (y1 < y2) {
				if (y2 <= sizeY - 1)
					return (x1 == x2-1);
				else
					return (x1 == x2+1);
			} else {
				if (y1 <= sizeY - 1)
					return (x2 == x1-1);
				else
					return (x2 == x1+1);				
			}
		}

		/*является ли точка x, y соседом острова island*/
		public static bool Map_IsIslandNeibourToPoint(IContextGet r, long island, long x, long y) {
			List<object> island_coords = r.GetList("/map/islands/coords/[{0}]", island);
			foreach (List<object> island_coord in island_coords) {
				if (Map_IsPointsNeibours(r, x, y, (long)island_coord[0], (long)island_coord[1]))
					return true;
			}

			return false;
		}

		/*возвращает размер острова*/
		public static int Map_IslandSize(IContextGet r, long island) {
			int res = r.GetList("/map/islands/buildings[{0}]", island).Count;
			return res;
		}
		
		/*@clear*/
		public static int Map_IslandMetroSize(int island_size)	{
			if (island_size == 1)
				return 1;
			return 2;
		}
		
		/*возвращает всех соседей позиции*/
		public static List<object> Map_GetPointNeighbors(IContextGet r, long x, long y) {
			List<object> res = new List<object>();
			
			_Map_AddPointNeighbors (r, x - 1, y, res);
			_Map_AddPointNeighbors (r, x + 1, y, res);
			_Map_AddPointNeighbors (r, x, y - 1, res);
			_Map_AddPointNeighbors (r, x, y + 1, res);
			
			long sizeY = r.GetLong("/map/size_y");
			long dx;
			dx = (y+1 <= sizeY-1 ? +1 : -1);
			_Map_AddPointNeighbors (r, x + dx, y + 1, res);
			
			dx = (y-1 >= sizeY-1 ? +1 : -1);
			_Map_AddPointNeighbors (r, x + dx, y - 1, res);
			
			return res;
		}
		
		private static void _Map_AddPointNeighbors(IContextGet r, long x, long y, List<object> l) {
			if (Map_IsPointOnMap(r, x, y))
				l.Add(new List<long>(){ x, y });				
		}
		
		/*пытается определить - есть ли путь через свои корабли до следующего острова*/
		public static bool Map_HasIslandsShipBrige(IContextGet r, long island1, long island2, long player) {
			List<object> island_coords = r.GetList("/map/islands/coords/[{0}]", island1);
			List<object> pass_coords = new List<object> ();
			
			//для каждой клетки острова-источника 
			foreach (object coords in island_coords) {	
				List<object> coord = (List<object>)coords;
				//2. надо рекурсивно искать для всех клеток-соседок каждой всех соседей-морей с кораблями игрока, но не повторяясь
				//для каждой новой надо определять - а не соседка ли она искомого острова
				if (_Map_HasIslandsShipBrige (r, (long)coord [0], (long)coord [1], pass_coords, player, island2))
					return true;
			}
			
			return false;
		}
		
		public static bool _Map_HasIslandsShipBrige(IContextGet r, long x, long y, List<object> pass_coords, long player, long island) {
			var p = new {X = x, Y = y};
			if (pass_coords.IndexOf(p) >= 0)
				return false;
			pass_coords.Add(p);
			
			List<long> ship_owners = r.GetList<long>("/map/seas/ships/owners");
			List<object> ship_coords = r.GetList("/map/seas/ships/coords");
			
			var neibours = from ship_number in Range(ship_owners)
						   where ship_owners[ship_number] == player
								&& Map_IsPointsNeibours(r, x, y, (long)((List<object>)ship_coords[ship_number])[0], (long)((List<object>)ship_coords[ship_number])[1])
						   select new { X = (long)((List<object>)ship_coords[ship_number])[0], Y = (long)((List<object>)ship_coords[ship_number])[1] };
			
			foreach (var neibour in neibours) {
				if (Map_IsIslandNeibourToPoint (r, island, neibour.X, neibour.Y))
					return true;
				if (_Map_HasIslandsShipBrige(r, neibour.X, neibour.Y, pass_coords, player, island))
					return true;
			}
			
			return false;
		}
		
        /*определяет, на какие острова может добраться плеер с острова по кораблям*/
        public static List<long> Map_GetBridgetIslands(IContextGet r, long island, long player) {
            //TODO надо бы оптимизировать, а то адский перебор получается
            List<long> res = new List<long>();
            int islandsCount = r.GetList ("/map/islands/owners").Count;
            for (int i = 0; i < islandsCount; ++i) {
                if (i != island && Map_HasIslandsShipBrige(r, island, (long)i, player))
                    res.Add((long)i);
            }
            return res;
        }

        /*есть ли у игрока пути побега с острова*/
        public static bool Map_CanPlayerRetreatFromIsland(IContextGet r, long island, long player) {
            List<long> islands = Map_GetBridgetIslands(r, island, player);
			List<long> owners = r.GetList<long> ("/map/islands/owners");
            foreach(long to_island in islands) {
                if (owners[(int)to_island] == player || owners[(int)to_island] == -1)
                    return true;
            }
            return false;
        }

        /*определяет, могут ли корабли игрока помещены в точку с координатами*/
        public static bool Map_IsPointAccessibleForShip(IContextGet r, long x, long y, long player) {
            if (!Library.Map_IsPointOnMap(r, x, y))
                return false;
            
            //TODO - кракены, корабли противника, острова и т.д. и т.д.

            return true; //все что не запрещено - разрешено
        }

        /**/
        public static bool Map_CanPlayerRetreatFromSea(IContextGet r, long x, long y, long player) {
            List<object> points = Map_GetPointNeighbors(r, x, y);
            foreach (List<long> coords in points) {
                if (Map_IsPointAccessibleForShip(r, coords[0], coords[1], player))
                    return true;
            }
            return false;
        }
        #endregion

		#region Бой

		#endregion

		#region Карты
		/*возвращает реальную цену ставки для игрока с учетом количества жрецов*/
		public static long Card_GoldForSlot(IContextGet r, long slot, long player) {
			long temle_count = 0;
			List<int> islands = Map_GetIslandsByOwner(r, player);
			foreach(int island in islands)
				temle_count += Map_GetBuildCountAtIsland(r, (long)island, Constants.buildTemple);
			return Math.Max(Constants.cardPrice[(int)slot] - temle_count, 1L);
		}
		#endregion

		#endregion

		public static long Max(List<object> l) 
		{
			if (l.Count == 0)
				return 0;

			long res = (long)l [0];
			foreach (object o in l) {
				if ((long)o > res)
					res = (long)o;
			}
			return res;
		}

		public static IEnumerable<int> Range(IList l){
			return Enumerable.Range (0, l.Count);
		}
	}
}

