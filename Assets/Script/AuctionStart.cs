using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AuctionStart : MonoBehaviour
{
    public AuctionManager AM;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region 경매 기능
    public void Auction()
    {
        if (AM.NowAuctionUnit != null)
        {
            Destroy(AM.NowAuctionUnit);
        }
        AM.NowAuctionUnit = RemoteActionUnitList();
        AM.NowAuctionUnitPos.transform.localScale = new Vector3(5f, 5f, 1f);
        AM.NowAuctionUnit = Instantiate(AM.NowAuctionUnit, AM.NowAuctionUnitPos.transform);
        AM.NowAuctionUnit.transform.localPosition = Vector3.zero;

    }

    //AuctionUnit.transform.GetChild(ActOrListPnt-1).gameObject;
    //NowAuctionUnit = AuctionUnit.transform.GetChild(ActOrListPnt - 1).gameObject;
    // auctionunit에있는걸 가져와보려고했는데 실패함 

    public void AuctionFailed()
    {
        GameObject FailedUnit = RemoteActionUnitList();
        AM._FailedPos[AM.FailedCount].transform.localScale += new Vector3(2f, 2f, 1f);
        AM._failedUnitsList[AM.FailedCount] = Instantiate(FailedUnit, AM._FailedPos[AM.FailedCount].transform);
        FailedUnit.transform.localPosition = Vector3.zero;

        AM.FailedCount++;
    }


    public GameObject RemoteActionUnitList()
    {
        GameObject ZeroListIndex = AM._instanceUnitsList[0];// 중단점 반환을 복사의복사를 해대서 ㅈ됨
        int InstUnCnt = AM._instanceUnitsList.Count;
        AM.ActOrListPnt++;
        if (AM._instanceUnitsList == null)
        {
            return null; // 경매 리스트에 아무도 없으면 경매를 종료해야함
        }

        --InstUnCnt;
        for (int i = 0; i < InstUnCnt; i++)
        {

            GameObject FirstPos = GameObject.Find($"AuctionPos{i}");
            GameObject FirePosChild = FirstPos.transform.GetChild(0).gameObject;
            Destroy(FirePosChild);

            AM._instanceUnitsList[i] = Instantiate(AM._auctionOrderList[AM.ActOrListPnt + i], AM._auctionPos[i].transform);
            AM._instanceUnitsList[i].transform.localPosition = Vector3.zero;
        }
        Destroy(AM._instanceUnitsList[InstUnCnt]);
        AM._instanceUnitsList.RemoveAt(InstUnCnt);
        AM.RemoteAtCount++;
        return ZeroListIndex;

    }


    void Betting(int MyBetGold)
    {

    }

    int AddBettingMoney(Button button)
    {
        string BetGold = button.transform.GetChild(0).ToString();
        Debug.Log(BetGold);
        return int.Parse(BetGold);

    }
    #endregion
}
