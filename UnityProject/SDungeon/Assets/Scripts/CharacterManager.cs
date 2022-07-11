using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE
{
    // 기본 상태
    WAIT, //턴을 받는 것을 대기
    ACTION, //턴을 받아 행동을 취함
    END, // 턴이 끝남
    DEAD, // 죽음
}

public enum EFFECT
{
    //기본, 상태이상 없음
    NONE,

    // 특수 상태이상
    STUN
}

public class CharacterManager : MonoBehaviour, ICharacter
{
    private STATE state = STATE.END;
    private EFFECT effect = EFFECT.NONE;
    public float turnSpeed { get; private set; }

    // 스텟, 능력치
    public float turn = 0f;
    public int maxHp;
    public int curHp;
    public int maxMp;
    public int curMp;
    public int power;
    public int magic;
    public int hide;
    public int speed;
    public int lucky;
    public int wisdom;
    // Hp, Mp bar, NowTurn
    public Transform hpBar;
    public Transform mpBar;
    public GameObject nowTurn;

    // -------------메서드----------------
    // state 관련
    public STATE State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
        }
    }
    // effect 관련
    public EFFECT Effect
    {
        get
        {
            return effect;
        }
        set
        {
            effect = value;
        }
    }
    // 턴 관련
    public virtual void setTurn()
    {
        turnSpeed = Random.Range(0f, 3f);
    }
    public virtual float getTurn()
    {
        return turnSpeed + speed;
    }

    //공격함
    public virtual void attack(CharacterManager target)
    {
        Debug.Log("Attack!" + this.getName() + " -> " + target.getName());
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
        }
        else
        {
            //Debug.Log("Miss!");
            UIManager.instance.updateLogText(this.getObjectName() + " Attack... Miss!");
        }
    }
    //스킬 사용
    public virtual bool skill(CharacterManager target, int num)
    {
        //Debug.Log(this.getObjectName() + " : SKILL!");
        return true;
    }
    public virtual string skill_1_Info()
    {
        return "";
    }


    //스킵, mp회복
    public virtual void skip()
    {
        recoverMP(1 + (maxMp)/5);
        UIManager.instance.updateLogText(this.getObjectName() + 
                            " Skip");
    }


    //공격받음
    public virtual void onDamage(int damage)
    {
        UIManager.instance.updateLogText(this.getObjectName() + " take " + damage + "Damage!");
        curHp -= damage;
        StopCoroutine(hitMotion());
        StartCoroutine(hitMotion());
        if(curHp <= 0)
        {
            curHp = 0;
            this.dead();
        }
        updateHpBar();
    }
    //공격받는 모션
    private IEnumerator hitMotion()
    {
        Color curColor = this.GetComponent<SpriteRenderer>().color;
        Color newColor = curColor;
        newColor.g = 0.5f;
        newColor.b = 0.5f;
        this.GetComponent<SpriteRenderer>().color = newColor;
        yield return new WaitForSeconds(0.29f);
        this.GetComponent<SpriteRenderer>().color = curColor;
    }
    //mp사용
    public virtual void useMp(int mp)
    {
        curMp -= mp;
        if(curMp < 0) curMp = 0;
        UIManager.instance.updateLogText(this.getObjectName() + 
                            " Use " + mp +"MP");
        updateMpBar();
    }
    //mp회복
    public virtual void recoverMP(int mp)
    {
        if(curMp + mp > maxMp)
        {
            mp = maxMp - curMp;
        }
        curMp += mp;
        UIManager.instance.updateLogText(this.getObjectName() + 
                            " Recovered " + mp +"MP");
        updateMpBar();
    }
    //hp회복
    public virtual void recoverHP(int hp)
    {
        if(curHp + hp > maxHp)
        {
            hp = maxHp - curHp;
        }
        curHp += hp;
        UIManager.instance.updateLogText(this.getObjectName() + 
                            " Recovered " + hp +"HP" );
        updateHpBar();
    }
    //상태이상 처리
    public virtual void checkEffect()
    {
        //스턴 상태 - 턴 스킵
        if(this.Effect == EFFECT.STUN)
        {
            this.State = STATE.END;
            this.Effect = EFFECT.NONE;
            UIManager.instance.updateLogText(this.getObjectName() + " is Stun. (SKIP TURN)" );
            return;
        }
    }


    //mp바, hp바, 턴알림 업데이트
    public void updateHpBar()
    {
        hpBar.localScale = new Vector3((float)curHp/maxHp, 1.0f, 1.0f);
    }
    public void updateMpBar()
    {
        mpBar.localScale = new Vector3((float)curMp/maxMp, 1.0f, 1.0f);
    }
    public void onNowTurn()
    {
        nowTurn.SetActive(true);
        Color newColor = nowTurn.GetComponent<SpriteRenderer>().color;
        newColor.a = 1.0f;
        nowTurn.GetComponent<SpriteRenderer>().color = newColor;
    }
    public void onNextTurn()
    {
        nowTurn.SetActive(true);
        Color newColor = nowTurn.GetComponent<SpriteRenderer>().color;
        newColor.a = 0.5f;
        nowTurn.GetComponent<SpriteRenderer>().color = newColor;
    }
    public void offNowTurn()
    {
        nowTurn.SetActive(false);
    }

    //사망처리
    public virtual void dead()
    {
        State = STATE.DEAD;
        UIManager.instance.updateLogText(this.getObjectName() + " Dead!" );
        this.gameObject.SetActive(false);
    }
    

    // 오브젝트 이름 반환
    public string getObjectName()
    {
        return gameObject.name;
    }
    // 이름 반환
    public virtual string getName()
    {
        return "CharacterManager";
    }
    // 오브젝트 태그 반환
    public string getTag()
    {
        return gameObject.tag;
    }
    // 오브젝트 정보 반환
    public virtual string getInfo()
    {
        return "Select Info" + System.Environment.NewLine
             + "이름 : " + getName() + "(" + Effect + ")" + System.Environment.NewLine
             + "HP : " + this.curHp + "/" + this.maxHp + System.Environment.NewLine
             + "MP : " + this.curMp + "/" + this.maxMp + System.Environment.NewLine;
    }
    public virtual string getStatInfo()
    {
        return "Select Status Info" + System.Environment.NewLine
             + "Power : " + this.power + System.Environment.NewLine
             + "Magic : " + this.magic + System.Environment.NewLine
             + "Speed : " + this.speed + System.Environment.NewLine
             + "Hide  : " + this.hide;
    }

    // Enemy전용 AI
    public virtual string enemyActionAI()
    {
        Debug.Log(this.getObjectName() + " : 행동 생각중");
        return null;
    }

}
