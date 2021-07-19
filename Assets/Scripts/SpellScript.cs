using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D mRigidBody;
    [SerializeField] private float speed;
    private int damage;
    public Transform MTarget { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (MTarget !=null)
        {
            // Hardcoded debug purposes
            Vector2 direction = MTarget.position - transform.position;

            mRigidBody.velocity = direction.normalized * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    /// <summary>
    /// Set how much damage a target takes
    /// </summary>
    /// <param name="target">Selected target</param>
    /// <param name="damage">Damage value</param>
    public void Initialize(Transform target, int damage)
    {
        this.MTarget = target;
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HitBox" && collision.transform == MTarget)
        {
            speed = 0;
            collision.GetComponentInParent<Enemy>().TakeDamage(damage);
            GetComponent<Animator>().SetTrigger("Impact");
            mRigidBody.velocity = Vector2.zero;
            MTarget = null;
        }
    }
}
