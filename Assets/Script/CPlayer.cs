using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeNet;

public class CPlayer : MonoBehaviour
{
	CGameUser owner;
	public byte player_index { get; private set; }
	public List<int> bidUnitsID { get; private set; }

	public int myGold;

	public string playerName;

	public int MyBetGoldAmount;

	public CPlayer(CGameUser user, byte player_index)
	{
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


