using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기사
public class Knight : CharacterManager
{
    private void Awake()
    {
        power = 5;
        magic = 2;
        hide = 2;
        speed = 5;
        lucky = 7;
        wisdom = 0;
    }
    private void Start()
    {
        maxHp = power + 3;
        curHp = maxHp;

        maxMp = magic+ 2;
        curMp = maxMp;
    }

    //Turn 관련
    public override void setTurn()
    {
        base.setTurn();
        turn = turnSpeed + speed;
    }
    public override float getTurn()
    {
        return turn;
    }

    // 공격
    public override void Attack(CharacterManager target)
    {
        base.Attack(target);
    }

    // 사망처리
    public override void Dead()
    {
        base.Dead();
    }
}