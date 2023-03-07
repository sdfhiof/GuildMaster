using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeNet;

public class CPlayer : MonoBehaviour
{
	public byte player_index { get; private set; }
	public List<int> bidUnitsID { get; private set; }

	public int myGold;

	public string playerName;

	public int MyBetGoldAmount;

	public CPlayer(byte player_index)
	{
		this.player_index = player_index;
		this.bidUnitsID = new List<int>();
		this.myGold = 1000;
		this.playerName = $"GuildMaster{player_index}";
	}

	public void reset()
	{
		this.bidUnitsID.Clear();
		this.myGold = 1000;
	}

	public void initialize(byte player_index)
	{
		this.player_index = player_index;
	}
}


