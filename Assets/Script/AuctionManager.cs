using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class AuctionManager : MonoBehaviour
{

    public List<GameObject> _auctionUnitList = new List<GameObject>();          /// 게임매니저에 넣어둘 원본 유닛들
    public List<GameObject> _auctionOrderList = new List<GameObject>();        /// 셔플을 통해 섞어둔 원본 유닛들
    public List<GameObject> _instanceUnitsList = new List<GameObject>();        /// 게임내 경매 리스트에 나타나는 인스턴스 프리팹들
    public List<GameObject> _failedUnitsList = new List<GameObject>();           /// 유찰된 유닛 리스트
    public List<GameObject> _auctionPos = new List<GameObject>();               /// 경매 리스트의 유닛들의 위치
    public List<GameObject> _failedPos = new List<GameObject>();                 /// 유찰 리스트의 유닛들 위치

    public string PlayerName;                       /// 플레이어 이름 차후 네트워크 연결시 서버에서 이름을 받아올 예정
    public List<GameObject> _bidUnits;           /// 플레이어가 입찰 성공시 입찰한 유닛을 저장할 리스트
    public Text Gold;                                  /// 플레이어 자금 텍스트 형식
    public int MyGold;                                /// Gold 텍스트 업데이트를 위한 int 변수 이 값을 변경시켜 
                                                           ///  Gold.text = MyGold.ToString()으로 텍스트 변경

    public GameObject AuctionUnit;                      /// 원본 유닛들을 저장하는 오브젝트
    public GameObject NowAuctionUnit;                /// 현재 경매중인 유닛이 들어가는 오브젝트
    public GameObject NowAuctionUnitPos;           /// NowAuctionUnit의 위치를 잡아주는 오브젝트

    public Text Bidder;                                      /// 현재 입찰중인 플레이어 이름 텍스트 형식
    public string NowBidder;                              /// Bidder 텍스트 업데이트를 위한 string형식 플레이어 이름이 들어감

    public Text NowBetGold;                     /// 현재 경매값 텍스트 형식
    public int NowBetGoldAmount;             /// NowBetGold 텍스트 업데이트를 위한 위한 int 변수 이 값을 변경시켜 
                                                        /// NowBetGold.text = NowBetGoldAmount.ToString()로 텍스트 변경

    public Text MyBetGold;                      /// 내가 입찰하려는 값 텍스트 형식
    public int MyBetGoldAmount;              /// NowBetGold 텍스트 업데이트를 위한 위한 int 변수 이 값을 변경시켜 
                                                       /// MyBetGold.text = MyBetGoldAmount.ToString()로 텍스트 변경

    public Button Bet100;                     /// 입찰+ 버튼
    public Button Bet10;                      /// 입찰+ 버튼
    public Button Bet5;                        /// 입찰+ 버튼

    public int FailedCount = 0;              /// 유찰 횟수 카운트
    public int ActOrListPnt = 0;             /// _auctionOrderList의 위치 참조를 위한 변수

    public Text TimeText;         /// 현재 경매 남은 시간을표현 텍스트 형식
    public float Timer;             /// TimeText 텍스트 업데이트를 위한 위한 float 변수 이 값을 변경시켜 
                                        /// TimeText.text = Mathf.Ceil(Timer).ToString()로 텍스트 변경

    void Start()
    {
        Timer = 2f;                               /// 초기 UI에 필요한  변수 초기화
        NowBetGold.text = "0";                 /// 초기 UI에 필요한  변수 초기화
        MyBetGold.text = "0";                   /// 초기 UI에 필요한  변수 초기화
        NowBidder = "현재 입찰자";           /// 초기 UI에 필요한  변수 초기화



        SetUnitList();                               

        Auction();

       

    }


    void Update()
    {
        NowBetGold.text = NowBetGoldAmount.ToString();               ///  update에서 지속적으로 실행돼 실시간으로 갱신되는 UI들
        MyBetGold.text = MyBetGoldAmount.ToString();                   ///  update에서 지속적으로 실행돼 실시간으로 갱신되는 UI들
        Bidder.text = NowBidder;                                                 ///  update에서 지속적으로 실행돼 실시간으로 갱신되는 UI들
        Gold.text = MyGold.ToString();                                          ///  update에서 지속적으로 실행돼 실시간으로 갱신되는 UI들
        AuctionTimer();                                                              /// 경매 타이머 활성화 경매가 끝나도 다음 경매가 이루어져야 하므로 update에 배치
    }

    #region 경매 시스템 초기 생성 전부 네트워크로 넘어갈 예정
    public void SetUnitList()  // 초기 경매씬이 활성화 되면 실행
    {
        
        _auctionUnitList.Clear();                     //  리스트를 초기화
        int _auctionUnitListCount = 12;           // 리스트에 들어갈 인스턴스 수

        

        for (int i= 0 ; i< _auctionUnitListCount; i++)         // 반복문으로 리스트에 게임오브젝트 추가
        {
            _auctionUnitList.Add(AuctionUnit.transform.GetChild(i).gameObject); // AuctionUnit의 자식오브젝트를 순서대로 가져옴

            // if (AuctionUnit.transform.GetChild(i) == null) return;

        }

        AuctionOrder(ref _auctionUnitList);                   /// SetUnitList로 만든 _auctionUnitList를 셔플해서 _auctionOrderList를 생성
        AuctionInOrder(ref _instanceUnitsList);              /// AuctionOrder로 만든 _auctionOrderList를 이용해 _instanceUnitsList를 생성
    }

    public void ShuffleList(ref List<GameObject> list)    // 리스트를 셔플해주는 함수
    {
        
        int to = list.Count;
        while (to > 1)
        {
            
            int from = Random.Range(0, --to);
            GameObject tmp = list[from];
            list[from] = list[to];
            list[to] = tmp;
            
        }
        
    }

    public void AuctionOrder(ref List<GameObject> list)  // 셔플한 리스트로 _auctionOrderList를 다시 생성
    {
        ShuffleList(ref list);
        _auctionOrderList = new List<GameObject>(list);
    }

    public void AuctionInOrder(ref List<GameObject> list)  // 셔플한 리스트로 _instanceUnitsList를 생성
    {
        for(int i = 0; i < 12; i++)
        {
            _auctionPos[i].transform.localScale += new Vector3(2f, 2f, 1f);               /// 생성한 인스턴스 크기 조정을 위해 _auctionPos크기를 조정함
            list[i] = Instantiate(_auctionOrderList[i], _auctionPos[i].transform);           /// _auctionOrderList의 i번째 오브젝트를 _auctionPos[i]의 자식으로
                                                                                                          /// 인스턴스화 리스트 수만큼 반복
           
        }
       

    }

    #endregion



    #region 경매 기능
    public void Auction()   // 최조 경매 시작 그리고 매 경매가 끝날 때마다 실행  서버로 넘어갈듯
    {
        if (NowAuctionUnit != null)  // 현재 경매중인유닛이 있으면
        {
            if(NowBidder == PlayerName)  // 현재 입찰자가 플레이어라면
            {
                GameObject BidUnit = Instantiate(NowAuctionUnit);  /// 게임 오브젝트 변수를 만들어 현재 경매 유닛을 저장
                _bidUnits.Add(BidUnit);                                       /// 위 게임 오브젝트를 리스트에 저장
            }
            Destroy(NowAuctionUnit);     // 현재 경매중인 유닛 삭제 (이전 경매가 끝났다는 의미)
            
        }

        if (_failedUnitsList.Count != 0 && _instanceUnitsList.Count == 0)  // 유찰된 유닛이 존재하고 경매유닛이 없다면 재정렬 함수 실행
        {
            ReDefineList();
        }

        NowAuctionUnit = RemoveActionUnitList();  // 경매 유닛 설정 

        if (NowAuctionUnit == null)     // true일 경우 더 이상 가져올 경매 리스트가 없으므로 경매 종료
        {
            return;
        }
        NowAuctionUnitPos.transform.localScale = new Vector3(5f, 5f, 1f);                             ///생성한 인스턴스 크기 조정을 위해
                                                                                                                          /// NowAuctionUnitPos크기를 조정함
        NowAuctionUnit = Instantiate(NowAuctionUnit, NowAuctionUnitPos.transform);            /// NowAuctionUnit을 NowAuctionUnitPos의 자식으로 인스턴스화                                 
        NowAuctionUnit.transform.localPosition = Vector3.zero;                                           /// NowAuctionUnit의 위치를 부모의 위치로 설정

        
    }

    //AuctionUnit.transform.GetChild(ActOrListPnt-1).gameObject;
    //NowAuctionUnit = AuctionUnit.transform.GetChild(ActOrListPnt - 1).gameObject;
    // auctionunit에있는걸 가져와보려고했는데 실패함 

    public void AuctionFailed()  // 아무도 경매에 참여하지 않았을 시 실행  서버로 넘어갈듯
    {
       
        GameObject FailedUnit = NowAuctionUnitPos.transform.GetChild(0).gameObject;         /// 현재 경매 중인 유닛을 오브젝트 변수에 넣음
        Destroy(NowAuctionUnit);                                                                                /// 현재 경매 중인 유닛 삭제

        if (FailedUnit == null)          // true일 경우 더 이상 가져올 경매 리스트가 없으므로 경매 종료
        {
            return;
        }
        _failedPos[FailedCount].transform.localScale = new Vector3(3f, 3f, 1f);                         /// 생성한 인스턴스 크기 조정을 위해
                                                                                                                          /// NowAuctionUnitPos크기를 조정함
        _failedUnitsList.Add(Instantiate(FailedUnit, _failedPos[FailedCount].transform));              /// FailedCount로 인스턴스화 된 오브젝트를
                                                                                                                                        /// 리스트와 _FailedPos의 위치를 결정
        FailedUnit.transform.localPosition = Vector3.zero;                                                                /// FailedUnit의 위치를 부모의 위치로 설정

        FailedCount++;                                                                                                           ///FailedCount를 증가시켜 다음 AuctionFailed 실행시
                                                                                                                                       /// 다음 리스트와 _FailedPos 위치를 한칸씩 밀어준다
        Auction();                                                                                                                  /// 유찰이 끝나면 다시 다음 유닛을 경매 시작

    }


    public GameObject RemoveActionUnitList()  //경매 시작시 경매 위치로 올라가는 유닛 삭제 및 경매 리스트 정렬 함수  서버로 넘어갈듯
    {
        if (_instanceUnitsList.Count != 0 )  // false일 경우 더 이상 가져올 경매 리스트가 없으므로 경매 종료
        {
            GameObject ZeroListIndex = _instanceUnitsList[0];           /// 현재 경매 유닛에 올라갈 0번째 리스트를 넣어준다
            int InstUnCnt = _instanceUnitsList.Count;                      /// InstUnCnt을 리스트 수만큼 맞춰준다
            ActOrListPnt++;                                                      /// _instanceUnitsList를 당겨오기 위해 쓰는 변수
                                                                                      /// 0번째 리스트 값은 현재 경매 유닛으로 올리므로
                                                                                      /// 1번째부터 가져오게끔 ++를 한다
                                                                                      
            --InstUnCnt;                                                          // 리스트 수를 -1 해주어 경매에 올라간 유닛을 제외한 리스트 수로 맞춰준다

            for (int i = 0; i < InstUnCnt; i++)    // 현재 경매 리스트 수의 -1만큼 반복해준다
            {
                Debug.Log(InstUnCnt);
                GameObject FirstPos = GameObject.Find($"AuctionPos{i}");                                                       /// i번쨰 AuctionPos를 가져온다
                GameObject FirstPosChild = FirstPos.transform.GetChild(0).gameObject;                                       ///FirstPos의 자식에 접근한다
                Destroy(FirstPosChild);                                                                                                       /// 자식 오브젝트를 삭제한다
                                                                                                                                                   
                _instanceUnitsList[i] = Instantiate(_auctionOrderList[ActOrListPnt + i], _auctionPos[i].transform);         /// i번째의 경매리스트에 _auctionOrderList의 i+ActOrListPnt번쨰
                                                                                                                                                   /// 리스트 값을 가져오고 i번쨰 _auctionPos의 자식으로 만들어준다
                _instanceUnitsList[i].transform.localPosition = Vector3.zero;                                                       ///_instanceUnitsList[i]의 위치를 부모의 위치로 설정

            }                                                                                                                                      /// 위 과정을 반복문을 통해 _instanceUnitsList[i]를 재정렬

            Destroy(_instanceUnitsList[InstUnCnt]);                                                                                     /// 위 과정이 끝나면 리스트 맨끝에는 같은 인스턴스가 두개 있는데
                                                                                                                                                  /// 가장 마지막 인스턴스를 삭제해준다
            _instanceUnitsList.RemoveAt(InstUnCnt);                                                                                   /// 리스트 역시 가장 마지막 리스트를 삭제해준다

           
            return ZeroListIndex;                                                                                                            // 가장 첫번째에 있던 경매 유닛을 반환해주어 현재 경매 유닛으로 설정한다

        }
        else
        {
            return null;
        }
    }

    public void ReDefineList()  //
    {
        for(int i = 0; i < _auctionOrderList.Count; i++)
        {
            Destroy(_auctionOrderList[i]);
        }

        for (int i = 0; i < _failedUnitsList.Count; i++)
        {
            _instanceUnitsList.Add(new GameObject());
            
        }
        Debug.Log(_failedUnitsList.Count);
        for (int k = 0; k < _failedUnitsList.Count ; k++)
        {
            _instanceUnitsList[k] = Instantiate(_failedUnitsList[k], _auctionPos[k].transform);
            GameObject FailedPos = GameObject.Find($"FailedPos{k}");
            GameObject FirstPosChild = FailedPos.transform.GetChild(0).gameObject;
            Destroy(FirstPosChild);
            
        }

        ActOrListPnt = 0;
        FailedCount = 0;
        _failedUnitsList.Clear();
        _auctionOrderList.Clear();
        
        for (int i = 0; i < _instanceUnitsList.Count; i++)
        {
            _auctionOrderList.Add(Instantiate(_instanceUnitsList[i]));
        }
        
        BettingReset();
        


    }

    

    void AuctionTimer()  // 업데이트에서 상시로 실행되는 경매 시간 함수  서버로 넘어갈듯
    {

        TimeText.text = Mathf.Ceil(Timer).ToString();  //  update에서 지속적으로 실행돼 실시간으로 갱신되는 UI 

        if (Timer > 0)                                        // 경매 시간이 0초가 아니면
        {                                                        
            Timer -= Time.deltaTime;                     // 1초씩 감소
        }                                                        
        else if (NowBetGoldAmount == 0)             // 경매 시간이 다 됐는데 아무도 입찰하지 않았다면 실행
        {                                                        
            AuctionFailed();                                 // 유찰 함수 실행
            BettingReset();                                  // 이후 입찰에 사용된 변수들 초기화
        }                                                        
        else if (NowBetGoldAmount > 0)               // 경매 시간이 다 됐는데 누군가 입찰했다면
        {                                                        
            Auction();                                         // 경매 함수 실행
            BettingReset();                                   // 이후 입찰에 사용된 변수들 초기화
        }
    }

    void BettingReset() // 입찰이 끝나고 입찰에 사용된 변수들 초기화
    {
        Timer = 2f;                                         // 경매시간을 다시 15초로 초기화
        NowBetGoldAmount = 0;                        // 현재경매 가격 초기화
        MyGold += MyBetGoldAmount;               // 만약 입찰이 끝날때 내 입찰을 눌러놓은 상태면 다시 내 골드에 더해준다
        MyBetGoldAmount = 0;                         //  그리고 내 입찰 가격 초기화
        NowBidder = "현재 입찰자";                    // 현재 입찰자 초기화
    }

    public void Betting()  // 입찰 버튼 함수
    {
        if(MyBetGoldAmount > NowBetGoldAmount)                 // 입찰 시 내 입찰 가격이 현재 경매가 보다 커야 실행된다
        {                                                                          
            if(NowBidder == PlayerName)                                // 현재 입찰자가 자신이라면 입찰 비활성화한다
            {                                                                      
                return;                                                           
            }                                                                      
            NowBetGoldAmount = MyBetGoldAmount;               // 현재 입찰가를 내 입찰가로 초기화한다
            MyBetGoldAmount = 0;                                        // 내 입찰가를 초기화한다
            Timer = 10f;                                                       // 남은 경매 시간을 10초로 초기화한다
            NowBidder = PlayerName;                                     // 현채 입찰자를 자신으로 초기화한다
        }
        
    }

    public void ResetMyBetting()  //내 입찰 가격을 초기화 하는 버튼 함수
    {
        MyGold += MyBetGoldAmount;  // 입찰가를 다시 내골드에 더해준다
        MyBetGoldAmount = 0;            // 그리고 입찰가를 초기화한다
    }

    public void AddBettingMoney(Button button)  //입찰가를 더해주는 버튼 클릭시 실행되는 함수
    {
        string BetGold = button.transform.GetChild(0).name;          // 버튼의 첫번째 자식의 이름을 가져와 저장한다
                                                                                     //  버튼의 첫번째 자식 이름은 버튼 가격과 동일
        int Gold = int.Parse(BetGold);                                       //  버튼의 이름을 정수로 변환 100 10 5
       
        if (MyGold > Gold)  // 내골드가 버튼 가격보다 더 많아야 실행된다
        {
            MyBetGoldAmount += Gold;                                   // 버튼 가격만큼 내 입찰가에 더하기
            MyGold -= Gold;                                                  // 동시에 내 골드를 버튼 가격만큼 차감
        }
        
    }


    public void BidFail()  // 누군가 상위 입찰을 할 경우 실행되는 함수
    {
        ResetMyBetting();                                      // 배팅 리셋 함수 실행
        if (NowBidder == PlayerName)                     // 현재 입찰자가 자신이라면
        {                                                            
            MyGold += NowBetGoldAmount;             // 현재 경매가를 내 골드에 더해준다 입찰 실패시 경매급 환급
        }
        

    }    
    
    public void TopBidder()
    {
        BidFail();
        NowBidder = "Player2";
        NowBetGoldAmount = 140;
        Timer = 2f;
    }

    

    #endregion
}



