using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class AuctionManager : MonoBehaviour
{

    public List<GameObject> _auctionUnitList = new List<GameObject>();          /// ���ӸŴ����� �־�� ���� ���ֵ�
    public List<GameObject> _auctionOrderList = new List<GameObject>();        /// ������ ���� ����� ���� ���ֵ�
    public List<GameObject> _instanceUnitsList = new List<GameObject>();        /// ���ӳ� ��� ����Ʈ�� ��Ÿ���� �ν��Ͻ� �����յ�
    public List<GameObject> _failedUnitsList = new List<GameObject>();           /// ������ ���� ����Ʈ
    public List<GameObject> _auctionPos = new List<GameObject>();               /// ��� ����Ʈ�� ���ֵ��� ��ġ
    public List<GameObject> _failedPos = new List<GameObject>();                 /// ���� ����Ʈ�� ���ֵ� ��ġ

    public string PlayerName;                       /// �÷��̾� �̸� ���� ��Ʈ��ũ ����� �������� �̸��� �޾ƿ� ����
    public List<GameObject> _bidUnits;           /// �÷��̾ ���� ������ ������ ������ ������ ����Ʈ
    public Text Gold;                                  /// �÷��̾� �ڱ� �ؽ�Ʈ ����
    public int MyGold;                                /// Gold �ؽ�Ʈ ������Ʈ�� ���� int ���� �� ���� ������� 
                                                           ///  Gold.text = MyGold.ToString()���� �ؽ�Ʈ ����

    public GameObject AuctionUnit;                      /// ���� ���ֵ��� �����ϴ� ������Ʈ
    public GameObject NowAuctionUnit;                /// ���� ������� ������ ���� ������Ʈ
    public GameObject NowAuctionUnitPos;           /// NowAuctionUnit�� ��ġ�� ����ִ� ������Ʈ

    public Text Bidder;                                      /// ���� �������� �÷��̾� �̸� �ؽ�Ʈ ����
    public string NowBidder;                              /// Bidder �ؽ�Ʈ ������Ʈ�� ���� string���� �÷��̾� �̸��� ��

    public Text NowBetGold;                     /// ���� ��Ű� �ؽ�Ʈ ����
    public int NowBetGoldAmount;             /// NowBetGold �ؽ�Ʈ ������Ʈ�� ���� ���� int ���� �� ���� ������� 
                                                        /// NowBetGold.text = NowBetGoldAmount.ToString()�� �ؽ�Ʈ ����

    public Text MyBetGold;                      /// ���� �����Ϸ��� �� �ؽ�Ʈ ����
    public int MyBetGoldAmount;              /// NowBetGold �ؽ�Ʈ ������Ʈ�� ���� ���� int ���� �� ���� ������� 
                                                       /// MyBetGold.text = MyBetGoldAmount.ToString()�� �ؽ�Ʈ ����

    public Button Bet100;                     /// ����+ ��ư
    public Button Bet10;                      /// ����+ ��ư
    public Button Bet5;                        /// ����+ ��ư

    public int FailedCount = 0;              /// ���� Ƚ�� ī��Ʈ
    public int ActOrListPnt = 0;             /// _auctionOrderList�� ��ġ ������ ���� ����

    public Text TimeText;         /// ���� ��� ���� �ð���ǥ�� �ؽ�Ʈ ����
    public float Timer;             /// TimeText �ؽ�Ʈ ������Ʈ�� ���� ���� float ���� �� ���� ������� 
                                        /// TimeText.text = Mathf.Ceil(Timer).ToString()�� �ؽ�Ʈ ����

    void Start()
    {
        Timer = 2f;                               /// �ʱ� UI�� �ʿ���  ���� �ʱ�ȭ
        NowBetGold.text = "0";                 /// �ʱ� UI�� �ʿ���  ���� �ʱ�ȭ
        MyBetGold.text = "0";                   /// �ʱ� UI�� �ʿ���  ���� �ʱ�ȭ
        NowBidder = "���� ������";           /// �ʱ� UI�� �ʿ���  ���� �ʱ�ȭ



        SetUnitList();                               

        Auction();

       

    }


    void Update()
    {
        NowBetGold.text = NowBetGoldAmount.ToString();               ///  update���� ���������� ����� �ǽð����� ���ŵǴ� UI��
        MyBetGold.text = MyBetGoldAmount.ToString();                   ///  update���� ���������� ����� �ǽð����� ���ŵǴ� UI��
        Bidder.text = NowBidder;                                                 ///  update���� ���������� ����� �ǽð����� ���ŵǴ� UI��
        Gold.text = MyGold.ToString();                                          ///  update���� ���������� ����� �ǽð����� ���ŵǴ� UI��
        AuctionTimer();                                                              /// ��� Ÿ�̸� Ȱ��ȭ ��Ű� ������ ���� ��Ű� �̷������ �ϹǷ� update�� ��ġ
    }

    #region ��� �ý��� �ʱ� ���� ���� ��Ʈ��ũ�� �Ѿ ����
    public void SetUnitList()  // �ʱ� ��ž��� Ȱ��ȭ �Ǹ� ����
    {
        
        _auctionUnitList.Clear();                     //  ����Ʈ�� �ʱ�ȭ
        int _auctionUnitListCount = 12;           // ����Ʈ�� �� �ν��Ͻ� ��

        

        for (int i= 0 ; i< _auctionUnitListCount; i++)         // �ݺ������� ����Ʈ�� ���ӿ�����Ʈ �߰�
        {
            _auctionUnitList.Add(AuctionUnit.transform.GetChild(i).gameObject); // AuctionUnit�� �ڽĿ�����Ʈ�� ������� ������

            // if (AuctionUnit.transform.GetChild(i) == null) return;

        }

        AuctionOrder(ref _auctionUnitList);                   /// SetUnitList�� ���� _auctionUnitList�� �����ؼ� _auctionOrderList�� ����
        AuctionInOrder(ref _instanceUnitsList);              /// AuctionOrder�� ���� _auctionOrderList�� �̿��� _instanceUnitsList�� ����
    }

    public void ShuffleList(ref List<GameObject> list)    // ����Ʈ�� �������ִ� �Լ�
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

    public void AuctionOrder(ref List<GameObject> list)  // ������ ����Ʈ�� _auctionOrderList�� �ٽ� ����
    {
        ShuffleList(ref list);
        _auctionOrderList = new List<GameObject>(list);
    }

    public void AuctionInOrder(ref List<GameObject> list)  // ������ ����Ʈ�� _instanceUnitsList�� ����
    {
        for(int i = 0; i < 12; i++)
        {
            _auctionPos[i].transform.localScale += new Vector3(2f, 2f, 1f);               /// ������ �ν��Ͻ� ũ�� ������ ���� _auctionPosũ�⸦ ������
            list[i] = Instantiate(_auctionOrderList[i], _auctionPos[i].transform);           /// _auctionOrderList�� i��° ������Ʈ�� _auctionPos[i]�� �ڽ�����
                                                                                                          /// �ν��Ͻ�ȭ ����Ʈ ����ŭ �ݺ�
           
        }
       

    }

    #endregion



    #region ��� ���
    public void Auction()   // ���� ��� ���� �׸��� �� ��Ű� ���� ������ ����  ������ �Ѿ��
    {
        if (NowAuctionUnit != null)  // ���� ������������� ������
        {
            if(NowBidder == PlayerName)  // ���� �����ڰ� �÷��̾���
            {
                GameObject BidUnit = Instantiate(NowAuctionUnit);  /// ���� ������Ʈ ������ ����� ���� ��� ������ ����
                _bidUnits.Add(BidUnit);                                       /// �� ���� ������Ʈ�� ����Ʈ�� ����
            }
            Destroy(NowAuctionUnit);     // ���� ������� ���� ���� (���� ��Ű� �����ٴ� �ǹ�)
            
        }

        if (_failedUnitsList.Count != 0 && _instanceUnitsList.Count == 0)  // ������ ������ �����ϰ� ��������� ���ٸ� ������ �Լ� ����
        {
            ReDefineList();
        }

        NowAuctionUnit = RemoveActionUnitList();  // ��� ���� ���� 

        if (NowAuctionUnit == null)     // true�� ��� �� �̻� ������ ��� ����Ʈ�� �����Ƿ� ��� ����
        {
            return;
        }
        NowAuctionUnitPos.transform.localScale = new Vector3(5f, 5f, 1f);                             ///������ �ν��Ͻ� ũ�� ������ ����
                                                                                                                          /// NowAuctionUnitPosũ�⸦ ������
        NowAuctionUnit = Instantiate(NowAuctionUnit, NowAuctionUnitPos.transform);            /// NowAuctionUnit�� NowAuctionUnitPos�� �ڽ����� �ν��Ͻ�ȭ                                 
        NowAuctionUnit.transform.localPosition = Vector3.zero;                                           /// NowAuctionUnit�� ��ġ�� �θ��� ��ġ�� ����

        
    }

    //AuctionUnit.transform.GetChild(ActOrListPnt-1).gameObject;
    //NowAuctionUnit = AuctionUnit.transform.GetChild(ActOrListPnt - 1).gameObject;
    // auctionunit���ִ°� �����ͺ������ߴµ� ������ 

    public void AuctionFailed()  // �ƹ��� ��ſ� �������� �ʾ��� �� ����  ������ �Ѿ��
    {
       
        GameObject FailedUnit = NowAuctionUnitPos.transform.GetChild(0).gameObject;         /// ���� ��� ���� ������ ������Ʈ ������ ����
        Destroy(NowAuctionUnit);                                                                                /// ���� ��� ���� ���� ����

        if (FailedUnit == null)          // true�� ��� �� �̻� ������ ��� ����Ʈ�� �����Ƿ� ��� ����
        {
            return;
        }
        _failedPos[FailedCount].transform.localScale = new Vector3(3f, 3f, 1f);                         /// ������ �ν��Ͻ� ũ�� ������ ����
                                                                                                                          /// NowAuctionUnitPosũ�⸦ ������
        _failedUnitsList.Add(Instantiate(FailedUnit, _failedPos[FailedCount].transform));              /// FailedCount�� �ν��Ͻ�ȭ �� ������Ʈ��
                                                                                                                                        /// ����Ʈ�� _FailedPos�� ��ġ�� ����
        FailedUnit.transform.localPosition = Vector3.zero;                                                                /// FailedUnit�� ��ġ�� �θ��� ��ġ�� ����

        FailedCount++;                                                                                                           ///FailedCount�� �������� ���� AuctionFailed �����
                                                                                                                                       /// ���� ����Ʈ�� _FailedPos ��ġ�� ��ĭ�� �о��ش�
        Auction();                                                                                                                  /// ������ ������ �ٽ� ���� ������ ��� ����

    }


    public GameObject RemoveActionUnitList()  //��� ���۽� ��� ��ġ�� �ö󰡴� ���� ���� �� ��� ����Ʈ ���� �Լ�  ������ �Ѿ��
    {
        if (_instanceUnitsList.Count != 0 )  // false�� ��� �� �̻� ������ ��� ����Ʈ�� �����Ƿ� ��� ����
        {
            GameObject ZeroListIndex = _instanceUnitsList[0];           /// ���� ��� ���ֿ� �ö� 0��° ����Ʈ�� �־��ش�
            int InstUnCnt = _instanceUnitsList.Count;                      /// InstUnCnt�� ����Ʈ ����ŭ �����ش�
            ActOrListPnt++;                                                      /// _instanceUnitsList�� ��ܿ��� ���� ���� ����
                                                                                      /// 0��° ����Ʈ ���� ���� ��� �������� �ø��Ƿ�
                                                                                      /// 1��°���� �������Բ� ++�� �Ѵ�
                                                                                      
            --InstUnCnt;                                                          // ����Ʈ ���� -1 ���־� ��ſ� �ö� ������ ������ ����Ʈ ���� �����ش�

            for (int i = 0; i < InstUnCnt; i++)    // ���� ��� ����Ʈ ���� -1��ŭ �ݺ����ش�
            {
                Debug.Log(InstUnCnt);
                GameObject FirstPos = GameObject.Find($"AuctionPos{i}");                                                       /// i���� AuctionPos�� �����´�
                GameObject FirstPosChild = FirstPos.transform.GetChild(0).gameObject;                                       ///FirstPos�� �ڽĿ� �����Ѵ�
                Destroy(FirstPosChild);                                                                                                       /// �ڽ� ������Ʈ�� �����Ѵ�
                                                                                                                                                   
                _instanceUnitsList[i] = Instantiate(_auctionOrderList[ActOrListPnt + i], _auctionPos[i].transform);         /// i��°�� ��Ÿ���Ʈ�� _auctionOrderList�� i+ActOrListPnt����
                                                                                                                                                   /// ����Ʈ ���� �������� i���� _auctionPos�� �ڽ����� ������ش�
                _instanceUnitsList[i].transform.localPosition = Vector3.zero;                                                       ///_instanceUnitsList[i]�� ��ġ�� �θ��� ��ġ�� ����

            }                                                                                                                                      /// �� ������ �ݺ����� ���� _instanceUnitsList[i]�� ������

            Destroy(_instanceUnitsList[InstUnCnt]);                                                                                     /// �� ������ ������ ����Ʈ �ǳ����� ���� �ν��Ͻ��� �ΰ� �ִµ�
                                                                                                                                                  /// ���� ������ �ν��Ͻ��� �������ش�
            _instanceUnitsList.RemoveAt(InstUnCnt);                                                                                   /// ����Ʈ ���� ���� ������ ����Ʈ�� �������ش�

           
            return ZeroListIndex;                                                                                                            // ���� ù��°�� �ִ� ��� ������ ��ȯ���־� ���� ��� �������� �����Ѵ�

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

    

    void AuctionTimer()  // ������Ʈ���� ��÷� ����Ǵ� ��� �ð� �Լ�  ������ �Ѿ��
    {

        TimeText.text = Mathf.Ceil(Timer).ToString();  //  update���� ���������� ����� �ǽð����� ���ŵǴ� UI 

        if (Timer > 0)                                        // ��� �ð��� 0�ʰ� �ƴϸ�
        {                                                        
            Timer -= Time.deltaTime;                     // 1�ʾ� ����
        }                                                        
        else if (NowBetGoldAmount == 0)             // ��� �ð��� �� �ƴµ� �ƹ��� �������� �ʾҴٸ� ����
        {                                                        
            AuctionFailed();                                 // ���� �Լ� ����
            BettingReset();                                  // ���� ������ ���� ������ �ʱ�ȭ
        }                                                        
        else if (NowBetGoldAmount > 0)               // ��� �ð��� �� �ƴµ� ������ �����ߴٸ�
        {                                                        
            Auction();                                         // ��� �Լ� ����
            BettingReset();                                   // ���� ������ ���� ������ �ʱ�ȭ
        }
    }

    void BettingReset() // ������ ������ ������ ���� ������ �ʱ�ȭ
    {
        Timer = 2f;                                         // ��Žð��� �ٽ� 15�ʷ� �ʱ�ȭ
        NowBetGoldAmount = 0;                        // ������ ���� �ʱ�ȭ
        MyGold += MyBetGoldAmount;               // ���� ������ ������ �� ������ �������� ���¸� �ٽ� �� ��忡 �����ش�
        MyBetGoldAmount = 0;                         //  �׸��� �� ���� ���� �ʱ�ȭ
        NowBidder = "���� ������";                    // ���� ������ �ʱ�ȭ
    }

    public void Betting()  // ���� ��ư �Լ�
    {
        if(MyBetGoldAmount > NowBetGoldAmount)                 // ���� �� �� ���� ������ ���� ��Ű� ���� Ŀ�� ����ȴ�
        {                                                                          
            if(NowBidder == PlayerName)                                // ���� �����ڰ� �ڽ��̶�� ���� ��Ȱ��ȭ�Ѵ�
            {                                                                      
                return;                                                           
            }                                                                      
            NowBetGoldAmount = MyBetGoldAmount;               // ���� �������� �� �������� �ʱ�ȭ�Ѵ�
            MyBetGoldAmount = 0;                                        // �� �������� �ʱ�ȭ�Ѵ�
            Timer = 10f;                                                       // ���� ��� �ð��� 10�ʷ� �ʱ�ȭ�Ѵ�
            NowBidder = PlayerName;                                     // ��ä �����ڸ� �ڽ����� �ʱ�ȭ�Ѵ�
        }
        
    }

    public void ResetMyBetting()  //�� ���� ������ �ʱ�ȭ �ϴ� ��ư �Լ�
    {
        MyGold += MyBetGoldAmount;  // �������� �ٽ� ����忡 �����ش�
        MyBetGoldAmount = 0;            // �׸��� �������� �ʱ�ȭ�Ѵ�
    }

    public void AddBettingMoney(Button button)  //�������� �����ִ� ��ư Ŭ���� ����Ǵ� �Լ�
    {
        string BetGold = button.transform.GetChild(0).name;          // ��ư�� ù��° �ڽ��� �̸��� ������ �����Ѵ�
                                                                                     //  ��ư�� ù��° �ڽ� �̸��� ��ư ���ݰ� ����
        int Gold = int.Parse(BetGold);                                       //  ��ư�� �̸��� ������ ��ȯ 100 10 5
       
        if (MyGold > Gold)  // ����尡 ��ư ���ݺ��� �� ���ƾ� ����ȴ�
        {
            MyBetGoldAmount += Gold;                                   // ��ư ���ݸ�ŭ �� �������� ���ϱ�
            MyGold -= Gold;                                                  // ���ÿ� �� ��带 ��ư ���ݸ�ŭ ����
        }
        
    }


    public void BidFail()  // ������ ���� ������ �� ��� ����Ǵ� �Լ�
    {
        ResetMyBetting();                                      // ���� ���� �Լ� ����
        if (NowBidder == PlayerName)                     // ���� �����ڰ� �ڽ��̶��
        {                                                            
            MyGold += NowBetGoldAmount;             // ���� ��Ű��� �� ��忡 �����ش� ���� ���н� ��ű� ȯ��
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



