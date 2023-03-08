using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocol : MonoBehaviour
{
	public enum PROTOCOL : short
	{
		BEGIN = 0,

		// �ε��� �����ض�.
		START_LOADING = 1,

		LOADING_COMPLETED = 2,

		// ���� ����.
		AUCTION_START = 3,

		// Ŭ���̾�Ʈ�� ���� ��û.
		AUCTION_REQ = 4,

		// �÷��̾ ���� ������ �˸���.
		PLAYER_AUCTIONING = 5,

		// Ŭ���̾�Ʈ�� ���� ������ �������� �˸���.
		AUCTION_FINISHED_REQ = 6,

		// �����Ǿ����� �˸���.
		AUCTION_FAILED = 7,

		//��ſ� �ʿ��� ���� �ʱ�ȭ�ߴٰ� �˸���.
		BETTINGRESET = 8,

		//������ ���� ���������� �˸���.
		BETTINGFAILED = 9,

		// ������ ���������� �˸���.
		BETTING = 10,

		// ���� �÷��̾ ���� ���� �����Ǿ���.
		ROOM_REMOVED = 11,

		// ���ӹ� ���� ��û.
		ENTER_GAME_ROOM_REQ = 12,

		// ���� ����.
		AUCTION_OVER = 13,

		END
	}
}
