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
        maxHp = power + 2;
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

    public override void Attack(CharacterManager target)
    {
        base.Attack(target);
    }

    public override void Dead()
    {
        base.Dead();
    }
}
