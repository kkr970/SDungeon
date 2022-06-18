using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public void ButtonClick(ref string name)
    {
        name = gameObject.name;
    }
}
