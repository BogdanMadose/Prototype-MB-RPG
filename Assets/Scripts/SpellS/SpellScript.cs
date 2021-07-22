using UnityEngine;

public class SpellScript : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D _rb;
    private int _damage;
    private Transform _damageSource;

    public Transform Target { get; private set; }

    // Start is called before the first frame update
    void Start() => _rb = GetComponent<Rigidbody2D>();

    private void FixedUpdate()
    {
        if (Target != null)
        {
            // Hardcoded debug purposes
            Vector2 direction = Target.position - transform.position;

            _rb.velocity = direction.normalized * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    /// <summary>
    /// Set how much damage a target takes
    /// </summary>
    /// <param name="target">Selected target</param>
    /// <param name="damage">Damage value</param>
    /// <param name="damageSource">Object that deals damage</param>
    public void Initialize(Transform target, int damage, Transform damageSource)
    {
        this.Target = target;
        this._damage = damage;
        this._damageSource = damageSource;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HitBox" && collision.transform == Target)
        {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            c.TakeDamage(_damage, _damageSource);
            GetComponent<Animator>().SetTrigger("Impact");
            _rb.velocity = Vector2.zero;
            Target = null;
        }
    }
}
