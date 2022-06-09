using UnityEngine;

public class CharacterManager : MonoBehaviour, ICharacter
{
    public float turnSpeed { get; private set; }

    public virtual void onDamage(float damage)
    {

    }

    public virtual void setTurn()
    {
        turnSpeed = Random.Range(0f, 3f);
    }
    public virtual float getTurn()
    {
        return turnSpeed;
    }

    public virtual string getName()
    {
        return gameObject.name;
    }
}
