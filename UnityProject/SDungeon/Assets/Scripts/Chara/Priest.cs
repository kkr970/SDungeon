using UnityEngine;

// 사제
// 특성 : 체력이 높음
//        적과 아군을 선택해 힐 또는 공격이 가능함
public class Priest : CharacterManager
{
    private void Awake()
    {
        power = 2;
        magic = 4;
        hide = 4;
        speed = 3;
        lucky = 2;
        wisdom = 5;
    }
    private void Start()
    {
        maxHp = power + 5;
        curHp = maxHp;

        maxMp = magic + 2;
        curMp = maxMp;
    }

    // 스킬
    // 1. 빛의 창/mp2소모 1) 적 : [magic (-1)12/(0)3/(+1)456] + [power (-1)12/(0)34/(1)56]의 데미지
    //                   2) 아군 : [magic (-1)12/(0)3/(+1)456]의 회복
    public override bool skill(CharacterManager target, int num)
    {
        string skillName = "";
        // 빛의 창
        if(num == 1)
        {
            if(curMp < 1) return false;
            useMp(1);

            skillName = "Light Spear";
            // 적 공격
            if(target.tag == "Enemy")
            {
                int totalDamage = 0;
                int magDamage = 0;
                for(int i = 0 ; i < magic ; i++)
                {
                    int _num = Random.Range(0, 6);
                    switch(_num)
                    {
                        case 0:
                            magDamage--;
                            break;
                        case 1:
                            magDamage--;
                            break;
                        case 2:
                            break;
                        case 3:
                            magDamage++;
                            break;
                        case 4:
                            magDamage++;
                            break;
                        case 5:
                            magDamage++;
                            break;
                        default:
                            Debug.LogWarning("Error, Skill()");
                            break;
                    }
                }
                if(magDamage > 0)
                {
                    totalDamage += magDamage;
                }
                int powDamage = 0;
                for(int i = 0 ; i < power ; i++)
                {
                    int _num = Random.Range(0, 6);
                    switch(_num)
                    {
                        case 0:
                            powDamage--;
                            break;
                        case 1:
                            powDamage--;
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                            powDamage++;
                            break;
                        case 5:
                            powDamage++;
                            break;
                        default:
                            Debug.LogWarning("Error, Skill()");
                            break;
                    }
                }
                if(powDamage > 0)
                {
                    totalDamage += powDamage;
                }
                // 데미지 적용
                if(totalDamage > 0)
                {
                    UIManager.instance.updateLogText(this.getObjectName() + " use " + skillName + "!");
                    target.onDamage(totalDamage);
                }
                else
                {
                    UIManager.instance.updateLogText(this.getObjectName() + " use " + skillName + "... Miss!");
                }
            }
            // 아군 힐
            else if(target.tag == "Player")
            {
                int heal = 0;
                for(int i = 0 ; i < magic ; i++)
                {
                    int _num = Random.Range(0, 6);
                    switch(_num)
                    {
                        case 0:
                            heal--;
                            break;
                        case 1:
                            heal--;
                            break;
                        case 2:
                            break;
                        case 3:
                            heal++;
                            break;
                        case 4:
                            heal++;
                            break;
                        case 5:
                            heal++;
                            break;
                        default:
                            Debug.LogWarning("Error, Skill()");
                            break;
                    }
                }
                // 회복 적용
                UIManager.instance.updateLogText(this.getObjectName() + " use " + skillName + "!");
                if(heal > 0)
                {
                    target.recoverHP(heal);
                }
                else
                {
                    target.recoverHP(0);
                }
            }
        }
        return true;
    }
    public override string skill_1_Info()
    {
        return "빛의 창" + System.Environment.NewLine
             + "사용 MP : 2" + System.Environment.NewLine
             + "적 : " + "마법 D6( 12 / 3 / 456 )" + System.Environment.NewLine
             + "      + 힘 D6( 12 / 34 / 56 ) 데미지" + System.Environment.NewLine
             + "아군 : " + "마법 D6( 12 / 3 / 456 )의 회복 ";
    }



    // 이름가져오기
    public override string getName()
    {
        return "Priest";
    }
    // 정보가져오기
    public override string getInfo()
    {
        string str = base.getInfo();
        str = str + "아군에게 힐이 가능한 스킬을 보유" + System.Environment.NewLine + "체력이 다소 높음";
        return str;
    }
}
