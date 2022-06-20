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

    // 특수 상태이상

}

public class CharacterManager : MonoBehaviour, ICharacter
{
    private STATE state = STATE.END;
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
    // 턴 관련
    public virtual void setTurn()
    {
        turnSpeed = Random.Range(0f, 3f);
    }
    public virtual float getTurn()
    {
        return turnSpeed;
    }

    //공격함
    public virtual void Attack(CharacterManager target)
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
            Debug.Log(this.getObjectName() + " : Attack " + target.getObjectName() + " " + damage + "Damage!");
            UIManager.instance.updateLogText(this.getObjectName() + " Attack -> " + target.getObjectName()
                                + " : " + damage + "Damage!" + System.Environment.NewLine);
            target.onDamage(damage);
            // 타겟의 현재 hp를 확인
            if(target.curHp <= 0)
            {
                target.Dead();
            }
        }
        else
        {
            Debug.Log("Miss!");
            UIManager.instance.updateLogText(this.getObjectName() + " Attack -> " + target.getObjectName()
                                + " : " + "Miss!" + System.Environment.NewLine);

        }
    }
    //공격받음
    public virtual void onDamage(int damage)
    {
        curHp -= damage;
        if(curHp < 0) curHp = 0;
        StartCoroutine(hitMotion());
        updateHpBar();
    }
    //공격받는 모션
    private IEnumerator hitMotion()
    {
        Color oriColor = this.GetComponent<SpriteRenderer>().color;
        Color newColor = oriColor;
        newColor.g = 0.5f;
        newColor.b = 0.5f;
        this.GetComponent<SpriteRenderer>().color = newColor;

        yield return new WaitForSeconds(0.29f);

        this.GetComponent<SpriteRenderer>().color = oriColor;
    }

    //mp사용
    public virtual void useMp(int mp)
    {
        curMp -= mp;
        if(curMp < 0) curMp = 0;
        updateMpBar();
    }
    //스킵, mp회복
    public virtual void skip()
    {
        this.curMp += 1 + (maxMp/5);
        if(this.curMp > this.maxMp)
        {
            this.curMp = this.maxMp;
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
    public virtual void Dead()
    {
        State = STATE.DEAD;
        UIManager.instance.updateLogText(this.getObjectName() + " Dead!" + System.Environment.NewLine);
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
        string str = "Select Info" + System.Environment.NewLine
                    +"이름 : " + getName() + System.Environment.NewLine
                    +"HP : " + this.curHp + System.Environment.NewLine
                    +"MP : " + this.curMp + System.Environment.NewLine;
        return str;
    }
}
