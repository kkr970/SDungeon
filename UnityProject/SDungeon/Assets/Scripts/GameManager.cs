using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    private static GameManager gm_instance;
    //외부 접근용 프로퍼티
    public static GameManager instance
    {
        get{
            if(gm_instance == null)
            {
                gm_instance = FindObjectOfType<GameManager>();
            }
            return gm_instance;
        }
    }
    
    public ICharacter[] charactors;
    public List<ICharacter> lcharactors;
    
    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            msetTurn();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mgetTurn();
        }
    }



    void msetTurn()
    {
        Debug.Log("Input : A, SetTurn!");
        charactors = FindObjectsOfType<CharacterManager>();
        lcharactors = new List<ICharacter>(charactors);
        for(int i = 0 ; i < lcharactors.Count ; i++)
        {
            lcharactors[i].setTurn();
        }
        lcharactors.Sort((a, b) => a.getTurn().CompareTo(b.getTurn())*(-1) );
    }
    void mgetTurn()
    {
        Debug.Log("Input : space, GetTurn!");
        for(int i = 0 ; i < lcharactors.Count ; i++)
        {
            Debug.Log(lcharactors[i].getName() + " " + lcharactors[i].getTurn());
        }
    }
}
