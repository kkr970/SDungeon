using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : CharacterManager
{
    public int speed = 4;
    public float turn = 0f;

    public override void setTurn()
    {
        base.setTurn();
        turn = turnSpeed + speed;
    }
    public override float getTurn()
    {
        return turn;
    }

    public override string getName()
    {
        return base.getName();
    }
}
