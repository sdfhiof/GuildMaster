using System;

namespace VirusWarGameServer
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

		// 유찰되었음을 알린다.
		AUCTION_FAILED = 7,

		//경매에 필요한 값을 초기화했다고 알린다.
		BETTINGRESET = 8,

		//누군가 상위 입찰했음을 알린다.
		BETTINGFAILED = 9,

		// 누군가 입찰했음을 알린다.
		BETTING = 10,

		// 상대방 플레이어가 나가 방이 삭제되었다.
		ROOM_REMOVED = 11,

		// 게임방 입장 요청.
		ENTER_GAME_ROOM_REQ = 12,

		// 게임 종료.
		AUCTION_OVER = 13,

		END
	}
}
