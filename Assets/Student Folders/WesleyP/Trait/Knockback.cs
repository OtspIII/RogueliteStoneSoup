using UnityEngine;

public class Knockback :Triat
{
    [SerializeField] private float knockbackForce = 10f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get Rigidbody2D of the object to knock back
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calculate knockback direction
                Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
                // Apply force
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
