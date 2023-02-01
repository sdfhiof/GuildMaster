using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirusWarGameServer
{
	using FreeNet;

	public class CPlayer
	{
		CGameUser owner;
		public byte player_index { get; private set; }
		public List<int> bidUnitsID { get; private set; }

		public int myGold;

		public string playerName;

		public CPlayer(CGameUser user, byte player_index)
		{
			this.owner = user;
			this.player_index = player_index;
			this.bidUnitsID = new List<int>();
			this.myGold = 1000;
			this.playerName = $"GuildMaster{(int)player_index}";
		}

		public void reset()
		{
			this.bidUnitsID.Clear();
			this.myGold = 1000;
		}

		public void send(CPacket msg)
		{
			this.owner.send(msg);
			CPacket.destroy(msg);
		}

		public void send_for_broadcast(CPacket msg)
		{
			this.owner.send(msg);
		}

	}
}
