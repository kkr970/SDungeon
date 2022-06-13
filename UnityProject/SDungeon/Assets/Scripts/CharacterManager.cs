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
    // Hp, Mp bar
    public Transform hpBar;
    public Transform mpBar;

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
            Debug.Log(this.getName() + " : Attack " + target.getName() + " " + damage + "Damage!");
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
        }
    }
    //공격받음
    public virtual void onDamage(int damage)
    {
        curHp -= damage;
        if(curHp < 0) curHp = 0;
        updateHpBar();
    }

    //mp사용
    public virtual void useMp(int mp)
    {
        curMp -= mp;
        if(curMp < 0) curMp = 0;
        updateMpBar();
    }

    //mp, hp바 업데이트
    public virtual void updateHpBar()
    {
        hpBar.localScale = new Vector3((float)curHp/maxHp, 1.0f, 1.0f);
    }
    public virtual void updateMpBar()
    {
        mpBar.localScale = new Vector3((float)curMp/maxMp, 1.0f, 1.0f);
    }

    //사망처리
    public virtual void Dead()
    {
        State = STATE.DEAD;
        this.gameObject.SetActive(false);
    }
    



    // 오브젝트 이름 가져오기
    public string getName()
    {
        return gameObject.name;
    }
    public string getTag()
    {
        return gameObject.tag;
    }
}
