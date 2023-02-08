using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocol : MonoBehaviour
{
	public enum PROTOCOL : short
	{
		BEGIN = 0,

		// 로딩을 시작해라.
		START_LOADING = 1,

		LOADING_COMPLETED = 2,

		// 게임 시작.
		AUCTION_START = 3,

		// 클라이언트의 입찰 요청.
		AUCTION_REQ = 4,

		// 플레이어가 입찰 중임을 알린다.
		PLAYER_AUCTIONING = 5,

		// 클라이언트의 입찰 연출이 끝났음을 알린다.
		AUCTION_FINISHED_REQ = 6,

		// 상대방 플레이어가 나가 방이 삭제되었다.
		ROOM_REMOVED = 7,

		// 게임방 입장 요청.
		ENTER_GAME_ROOM_REQ = 8,

		// 게임 종료.
		AUCTION_OVER = 9,

		END
	}
}
