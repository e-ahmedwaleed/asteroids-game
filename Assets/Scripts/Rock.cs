using UnityEngine;

public class Rock : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        // apply impulse force to get game object moving
        const float minImpulseForce = 1f;
        const float maxImpulseForce = 2f;
        var angle = Random.Range(0, 2 * Mathf.PI);
        var direction = new Vector2(
            Mathf.Cos(angle), Mathf.Sin(angle));
        var magnitude = Random.Range(minImpulseForce, maxImpulseForce);
        GetComponent<Rigidbody2D>().AddForce(
            direction * magnitude,
            ForceMode2D.Impulse);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}