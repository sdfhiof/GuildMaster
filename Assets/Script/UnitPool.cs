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
        // ���⿡�� auctionorderList�� instanceList�� �� 32���� Ŭ���� �����ؾ��� 16 X 2
        GameObject U_Object = Instantiate();
        Q_Unit.Enqueue(U_Object);
        U_Object.SetActive(false);
    }

    
    public void InsertQueue(GameObject Unit)
    {
        Q_Unit.Enqueue(Unit);
        Unit.SetActive(false);
    }

    // ������ Ŭ���� �����ؼ� ���� �κ��� �ʿ���
    public GameObject GetQueue()
    {
        GameObject Unit = Q_Unit.Dequeue();
        Unit.SetActive(true);
        return Unit;
    }
}
