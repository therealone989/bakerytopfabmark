using UnityEngine;

public class ClowdMovement : MonoBehaviour
{
    public float speed = 5f;  // Geschwindigkeit der Wolke

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);  // Wolke bewegt sich in seine eigene Vorwärtsrichtung
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
