using UnityEngine;

public class ClowdMovement : MonoBehaviour
{
    public float speed = 5f;  // Geschwindigkeit des Autos

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);  // Auto bewegt sich in seine eigene Vorwärtsrichtung
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ende"))
        {
            Debug.Log("End reached");
            Destroy(gameObject);
        }
    }
}
