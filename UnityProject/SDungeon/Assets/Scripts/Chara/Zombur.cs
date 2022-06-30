using UnityEngine;

// 좀버
// 체력은 낮으나, 한번 죽을 때 마다 mp가 감소, 또한 mp가 0일 경우 사망처리
// 공격 시, 2이상의 피해를 주면 hp1 회복
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

    //공격
    public override void attack(CharacterManager target)
    {
        //Debug.Log("Attack!" + this.getName() + " -> " + target.getName());
        int damage = 0;
        //데미지 계산 power만큼 반복, 01:-1,  2:0,  345:+1
        for(int i = 0 ; i < power ; i++)
        {
            int num = Random.Range(0, 6);
            switch(num)
            {
                case 0:
                    damage--;
                    break;
                case 1:
                    damage--;
                    break;
                case 2:
                    break;
                case 3:
                    damage++;
                    break;
                case 4:
                    damage++;
                    break;
                case 5:
                    damage++;
                    break;
                default:
                    Debug.LogWarning("Error, Attack()");
                    break;
            }
        }
        // 데미지 적용
        if(damage > 0)
        {
            //Debug.Log(this.getObjectName() + " : Attack " + target.getObjectName() + " " + damage + "Damage!");
            UIManager.instance.updateLogText(this.getObjectName() + " Attack!");
            target.onDamage(damage);
            //좀비의 특성 2이상의 데미지를 줄 경우, 1회복
            if(damage >= 2)
            {
                recoverHP(1);
            }
        }
        else
        {
            //Debug.Log("Miss!");
            UIManager.instance.updateLogText(this.getObjectName() + " Attack... Miss!");
        }
    }

    //사망
    public override void dead()
    {
        //좀버의 특성 발동
        if(curMp > 0)
        {
            useMp(1);
            curHp = maxHp;
            updateHpBar();
            return;
        }
        base.dead();
    }

    //행동 선택AI - 좀비는 기본공격밖에 없음
    public override string enemyActionAI()
    {
        return "Attack";
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
