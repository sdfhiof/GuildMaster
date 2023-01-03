using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoWar_Manager : MonoBehaviour
{
    static AutoWar_Manager Instance;
    public static AutoWar_Manager GetInstance() {Init(); return Instance;}

    public float _findTimer;
    public List<Transform> _unitPool = new List<Transform>();
    public List<WarUnit> _guildUnitList = new List<WarUnit>();
    public List<WarUnit> _enemyUnitList = new List<WarUnit>();

    void Start()
    {
        Init();
        SetUnitList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static void Init()
    {
        GameObject AM = GameObject.Find("WarManager");
        Instance = AM.GetComponent<AutoWar_Manager>();  
    }

    void SetUnitList()
    {
        _guildUnitList.Clear();
        _enemyUnitList.Clear();

        for (var i = 0; i < _unitPool[i].childCount; i++)
        {
            for(var j = 0; j < _unitPool[i].childCount; j++)
            {
                switch(i)
                {
                    case 0:
                    _guildUnitList.Add(_unitPool[i].GetChild(j).GetComponent<WarUnit>());
                    _unitPool[i].GetChild(j).gameObject.tag = "Guild";
                    break;

                    case 1:
                    _enemyUnitList.Add(_unitPool[i].GetChild(j).GetComponent<WarUnit>());
                    _unitPool[i].GetChild(j).gameObject.tag = "Enemy";
                    break;
                }
            }
        }
    }

    public WarUnit GetTarget(WarUnit unit)
    {
        WarUnit tUnit = null;
        List<WarUnit> tList = new List<WarUnit>();

        

        switch(unit.tag)
        {
            case "Guild": tList = _enemyUnitList; break;
            case "Enemy": tList = _guildUnitList; break;
        }
        float tSDis = 999999f;

        for(var i = 0; i < tList.Count; i++)
        {            
            float tDis = ((Vector2)tList[i].transform.localPosition - (Vector2)unit.transform.localPosition).sqrMagnitude;
            if(tDis <= unit._unitAR * unit._unitAR)
            {
                if(tList[i]._unitState != WarUnit.UnitState.death)
                {
                    if(tDis < tSDis)
                    {
                        tUnit = tList[i];
                        tSDis = tDis;
                    }
                }
            }
        }
        return tUnit;
    }
}
