using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UnitPool : MonoBehaviour
{
    public static UnitPool instance;

    public GameObject Unit;

    public Queue<GameObject> Q_Unit = new Queue<GameObject>();

    private void Start()
    {
        instance = this;
        // 여기에서 auctionorderList랑 instanceList에 쓸 32개의 클론을 구현해야함 16 X 2
        GameObject U_Object = Instantiate();
        Q_Unit.Enqueue(U_Object);
        U_Object.SetActive(false);
    }

    
    public void InsertQueue(GameObject Unit)
    {
        Q_Unit.Enqueue(Unit);
        Unit.SetActive(false);
    }

    // 구현된 클론을 구분해서 꺼낼 부분이 필요함
    public GameObject GetQueue()
    {
        GameObject Unit = Q_Unit.Dequeue();
        Unit.SetActive(true);
        return Unit;
    }
}
