using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionManager : MonoBehaviour
{
    public List<GameObject> _auctionUnitList = new List<GameObject>();
    public Queue<GameObject> _auctionFailedQueue = new Queue<GameObject>();
    public Queue<GameObject> _auctionOrderQueue = new Queue<GameObject>();
    public GameObject AuctionUnit;
    public GameObject NowAutionUnit;

    public int NowBetAmount = 0;
    public int MyBetAmount = 0;





    // Start is called before the first frame update
    void Start()
    {
        
        SetUnitList();
       // AuctionFailed(ref _auctionUnitList);
        ShuffleList(ref _auctionUnitList);

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _auctionUnitList.Count; i++)
        {
            
        }

    }

    public void SetUnitList()
    {
        
        _auctionUnitList.Clear();
        int _auctionUnitListCount = 12;

        

        for (int i= 0 ; i< _auctionUnitListCount; i++)
        {
            _auctionUnitList.Add(AuctionUnit.transform.GetChild(i).gameObject);

            if (AuctionUnit.transform.GetChild(i) == null) return;
            
        }

        
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
        _auctionOrderQueue = new Queue<GameObject>(list);
    }

    public void AuctionFailed(ref Queue<GameObject> queue)
    {

       // _auctionFailedQueue.Enqueue(queue.);


       queue.Dequeue();
      
    }
}


