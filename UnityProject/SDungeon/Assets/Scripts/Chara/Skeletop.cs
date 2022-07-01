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

    public override bool skill(CharacterManager target, int num)
    {
        string skillName = "";
        // 행운의 공격
        if(num == 1)
        {
            if(curMp < 1) return false;
            useMp(1);

            skillName = "Lucky Strike";
            bool success = false;
            for(int i = 0 ; i < lucky ; i++)
            {
                int _num = Random.Range(0, 6);
                switch(_num)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        success = true;
                        break;
                    default:
                        Debug.LogWarning("Error, Skill()");
                        break;
                }
            }
            
            // 데미지 적용
            if(success)
            {
                UIManager.instance.updateLogText(this.getObjectName() + " use " + skillName + "!");
                target.onDamage(1);
                target.Effect = EFFECT.STUN;
                UIManager.instance.updateLogText(target.getObjectName() + " STUN!");
            }
            else
            {
                UIManager.instance.updateLogText(this.getObjectName() + " use " + skillName + "... Miss!");
            }
        }
        return true;
    }
    
    //행동 선택AI
    public override string enemyActionAI()
    {
        string action = "Attack";
        if(curMp >= 1)
        {
            //50%의 확률로 스킬1 발동
            int num = Random.Range(0, 3);
            switch(num)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    action = "Skill1";
                    break;
            }
        }

        return action;
    }



    // 이름가져오기
    public override string getName()
    {
        return "Skeletop";
    }
    // 정보가져오기
    public override string getInfo()
    {
        string str = base.getInfo();
        str = str + "공격이 꽤 아프다!";
        return str;
    }
}
