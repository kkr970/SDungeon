using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 좀버
// 체력은 낮으나, 한번 죽을 때 마다 mp가 감소, 또한 mp가 0일 경우 사망처리
public class Zombur : CharacterManager
{
    private void Awake()
    {
        power = 3;
        magic = 0;
        hide = 0;
        speed = 2;
        lucky = 3;
        wisdom = 1;
    }
    private void Start()
    {
        maxHp = 2;
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
        //좀버의 특성 발동
        if(curMp > 0)
        {
            useMp(1);
            curHp = maxHp;
            updateHpBar();
            return;
        }
        base.Dead();
    }
    // 이름가져오기
    public override string getName()
    {
        return "Zombur";
    }
    // 정보가져오기
    public override string getInfo()
    {
        string str = base.getInfo();
        str = str + "좀버는 승리한다..!";
        return str;
    }
}
