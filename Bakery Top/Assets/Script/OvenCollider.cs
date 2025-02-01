using UnityEngine;

public class OvenCollider : MonoBehaviour
{
    public enum ColliderType { Firewood, Dough }
    public ColliderType colliderType;
    private Oven oven;

    private void Start()
    {
        oven = GetComponentInParent<Oven>(); // Holt den Ofen-Parent
    }

    private void OnCollisionEnter(Collision other)
    {
        if (oven == null) return;

        Item item = other.collider.GetComponent<Item>();
        if (item == null) return;

        string itemName = item.GetItemName();

        if (colliderType == ColliderType.Firewood && itemName == "Holz")
        {
            oven.AddFirewood(other.gameObject);
        }
        else if (colliderType == ColliderType.Dough && itemName == "Dough")
        {
            oven.AddDough(other.gameObject);
        }
    }

}
