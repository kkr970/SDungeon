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

    public float turn = 0f;
    public int maxHp;
    public int curHp;
    public int maxMp;
    public int curMp;
    // 각 2줄씩 합해서 7
    public int power;
    public int magic;
    //
    public int hide;
    public int speed;
    //
    public int lucky;
    public int wisdom;

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
            target.curHp -= damage;
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
    /*
    //공격받음
    public virtual void Damage(int damage)
    {
        Debug.Log("Attacked ( "+damage+" )Damage");
    }
    */

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
