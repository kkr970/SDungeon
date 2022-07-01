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

        maxMp = magic + 2;
        curMp = maxMp;
    }


    // 스킬
    // 1. 얼음가시/mp 1소모 : magic계수, magic만큼 반복, 1번당 (0)123/(1)456의 데미지
    public override bool skill(CharacterManager target, int num)
    {
        string skillName = "";
        // 얼음가시
        if(num == 1)
        {
            if(curMp < 1) return false;
            useMp(1);
            skillName = "Ice Needle";
            UIManager.instance.updateLogText(this.getObjectName() + " use " + skillName + "!");
            int totalDamage = 0;
            for(int i = 0 ; i < magic ; i++)
            {
                int damage = 0;
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
                // 데미지 적용
                if(damage > 0)
                {
                    totalDamage += damage;
                    if(target.State == STATE.DEAD) continue;
                    target.onDamage(damage);
                    //UIManager.instance.updateLogText(skillName + " : Hit!");
                }
            }
            UIManager.instance.updateLogText(totalDamage +" Hit!");
        }
        return true;
    }
    public override string skill_1_Info()
    {
        return "얼음가시" + System.Environment.NewLine
             + "사용 MP : 1" + System.Environment.NewLine
             + "마법 D6( / 123 / 456 )의 다단히트 데미지";
    }


    // 데미지 받음
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
    // 사망
    public override void dead()
    {
        base.dead();
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
