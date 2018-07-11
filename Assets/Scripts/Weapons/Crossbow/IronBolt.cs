using UnityEngine;

public class IronBolt : MonoBehaviour {

    public Rigidbody rb;
    private float lifespan = 3f;

    private int damage = 20;

    public void OnEnable()
    {
        // Reset the values to avoid holdover
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Entered enemy " + Time.time);
            EnemyController enemy = (EnemyController)other.gameObject.GetComponent(typeof(EnemyController));
            enemy.TakeHit(damage);
            Kill();
        }
    }

    public void OnFire(Transform barrel, float muzzleVelocity, Vector3 playerVelocity)
    {
        // TODO: we probably don't need to do this again
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Ignoring player velocity for now
        rb.velocity = rb.transform.forward * muzzleVelocity;
        Invoke("Kill", lifespan);
    }

    private void Kill()
    {
        gameObject.SetActive(false);
    }
}
