using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace VirusWarGameServer
{
	using FreeNet;

	/// <summary>
	/// 게임의 로직이 처리되는 핵심 클래스이다.
	/// </summary>
	public class CGameRoom
	{
		enum PLAYER_STATE : byte
		{
			// 방에 막 입장한 상태.
			ENTERED_ROOM,

			// 로딩을 완료한 상태.
			LOADING_COMPLETE,

			// 경매 진행 준비 상태.
			READY_TO_AUCTION,

			// 경매 연출을 모두 완료한 상태.
			CLIENT_AUCTION_FINISHED
		}

		// 게임을 진행하는 플레이어
		List<CPlayer> players;

		// 플레이어들의 상태를 관리하는 변수.
		Dictionary<byte, PLAYER_STATE> player_state;


		List<int> _auctionUnitIDList;
		List<int> _auctionOrderIDList;
		List<int> _instanceUnitsIDList;
		List<int> _failedUnitsIDList;

		List<Vector2> _auctionIDPos;
		List<Vector2> _instanceUnitsIdPos;
		List<Vector2> _failedUnitsIdPos;

		int NowAuctionUnitID;

		int ActOrListPnt;
		int FailedCount;

		string Bidder;

		int NowBetGoldAmount;

		int AuctionTime = 15;

		Timer ATimer;

		public CGameRoom()
		{
			this.players = new List<CPlayer>();
			this.player_state = new Dictionary<byte, PLAYER_STATE>();


			// 경매 초기 상태를 지정하는 자리가 될듯
			SetUnitList();


		}

		public void SetUnitList()  // 초기 경매가 활성화 되면 실행
		{

			_auctionUnitIDList.Clear();                   //  리스트를 초기화
			int _auctionUnitListCount = 16;           // 리스트에 들어갈 인스턴스 수

			_auctionUnitIDList = Enumerable.Range(0, _auctionUnitListCount).ToList();

			AuctionOrder(ref _auctionUnitIDList);                   /// SetUnitList로 만든 _auctionUnitList를 셔플해서 _auctionOrderList를 생성
			AuctionInOrder(ref _instanceUnitsIDList);              /// AuctionOrder로 만든 _auctionOrderList를 이용해 _instanceUnitsList를 생성

			
		}

		public void ShuffleList(ref List<int> list)    // 리스트를 셔플해주는 함수
		{

			int to = list.Count;
			while (to > 1)
			{
				var rnd = new Random(DateTime.Now.Millisecond);
				int from = rnd.Next(0, --to);
				int tmp = list[from];
				list[from] = list[to];
				list[to] = tmp;

			}
		}

		public void AuctionOrder(ref List<int> list)  // 셔플한 리스트로 _auctionOrderList를 다시 생성
		{
			ShuffleList(ref list);
			_auctionOrderIDList = new List<int>(list);
		}

		public void AuctionInOrder(ref List<int> list)  // 셔플한 리스트로 _instanceUnitsList를 생성
		{
			//_instanceUnitsIDList = list.ConvertAll(new List<int>); 깊은 복사 형식  int 형식이라 이런식으로 할필요 없을듯
			list = _auctionOrderIDList.ToList();

		}

		/// <summary>
		/// 모든 유저들에게 메시지를 전송한다.
		/// </summary>
		/// <param name="msg"></param>
		void broadcast(CPacket msg)
		{
			this.players.ForEach(player => player.send_for_broadcast(msg));
			CPacket.destroy(msg);
		}

		

		/// <summary>
		/// 플레이어의 상태를 변경한다.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="state"></param>
		void change_playerstate(CPlayer player, PLAYER_STATE state)
		{
			if (this.player_state.ContainsKey(player.player_index))
			{
				this.player_state[player.player_index] = state;
			}
			else
			{
				this.player_state.Add(player.player_index, state);
			}
		}


		/// <summary>
		/// 모든 플레이어가 특정 상태가 되었는지를 판단한다.
		/// 모든 플레이어가 같은 상태에 있다면 true, 한명이라도 다른 상태에 있다면 false를 리턴한다.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		bool allplayers_ready(PLAYER_STATE state)
		{
			foreach(KeyValuePair<byte, PLAYER_STATE> kvp in this.player_state)
			{
				if (kvp.Value != state)
				{
					return false;
				}
			}

			return true;
		}


		/// <summary>
		/// 매칭이 성사된 플레이어들이 게임에 입장한다.
		/// </summary>
		public void enter_gameroom(CGameUser user1, CGameUser user2, CGameUser user3, CGameUser user4)
		{
			// 플레이어들을 생성하고 각각 1번, 2번, 3번, 4번 인덱스를 부여해 준다.
			CPlayer player1 = new CPlayer(user1, 1);
			CPlayer player2 = new CPlayer(user2, 2);
			CPlayer player3 = new CPlayer(user3, 3);
			CPlayer player4 = new CPlayer(user4, 4); 
			this.players.Clear();
			this.players.Add(player1);
			this.players.Add(player2);
			this.players.Add(player3);
			this.players.Add(player4);

			// 플레이어들의 초기 상태를 지정해 준다.
			this.player_state.Clear();
			change_playerstate(player1, PLAYER_STATE.ENTERED_ROOM);
			change_playerstate(player2, PLAYER_STATE.ENTERED_ROOM);
			change_playerstate(player3, PLAYER_STATE.ENTERED_ROOM);
			change_playerstate(player4, PLAYER_STATE.ENTERED_ROOM);

			// 로딩 시작메시지 전송.
			this.players.ForEach(player =>
            {
                CPacket msg = CPacket.create((Int16)PROTOCOL.START_LOADING);
                msg.push(player.player_index);  // 본인의 플레이어 인덱스를 알려준다.
                player.send(msg);
            });

			user1.enter_room(player1, this);
			user2.enter_room(player2, this);
			user3.enter_room(player3, this);
			user4.enter_room(player4, this);
		}


		/// <summary>
		/// 클라이언트에서 로딩을 완료한 후 요청함.
		/// 이 요청이 들어오면 게임을 시작해도 좋다는 뜻이다.
		/// </summary>
		/// <param name="sender">요청한 유저</param>
		public void loading_complete(CPlayer player)
		{
			// 해당 플레이어를 로딩완료 상태로 변경한다.
			change_playerstate(player, PLAYER_STATE.LOADING_COMPLETE);

			// 모든 유저가 준비 상태인지 체크한다.
			if (!allplayers_ready(PLAYER_STATE.LOADING_COMPLETE))
			{
				// 아직 준비가 안된 유저가 있다면 대기한다.
				return;
			}

			// 모두 준비 되었다면 게임을 시작한다.
			Auction_start();
		}


		/// <summary>
		/// 게임을 시작한다.
		/// </summary>
		void Auction_start()
		{
			// 게임을 새로 시작할 때 마다 혹은 경매가 끝날때마다 초기화해줘야 할 것들.
			bettingReset();

			// 게임 시작 메시지 전송.
            CPacket msg = CPacket.create((short)PROTOCOL.AUCTION_START);
            this.players.ForEach(player =>
            {
                msg.push(player.player_index);      // 누구인지 구분하기 위한 플레이어 인덱스.
                byte my_gold = (byte)player.myGold;  // 모든 플레이어에게 기본 경매금 설정
				msg.push(my_gold);
               
            });
			msg.push(_auctionUnitIDList);                         // 경매에 사용되는 모든 리스트 업데이트
			msg.push(_auctionOrderIDList);
			msg.push(_instanceUnitsIDList);
			msg.push(_failedUnitsIDList);
			broadcast(msg);

			while (_instanceUnitsIDList.Count > 0 || _failedUnitsIDList.Count > 0 || NowAuctionUnitID != 17)
            {
				AuctionTimer();

			}

			Auction_over();


		}

		void Auction()   // 최조 경매 시작 그리고 매 경매가 끝날 때마다 실행 
		{
			CPacket msg = CPacket.create((short)PROTOCOL.AUCTION_REQ);
			if (0 <= NowAuctionUnitID && NowAuctionUnitID < 16)  // 현재 경매중인유닛이 있으면
			{
				this.players.ForEach(player =>
				{
					if (Bidder == player.playerName)  // 현재 입찰자가 플레이어라면
					{

						player.bidUnitsID.Add(NowAuctionUnitID);                                  /// 위 게임 오브젝트를 리스트에 저장
						msg.push(player.bidUnitsID);
					}

				});
				NowAuctionUnitID = 17;
			}

			if (_failedUnitsIDList.Count != 0 && _instanceUnitsIDList.Count == 0)  // 유찰된 유닛이 존재하고 경매유닛이 없다면 재정렬 함수 실행
			{
				ReDefineList();
			}

			NowAuctionUnitID = RemoveActionUnitList();  // 경매 유닛 설정                                 

			// 최종 결과를 broadcast한다.
			msg.push(NowAuctionUnitID);                         // 어떤 유닛이 경매로 올라갔는지
			msg.push(_failedUnitsIDList);                           // 경매에 사용되는 모든 리스트 업데이트
			msg.push(_instanceUnitsIDList);
			msg.push(_auctionOrderIDList);
			broadcast(msg);                                           // 모두에게 전송
		}

		public int RemoveActionUnitList()  //경매 시작시 경매 위치로 올라가는 유닛 삭제 및 경매 리스트 정렬 함수
		{
			int ZeroListIndex = _instanceUnitsIDList[0];           /// 현재 경매 유닛에 올라갈 0번째 리스트를 넣어준다
			int InstUnCnt = _instanceUnitsIDList.Count;          /// InstUnCnt을 리스트 수만큼 맞춰준다
			ActOrListPnt++;                                             /// _instanceUnitsList를 당겨오기 위해 쓰는 변수
																		     /// 0번째 리스트 값은 현재 경매 유닛으로 올리므로
																		     /// 1번째부터 가져오게끔 ++를 한다

			--InstUnCnt;                                                 // 리스트 수를 -1 해주어 경매에 올라간 유닛을 제외한 리스트 수로 맞춰준다

			for (int i = 0; i < InstUnCnt; i++)    // 현재 경매 리스트 수의 -1만큼 반복해준다
			{
				_instanceUnitsIDList[i] = _auctionOrderIDList[ActOrListPnt + i];         /// i번째의 경매리스트에 _auctionOrderList의 i+ActOrListPnt번쨰
																										 /// 리스트 값을 가져오고 i번쨰 _auctionPos의 자식으로 만들어준다

			}                                                                                            /// 위 과정을 반복문을 통해 _instanceUnitsList[i]를 재정렬

			_instanceUnitsIDList.RemoveAt(InstUnCnt);                                      /// 리스트 가장 마지막 리스트를 삭제해준다


			return ZeroListIndex;                                                                 // 가장 첫번째에 있던 경매 유닛을 반환해주어 현재 경매 유닛으로 설정한다

		}

		public void AuctionFailed()  // 아무도 경매에 참여하지 않았을 시 실행
		{

			int FailedUnit = NowAuctionUnitID;       /// 현재 경매 중인 유닛을 오브젝트 변수에 넣음
			NowAuctionUnitID = 17;                     /// 현재 경매 중인 유닛 삭제


			if (FailedUnit < 0 || 16 < FailedUnit )          // true일 경우 더 이상 가져올 경매 리스트가 없으므로 경매 종료
			{
				return;
			}

			_failedUnitsIDList.Add(FailedUnit);                                                                       /// 경매중이던 유닛을 유찰 리스트에 추가

			CPacket msg = CPacket.create((short)PROTOCOL.AUCTION_FAILED);
			msg.push(NowAuctionUnitID);                         // 어떤 유닛이 경매로 올라갔는지
			msg.push(_failedUnitsIDList);                           // 경매에 사용되는 모든 리스트 업데이트
			msg.push(_instanceUnitsIDList);
			msg.push(_auctionOrderIDList);
			broadcast(msg);                                           // 모두에게 전송
			Auction();                                                                                                      /// 유찰이 끝나면 다시 다음 유닛을 경매 시작

		}

		public void ReDefineList()
		{
			_instanceUnitsIDList = _failedUnitsIDList.ToList();

			ActOrListPnt = 0;
			FailedCount = 0;
			_failedUnitsIDList.Clear();
			_auctionOrderIDList.Clear();

			_auctionOrderIDList = _instanceUnitsIDList.ToList();

			bettingReset();

		}
		void Timer_Elapsed(object sender ,System.Timers.ElapsedEventArgs e)
        {
			AuctionTime -= 1;
        }

		void AuctionTimer()  // 상시로 실행되는 경매 시간 함수
		{
			ATimer = new Timer();
			ATimer.Interval = 1000;
			ATimer.Enabled = true;

			if (AuctionTime > 0)                                        // 경매 시간이 0초가 아니면
			{
				ATimer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);                      // 1초씩 감소
			}
			else if (NowBetGoldAmount == 0)                     // 경매 시간이 다 됐는데 아무도 입찰하지 않았다면 실행
			{
				bettingReset();                                           // 이후 입찰에 사용된 변수들 초기화
				AuctionFailed();                                          // 유찰 함수 실행
			}														        
			else if (NowBetGoldAmount > 0)                       // 경매 시간이 다 됐는데 누군가 입찰했다면
			{
				bettingReset();                                           // 이후 입찰에 사용된 변수들 초기화
				Auction();                                                 // 경매 함수 실행
			}
		}


		/// 게임 데이터를 초기화 한다.
		void bettingReset()
		{
			AuctionTime = 10;                                         // 경매시간을 다시 10초로 초기화
			NowBetGoldAmount = 0;                                // 현재경매 가격 초기화
			Bidder = "현재 입찰자";                                 // 현재 입찰자 초기화

			CPacket msg = CPacket.create((short)PROTOCOL.BETTINGRESET);
			this.players.ForEach(player =>
			{
				msg.push(player.player_index);      // 누구인지 구분하기 위한 플레이어 인덱스.


				player.myGold += player.MyBetGoldAmount;          // 
				player.MyBetGoldAmount = 0;
				msg.push(player.myGold);
				msg.push(player.MyBetGoldAmount);

			});
			msg.push(AuctionTime);                                          // 초기화되는 즉시 모두 업데이트해서
			msg.push(NowBetGoldAmount);                      
			msg.push(Bidder);
			broadcast(msg);                                                   // 모두에게 전송
		}

		void on_recv(CPacket msg)  // 받아온 패킷을 id를 통해 어떤 함수를 실행 시킬지 판단하는 함수
		{

			PROTOCOL protocol_id = (PROTOCOL)msg.pop_protocol_id();

			switch (protocol_id)
			{
				case PROTOCOL.BETTING:
					betting(msg);
					break;

			}
		}


		public void top_Bid()  // 누군가 상위 입찰을 할 경우 실행되는 함수
		{
			CPacket msg = CPacket.create((short)PROTOCOL.BETTINGFAILED);
			this.players.ForEach(player =>
			{
				msg.push(player.player_index);      // 누구인지 구분하기 위한 플레이어 인덱스.

				if (Bidder == player.playerName)                     // 현재 입찰자가 자신이라면
				{
					player.myGold += NowBetGoldAmount;             // 현재 경매가를 내 골드에 더해준다 입찰 실패시 경매급 환급
				}

				player.myGold += player.MyBetGoldAmount;          //  타인이 입찰하는 순간 올려놓은 자신의 경매금을 돌려받고
				player.MyBetGoldAmount = 0;                             // 입찰가를 0으로 초기화
				msg.push(player.myGold);
				msg.push(player.MyBetGoldAmount);
			});
			broadcast(msg);                                           // 모두에게 전송

		}
		public void betting(CPacket Rmsg)  // 입찰 시 실행되는 함수
		{
			int player_Index = Rmsg.pop_byte() - 1;

			CPlayer player = players[player_Index];

			if (player.MyBetGoldAmount > NowBetGoldAmount)                 // 입찰 시 내 입찰 가격이 현재 경매가 보다 커야 실행된다
			{
				if (Bidder == player.playerName)                                // 현재 입찰자가 자신이라면 입찰 비활성화한다
				{ 
					return;
				}
				top_Bid();
				NowBetGoldAmount = Rmsg.pop_int16();                   // 현재 입찰가를 클라이언트에서 요청한 입찰가로 초기화한다
				player.MyBetGoldAmount = 0;                                   // 내 입찰가를 초기화한다
				player.myGold -= NowBetGoldAmount;                       // 입찰시 돈이 빠져나간다
				AuctionTime = 10;                                                   // 남은 경매 시간을 10초로 초기화한다
                Bidder = player.playerName;                                     // 현채 입찰자를 자신으로 초기화한다

				CPacket msg = CPacket.create((short)PROTOCOL.BETTING);
				msg.push(player.playerName);                            // 누가
				msg.push(NowBetGoldAmount);                     // 얼마에 입찰했는지
				msg.push(player.myGold);                                // 남은 돈은 얼마인지
				broadcast(msg);                                           // 모두에게 전송
			}
        }

		/// 모든 경매가 끝났을때 실행
		public void Auction_over()
		{
			// 경매 끝
			
			CPacket msg = CPacket.create((short)PROTOCOL.AUCTION_OVER);
			msg.push();
			msg.push();
			msg.push();
			broadcast(msg);

			//방 제거.
			Program.game_main.room_manager.remove_room(this);
		}


		public void destroy()
		{
			CPacket msg = CPacket.create((short)PROTOCOL.ROOM_REMOVED);
			broadcast(msg);

			this.players.Clear();
		}
	}
}
