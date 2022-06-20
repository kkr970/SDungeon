using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기사
// 특성 : 다른 플레이어보다 힘비례 체력이 높음
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

    // 이름가져오기
    public override string getName()
    {
        return "Knight";
    }
    // 정보가져오기
    public override string getInfo()
    {
        string str = base.getInfo();
        str = str + "체력과 기본공격력이 높음";
        return str;
    }
}
