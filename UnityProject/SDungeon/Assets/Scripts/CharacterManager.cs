using UnityEngine;

public enum STATE
{
    WAIT, //턴을 받는 것을 대기
    ACTION, //턴을 받아 행동을 취함
    END, // 턴이 끝남
    DEAD // 죽음
}

public class CharacterManager : MonoBehaviour, ICharacter
{
    public STATE state = STATE.END;
    public float turnSpeed { get; private set; }


    // -------------메서드----------------
    // 턴 관련
    public virtual void setTurn()
    {
        turnSpeed = Random.Range(0f, 3f);
    }
    public virtual float getTurn()
    {
        return turnSpeed;
    }

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
    
    // 오브젝트 이름 가져오기
    public virtual string getName()
    {
        return gameObject.name;
    }
}
