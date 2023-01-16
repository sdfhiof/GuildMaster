using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AuctionManager : MonoBehaviour
{
    public AuctionStart auctionStart;


    public List<GameObject> _auctionUnitList = new List<GameObject>();
    public List<GameObject> _auctionOrderList = new List<GameObject>();
    public List<GameObject> _instanceUnitsList = new List<GameObject>();
    public List<GameObject> _failedUnitsList = new List<GameObject>();
    public List<GameObject> _auctionPos = new List<GameObject>();
    public List<GameObject> _FailedPos = new List<GameObject>();

    public string Player1;
    public string Player2;
    public string Player3;
    public string Player4;


    public GameObject AuctionUnit;
    public GameObject NowAuctionUnit;
    public GameObject NowAuctionUnitPos;

    public Text NowBetGold;
    public int NowBetGoldAmount;

    public Text MyBetGold;
    public int MyBetGoldAmount;

    public Button Bet100;
    public Button Bet10;
    public Button Bet5;

    public int FailedCount = 0;
    public int AuctionIndex = 0;
    public int RemoteAtCount = 0;
    public int ActOrListPnt = 0;

    public Text TimeText;
    public float Time;

    
    // Start is called before the first frame update
    void Start()
    {
        
        SetUnitList();

        auctionStart.Auction();

        
        //Betting();

    }

   
    void Update()
    {
        NowBetGoldAmount = 0;
        MyBetGoldAmount = 0;
    }

    #region ��� �ý��� �ʱ� ����
    public void SetUnitList()
    {
        
        _auctionUnitList.Clear();
        int _auctionUnitListCount = 12;

        

        for (int i= 0 ; i< _auctionUnitListCount; i++)
        {
            _auctionUnitList.Add(AuctionUnit.transform.GetChild(i).gameObject);

           // if (AuctionUnit.transform.GetChild(i) == null) return;
            
        }

        AuctionOrder(ref _auctionUnitList);
        AuctionInOrder(ref _instanceUnitsList);
    }

    public void ShuffleList(ref List<GameObject> list)
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

    public void AuctionOrder(ref List<GameObject> list)
    {
        ShuffleList(ref list);
        _auctionOrderList = new List<GameObject>(list);
    }

    public void AuctionInOrder(ref List<GameObject> list)
    {
        for(int i = 0; i < 12; i++)
        {
            _auctionPos[i].transform.localScale += new Vector3(2f, 2f, 1f);
            list[i] = Instantiate(_auctionOrderList[i], _auctionPos[i].transform);
            
        }
       

    }

    #endregion

    

    #region ��� ���
    //public void Auction()
    //{ 
    //    if (NowAuctionUnit != null)
    //    {
    //        Destroy(NowAuctionUnit);
    //    }
    //    NowAuctionUnit = RemoteActionUnitList();
    //    NowAuctionUnitPos.transform.localScale = new Vector3(5f, 5f, 1f);
    //    NowAuctionUnit = Instantiate(NowAuctionUnit, NowAuctionUnitPos.transform);  
    //    NowAuctionUnit.transform.localPosition = Vector3.zero;

    //}

    ////AuctionUnit.transform.GetChild(ActOrListPnt-1).gameObject;
    ////NowAuctionUnit = AuctionUnit.transform.GetChild(ActOrListPnt - 1).gameObject;
    //// auctionunit���ִ°� �����ͺ������ߴµ� ������ 

    //public void AuctionFailed()
    //{
    //    GameObject FailedUnit = RemoteActionUnitList();
    //    _FailedPos[FailedCount].transform.localScale += new Vector3(2f, 2f, 1f);
    //    _failedUnitsList[FailedCount] = Instantiate(FailedUnit, _FailedPos[FailedCount].transform);
    //    FailedUnit.transform.localPosition = Vector3.zero;

    //    FailedCount++;
    //}


    //public GameObject RemoteActionUnitList()
    //{
    //    GameObject ZeroListIndex = _instanceUnitsList[0];// �ߴ��� ��ȯ�� �����Ǻ��縦 �ش뼭 ����
    //    int InstUnCnt = _instanceUnitsList.Count;
    //    ActOrListPnt++;
    //    if (_instanceUnitsList == null)
    //    {
    //        return null; // ��� ����Ʈ�� �ƹ��� ������ ��Ÿ� �����ؾ���
    //    }

    //    --InstUnCnt;
    //    for (int i = 0; i < InstUnCnt; i++)
    //     {

    //        GameObject FirstPos = GameObject.Find($"AuctionPos{i}");
    //        GameObject FirePosChild = FirstPos.transform.GetChild(0).gameObject;
    //        Destroy(FirePosChild);

    //        _instanceUnitsList[i] = Instantiate(_auctionOrderList[ActOrListPnt + i], _auctionPos[i].transform);
    //        _instanceUnitsList[i].transform.localPosition = Vector3.zero;
    //    }
    //    Destroy(_instanceUnitsList[InstUnCnt]);
    //    _instanceUnitsList.RemoveAt(InstUnCnt);
    //    RemoteAtCount++;
    //    return ZeroListIndex;

    //}


    //void Betting(int MyBetGold)
    //{

    //}

    //int AddBettingMoney(Button button)
    //{
    //   string BetGold = button.transform.GetChild(0).ToString();
    //    Debug.Log(BetGold);
    //    return int.Parse(BetGold);

    //}
    #endregion
}



