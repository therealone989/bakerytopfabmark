using UnityEngine;

public class OvenDoor : MonoBehaviour, IInteractable
{

    private Oven oven;
    void Start()
    {
        oven = GetComponentInParent<Oven>();
    }

    public string GetInteractText()
    {
        return "Dr�cke [E], zum �ffnen";
    }

    public void Interact()
    {
        if(oven != null)
        {
            if(gameObject.name == "ObereT�r")
            {
                oven.ToggleOvenDoor();
            }
            if(gameObject.name == "UntereT�r")
            {
                oven.ToggleFirewoodDoor();
            }
        }
    }


}
