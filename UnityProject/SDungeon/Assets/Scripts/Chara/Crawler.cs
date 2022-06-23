using UnityEngine;

// 기어다니는 무언가
// 스킬1 : 다음 공격을 강화함, 다음 공격은 높은 속도를 가짐, 스킬 사용중일 때, 받는 데미지 1 감소
//
public class Crawler : CharacterManager
{
    private bool isCrawling = false;

    private void Awake()
    {
        power = 4;
        magic = 1;
        hide = 1;
        speed = 1;
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
    public override void setTurn()
    {
        base.setTurn();
        turn = turnSpeed + speed;
    }
    public override float getTurn()
    {
        if(isCrawling) return 10.0f;
        
        return turn;
    }

    //행동 선택AI
    public override string enemyActionAI()
    {
        string action = "Attack";


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
