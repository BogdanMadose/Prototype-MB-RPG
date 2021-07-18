using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D mRigidBody;
    [SerializeField] private float speed;
    public Transform MTarget { get; set; }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HitBox" && collision.transform == MTarget)
        {
            GetComponent<Animator>().SetTrigger("Impact");
            mRigidBody.velocity = Vector2.zero;
            MTarget = null;
        }
    }
}
