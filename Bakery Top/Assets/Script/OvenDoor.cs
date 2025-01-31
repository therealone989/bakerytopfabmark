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
        return "Drücke [E], zum öffnen";
    }

    public void Interact()
    {
        if(oven != null)
        {
            if(gameObject.name == "ObereTür")
            {
                oven.ToggleOvenDoor();
            }
            if(gameObject.name == "UntereTür")
            {
                oven.ToggleFirewoodDoor();
            }
        }
    }


}
