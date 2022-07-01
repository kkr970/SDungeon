using UnityEngine;

// 기사
// 특성 : 공격력이 높음
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

        maxMp = magic;
        curMp = maxMp;
    }

    // 스킬
    // 1. 강타/mp 1소모 : power계수 (-1)0/(0)12/(+1)345의 데미지
    public override bool skill(CharacterManager target, int num)
    {
        string skillName = "";
        // 강타
        if(num == 1)
        {
            if(curMp < 1) return false;
            useMp(1);

            skillName = "Strike";
            int damage = 0;
            for(int i = 0 ; i < power + 1 ; i++)
            {
                int _num = Random.Range(0, 6);
                switch(_num)
                {
                    case 0:
                        damage--;
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
            }
            
            // 데미지 적용
            if(damage > 0)
            {
                UIManager.instance.updateLogText(this.getObjectName() + " use " + skillName + "!");
                target.onDamage(damage);
            }
            else
            {
                UIManager.instance.updateLogText(this.getObjectName() + " use " + skillName + "... Miss!");
            }
        }
        return true;
    }
    public override string skill_1_Info()
    {
        return "강타" + System.Environment.NewLine
             + "사용 MP : 1" + System.Environment.NewLine
             + "힘+1 D6( 1 / 23 / 456 )의 데미지";
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
        str = str + "기본공격력이 높음";
        return str;
    }
}
