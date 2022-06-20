using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아크메이지
// 특성 : 공격받을 때, 50%(내림)만큼 mp가 대신 줄어듬
public class ArchMage : CharacterManager
{
    private void Awake()
    {
        power = 3;
        magic = 5;
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
    public override void onDamage(int damage)
    {
        int mpDamage = damage/2;
        if(mpDamage > curMp)
        {
            mpDamage = curMp;
        }
        damage -= mpDamage;

        base.onDamage(damage);
        useMp(mpDamage);
    }

    public override void Dead()
    {
        base.Dead();
    }
    // 이름가져오기
    public override string getName()
    {
        return "ArchMage";
    }
    // 정보가져오기
    public override string getInfo()
    {
        string str = base.getInfo();
        str = str + "공격받을 때, 50%(내림)만큼 mp소모";
        return str;
    }
}
