using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아크메이지
public class ArchMage : CharacterManager
{
    private void Awake()
    {
        power = 5;
        magic = 2;
        hide = 3;
        speed = 4;
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
