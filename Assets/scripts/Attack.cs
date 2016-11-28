using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

    [SerializeField]
    private bool splashAttack = false;
    [SerializeField]
    private float splashRadius = 0;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float speed = 4;
    [SerializeField]
    private float damage = 5;
    [SerializeField]
    private bool playOnCreate = false;
    [SerializeField]
    private bool slowEnemy = false;
    [SerializeField]
    private float slowFactor = 0.5f;
    [SerializeField]
    private LayerMask affectedBySplash;

    private bool _hasTarget = false;
    private GameObject _target;
    private Rigidbody2D _rb;
    private bool _played = false;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
	}
	
    public void SetTarget(GameObject target) {
        this._target = target;
        _hasTarget = true;
    }

	// Update is called once per frame
	void Update () {

        if (_hasTarget)
        {

            if (_target != null)
            {
               float z = Mathf.Atan2(
                (_target.transform.position.y - transform.position.y),
                (_target.transform.position.x - transform.position.x)
                 ) * Mathf.Rad2Deg - 90;

                transform.eulerAngles = new Vector3(0, 0, z);
                _rb.AddForce(transform.up * speed);
            } else {
                if (!_played) {
                    PlayAndEnd();
                }
            }
        }
    }

    public void Play() {
        animator.SetTrigger("Play");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!_played && _hasTarget) {
            //OR DIRTS?

            if (other.gameObject.Equals(_target))
            {

                //DAMAGE ENEMY
                if (splashAttack)
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, splashRadius, affectedBySplash, -2, 2);
                    float splashDamage = damage / 2f;
                    for (int i = 0; i < colliders.Length; i++) {
                        colliders[i].GetComponent<Dirt>().WashDamage(splashDamage);
                    }
                    //SPAWN splash effect
                }

                //SLOW EFFECT
                if (slowEnemy) {
                    Ball ball = _target.GetComponent<Ball>();
                    ball.SetSpeedTemporary(ball.GetSpeed() * slowFactor, 2f);
                }

                PlayAndEnd();
                if (_target.transform.childCount != 0)
                {
                    _target.transform.GetChild(0).GetComponent<Dirt>().WashDamage(damage);
                }
            }
        }
    }

    private IEnumerator DestroyAfter(float f) {
        yield return new WaitForSeconds(f);
        Destroy(gameObject);
    }

    private void Die() {
        Destroy(gameObject);
    }

    public bool IsPlayOnCreate() {
        return playOnCreate;
    }

    void PlayAndEnd() {
        if (playOnCreate)
        {
            animator.SetTrigger("Die");
        }
        else
        {
            Play();
        }
        _played = true;
    }
}
