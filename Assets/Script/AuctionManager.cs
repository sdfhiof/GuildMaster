using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AuctionManager : MonoBehaviour
{
    public List<GameObject> _auctionUnitList = new List<GameObject>();
    public List<GameObject> _auctionOrderList = new List<GameObject>();
    public List<GameObject> _instanceUnitsList = new List<GameObject>();
    public List<GameObject> _failedUnitsList = new List<GameObject>();
    public List<GameObject> _auctionPos = new List<GameObject>();
    public List<GameObject> _FailedPos = new List<GameObject>();


    public GameObject AuctionUnit;
    public GameObject NowAuctionUnit;
    public GameObject NowAuctionUnitPos;

    public int NowBetAmount = 0;
    public int MyBetAmount = 0;
    public int FailedCount = 0;
    public int AuctionIndex = 0;
    public int RemoteAtCount = 0;
    public int ActOrListPnt = 0;




    // Start is called before the first frame update
    void Start()
    {
        
        SetUnitList();

        Auction();
        //AuctionFailed();



    }

   
    void Update()
    {

    }

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

    public void Auction()
    { 
        if (NowAuctionUnit != null)
        {
            Destroy(NowAuctionUnit);
        }
        NowAuctionUnit = RemoteActionUnitList();
        NowAuctionUnitPos.transform.localScale += new Vector3(4f, 4f, 1f);
        NowAuctionUnit = Instantiate(NowAuctionUnit, NowAuctionUnitPos.transform);
    }

    public void AuctionFailed()
    {
        GameObject FailedUnit = RemoteActionUnitList();
        _FailedPos[FailedCount].transform.localScale += new Vector3(2f, 2f, 1f);
        _failedUnitsList[FailedCount] = Instantiate(FailedUnit, _FailedPos[FailedCount].transform);


        FailedCount++;
    }


    public GameObject RemoteActionUnitList()
    {
        GameObject ZeroListIndex = _instanceUnitsList[0];// 중단점 반환을 복사의복사를 해대서 ㅈ됨
        int InstUnCnt = _instanceUnitsList.Count;
        ActOrListPnt++;
        if (_instanceUnitsList == null)
        {
            return null; // 경매 리스트에 아무도 없으면 경매를 종료해야함
        }

        --InstUnCnt;
        for (int i = 0; i < InstUnCnt; i++)
         {
            
            GameObject FirstPos = GameObject.Find($"AuctionPos{i}");
            GameObject FirePosChild = FirstPos.transform.GetChild(0).gameObject;
            Destroy(FirePosChild);
            
            _instanceUnitsList[i] = Instantiate(_auctionOrderList[ActOrListPnt + i], _auctionPos[i].transform);
            
         }
        Destroy(_instanceUnitsList[InstUnCnt]);
        _instanceUnitsList.RemoveAt(InstUnCnt);
        RemoteAtCount++;
        return ZeroListIndex;

    }

}


