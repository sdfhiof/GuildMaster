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
        AM.Time = 15f;
        AM.NowBetGold.text = "0";
        AM.MyBetGold.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
       // AM.NowBetGoldAmount += 15; // 중간 지점 업데이트에 있는게 아니면 텍스트같은 애들은 실시간 변수 변화가 적용이 안됨
       // AM.MyBetGoldAmount += 10;  // 그래서 한 경매가 끝나고 호출되는 bettingreset도 적용이 안됨

        AuctionTimer();
    }

    #region 경매 기능
    public void Auction()
    {
        if (AM.NowAuctionUnit != null)
        {
            Destroy(AM.NowAuctionUnit);
        }
        AM.NowAuctionUnit = RemoteActionUnitList();

        if (AM.NowAuctionUnit == null)
        {
            return;
        }
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

        if(FailedUnit == null)
        {
            return;
        }
        AM._FailedPos[AM.FailedCount].transform.localScale += new Vector3(2f, 2f, 1f);
        AM._failedUnitsList[AM.FailedCount] = Instantiate(FailedUnit, AM._FailedPos[AM.FailedCount].transform);
        FailedUnit.transform.localPosition = Vector3.zero;

        AM.FailedCount++;
        
    }


    public GameObject RemoteActionUnitList()
    {
        if (AM._instanceUnitsList.Count != 0)
        {
            GameObject ZeroListIndex = AM._instanceUnitsList[0];// 아직 못고침 반환을 복사의복사를 해대서 ㅈ됨
            int InstUnCnt = AM._instanceUnitsList.Count;
            AM.ActOrListPnt++;


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
        return null;
    }

    void AuctionTimer()
    {
        
        AM.TimeText.text = Mathf.Ceil(AM.Time).ToString();
        if (AM.Time > 0)
        {
            AM.Time -= Time.deltaTime;
        }
        else if (AM.NowBetGoldAmount == 0)
        {
            AuctionFailed();
            BettingReset();
        }
        else if(AM.NowBetGoldAmount > 0)
        {
            Auction();
            BettingReset();
        }
        
    }

    void BettingReset()
    {
        Debug.Log("asdasd");
        AM.Time = 15f;
        AM.NowBetGoldAmount = 0;
        AM.MyBetGoldAmount = 0;
        AM.NowBetGold.text = AM.NowBetGoldAmount.ToString();
        AM.MyBetGold.text = AM.MyBetGoldAmount.ToString();
    }

    public void Betting(int MyBetGold)
    {

    }

    public void AddBettingMoney(Button button)
    {
        
        string BetGold = button.transform.GetChild(0).name;
        int Gold = int.Parse(BetGold);
        Debug.Log(Gold);
        AM.MyBetGoldAmount += Gold;
        Debug.Log(AM.MyBetGoldAmount);
        AM.MyBetGold.text = AM.MyBetGoldAmount.ToString();
    }

    public void AddMyBetMoney(int BetMoney)
    {
        
        
        Debug.Log("wwee");
    }

    public void BettingButton()
    {
        AM.Time = 10f;
        
    }

    #endregion
}
