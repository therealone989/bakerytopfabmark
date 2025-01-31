using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 5f;  // Geschwindigkeit des Autos
    public float pushForce = 10f;  // Wie stark das Auto den Spieler wegschiebt
    public float upwardPush = 5f;  // Wie stark das Auto den Spieler nach oben schiebt

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);  // Auto bewegt sich in seine eigene Vorwärtsrichtung
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Start Collision");
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Richtung berechnen
                Vector3 pushDirection = collision.transform.position - transform.position;
                pushDirection.y = 0;  // Nur horizontal schieben

                // Spieler nach hinten und oben schleudern
                playerRb.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
                playerRb.AddForce(Vector3.up * upwardPush, ForceMode.Impulse);
            }
        }
        if (collision.gameObject.CompareTag("Ende"))
        {
            Debug.Log("End reached");
            Destroy(gameObject);
        }
    }
}
