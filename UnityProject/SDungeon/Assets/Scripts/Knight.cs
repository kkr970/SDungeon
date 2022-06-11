using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : CharacterManager
{
    private int speed = 5;
    private float turn = 0f;
    //STATE state = STATE.WAIT;

    //Turn 관련
    public override void setTurn()
    {
        base.setTurn();
        turn = turnSpeed + speed;
    }
    public override float getTurn()
    {
        return turn;
    }
}
