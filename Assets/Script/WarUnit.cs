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

    public Vector2 _tempDis;

    public Vector2 _dirVec;

    public float _findTimer;

    public float _attackTimer;

    void Start()
    {
        AM = AutoWar_Manager.GetInstance();
    }

    void Update()
    {
        CheckState();
    }

    void SetInitState()
    {
        _unitState = UnitState.idle;
    }

    void CheckState()
    {
        switch (_unitState)
        {
            case UnitState.idle:
                FindTarget();
                break;

            case UnitState.run:
                FindTarget();
                DoMove();
                break;

            case UnitState.attack:
                CheckAttack();
                break;

            case UnitState.skill:
                break;

            case UnitState.stun:
                break;

            case UnitState.death:
                break;
        }
    }

    void SetState(UnitState state)
    {
        _unitState = state;
        switch (_unitState)
        {
            case UnitState.idle:
                _spumPref.PlayAnimation("idle");
                break;

            case UnitState.run:
                _spumPref.PlayAnimation("run");
                break;

            case UnitState.attack:
                _spumPref.PlayAnimation("attack");
                break;

            case UnitState.skill:
                _spumPref.PlayAnimation("skill");
                break;

            case UnitState.stun:
                _spumPref.PlayAnimation("stun");
                break;

            case UnitState.death:
                _spumPref.PlayAnimation("death");
                break;
        }
    }

    void FindTarget()
    {
        _findTimer += Time.deltaTime;
        if (_findTimer > AM._findTimer)
        {
            _target = AM.GetTarget(this);
            if (_target != null) SetState(UnitState.run);
            else SetState(UnitState.idle);
            _findTimer = 0f;
        }
    }

    void DoMove()
    {
        if (!CheckTarget()) return;
        CheckDistance();
        _dirVec = _tempDis.normalized;
        SetDirection();
        transform.position += (Vector3)_dirVec * _unitMS * Time.deltaTime;
    }

    bool CheckDistance()
    {
        _tempDis = (Vector2)(_target.transform.localPosition - transform.position);
        float tDis = _tempDis.sqrMagnitude;

        if (tDis <= _unitAR * _unitAR)
        {
            SetState(UnitState.attack);

            return true;
        }
        else
        {
            if (!CheckTarget()) SetState(UnitState.idle);
            else SetState(UnitState.run);

            return false;
        }
    }

    void CheckAttack()
    {
        if (!CheckTarget()) return;
        if (!CheckDistance()) return;

        _attackTimer += Time.deltaTime;

        if (_attackTimer > _unitAS)
        {
            DoAttack();
            _attackTimer = 0;
        }
    }

    void DoAttack()
    {
        _spumPref.PlayAnimation("Attack");
    }

    void SetDirection()
    {
        if (_dirVec.x >= 0)
        {
            _spumPref._anim.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            _spumPref._anim.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    bool CheckTarget()
    {
        bool value = true;
        if (_target == null) value = false;
        if (_target._unitState == UnitState.death) value = false;
        if (!_target.gameObject.activeInHierarchy) value = false;

        if (!value)
        {
            SetState(UnitState.idle);
        }
        return value;
    }
}