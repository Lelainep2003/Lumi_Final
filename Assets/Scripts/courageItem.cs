using UnityEngine;

public class CourageItem : MonoBehaviour
{
    public float fallSpeed = 4f;

    void Update()
    {
        // Move the item downward
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Destroy if it falls below the screen
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("RestoreCourage", 1);
            Destroy(gameObject);
        }
    }
}