using UnityEngine;

// 기어다니는 무언가
// 스킬1 : 스킬2를 다음턴에 발동
//         다음 턴은 높은 속도, 힘+3
//         스킬 사용중일 때, 받는 데미지 1 감소, 데미지를 받을 경우 힘이 원래대로 돌아옴
// 스킬2 : 강력한 물리 공격을 가함
public class Crawler : CharacterManager
{
    private bool isCrawling = false;
    private CharacterManager crawlTarget;

    private void Awake()
    {
        power = 3;
        magic = 1;
        hide = 1;
        speed = 2;
        lucky = 3;
        wisdom = 1;
    }
    private void Start()
    {
        maxHp = 5;
        curHp = maxHp;

        maxMp = 0;
        curMp = maxMp;
    }

    //Turn 관련
    public override float getTurn()
    {
        if(isCrawling) return 6.5f;
        
        return turn;
    }

    public override bool skill(CharacterManager target, int num)
    {
        string skillName = "";
        // 스킬1 크롤링(기습 사용)
        if(num == 1)
        {
            skillName = "Crawling";
            UIManager.instance.updateLogText(this.getObjectName() + " is " + skillName + "..." + " // Target : " + target.getObjectName());
            isCrawling = true;
            power += 3;
            crawlTarget = target;
        }
        // 스킬2 기습()
        else if(num == 2)
        {
            skillName = "Sucker Punch";
            int damage = 0;
            //데미지 계산 power만큼 반복, 012:0,  345:+1
            for(int i = 0 ; i < power ; i++)
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
            if(damage  > 0)
            {
                UIManager.instance.updateLogText(this.getObjectName() + " use " + skillName + "!");
                crawlTarget.onDamage(damage);
            }
            else
            {
                UIManager.instance.updateLogText(this.getObjectName() + " use " + skillName + "... Miss!");
            }
            // 크롤링 종료
            isCrawling = false;
            if(power != 3)
            {
                power = 3;
            }
        }
        return true;
    }

    //피격
    public override void onDamage(int damage)
    {
        if(isCrawling)
        {
            if(damage > 0)
            {
                damage -= 1;
                UIManager.instance.updateLogText(this.getObjectName() + " reduce 1damage!");
                if(power != 3)
                {
                    power = 3;
                }
            }
        }
        base.onDamage(damage);
    }


    //행동 선택AI
    public override string enemyActionAI()
    {
        string action = "Attack";

        // 크롤링 상태일 경우, 스킬2 발동
        // 크롤링 상태는 2/6 의 확률로 발동
        if(isCrawling) action = "Skill2";
        else
        {
            int _num = Random.Range(0, 6);
            bool useCrawling = false;
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
                    useCrawling = true;
                    break;
                case 5:
                    useCrawling = true;
                    break;
                default:
                    break;
            }
            if(useCrawling)
            {
                action = "Skill1";
            }
        }

        return action;
    }


    // 이름가져오기
    public override string getName()
    {
        return "Crawler";
    }
    // 정보가져오기
    public override string getInfo()
    {
        string str = base.getInfo();
        str = str + "무엇인가 기어다니고 있다.";
        return str;
    }
}
