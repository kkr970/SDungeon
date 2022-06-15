using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스켈레톱
// 평범한 몬스터이며, 공격력이 꽤나 높음
public class Skeletop : CharacterManager
{
    private void Awake()
    {
        power = 5;
        magic = 1;
        hide = 2;
        speed = 3;
        lucky = 2;
        wisdom = 0;
    }
    private void Start()
    {
        maxHp = 3;
        curHp = maxHp;

        maxMp = 2;
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
    // 이름가져오기
    public override string getName()
    {
        return "Skeletop";
    }
}
