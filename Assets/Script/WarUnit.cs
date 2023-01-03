using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarUnit : MonoBehaviour
{
    public SPUM_Prefabs _spumPref;    

    AutoWar_Manager AM;

    public WarUnit _target;

    public enum UnitState
    {
        idle,

        run,

        attack,

        skill,

        stun,

        death
    } 
    public UnitState _unitState = UnitState.idle;

    public float _unitHP;

    public float _unitMS;

    public float _unitAT;

    public float _unitFR;

    public float _unitAS;

    public float _unitAR;

    public float _findTimer;

    void Start()
    {
        AM = AutoWar_Manager.GetInstance();        
    }

    void Update()
    {
        
        _findTimer += Time.deltaTime;
        if(_findTimer > AM._findTimer)
        {
            CheckState();
            _findTimer = 0f;
        }
        
    }

    void SetInitState()
    {
        _unitState = UnitState.idle;
    }

    void CheckState()
    {
        switch(_unitState)
        {
            case UnitState.idle:
            FindTarget();
            break;

            case UnitState.run:
            FindTarget();
            break;

            case UnitState.attack:
            FindTarget();
            break;

            case UnitState.skill:
            FindTarget();
            break;

            case UnitState.stun:
            FindTarget();
            break;

            case UnitState.death:
            FindTarget();
            break;
        }
    }

    void FindTarget()
    {
        _target = AM.GetTarget(this);
    }
}
