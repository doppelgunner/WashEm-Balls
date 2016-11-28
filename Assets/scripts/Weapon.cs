using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    [SerializeField]
    private LayerMask canBeTargeted;
    [SerializeField]
    private float radius = 3f;
    [SerializeField]
    private float attackCD;
    [SerializeField]
    private GameObject attack;
    [SerializeField]
    private Animator animator;
    

    private float _currentWaitTime;
    private bool _wait;

	// Use this for initialization
	void Start () {
        _wait = false;
        _currentWaitTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void FixedUpdate() {

        if (!_wait) {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, radius, canBeTargeted, -2, 2);
            if (collider != null)
            {
                GameObject dirt = collider.gameObject;

                float z = Mathf.Atan2(
                    (dirt.transform.position.y - transform.position.y),
                    (dirt.transform.position.x - transform.position.x)
                    ) * Mathf.Rad2Deg - 90;

                transform.eulerAngles = new Vector3(0, 0, z);

                //ATTACK
                GameObject attackObject = Instantiate(attack,transform.position, transform.rotation) as GameObject;
                Attack attackScript = attackObject.GetComponent<Attack>();
                attackScript.SetTarget(collider.transform.parent.gameObject);
                if (attackScript.IsPlayOnCreate()) {
                    attackScript.Play();
                }
                animator.SetTrigger("Attack");
                _wait = true;
            }
        } else {
            _currentWaitTime += Time.deltaTime;
        }

        if (_currentWaitTime >= attackCD) {
            _currentWaitTime = 0;
            _wait = false;
        }
   
    }

    public enum Type {
        WATER_GUN,
        WATER_TURRET, 
        STICKY_SOAP,
        BUCKET_CATAPULT,
        NULL
    }

    public void LockOnTarget() {

    }

    public static int GetCost()
    {
        switch (GameManager.GetSelectedWeapon())
        {
            case Weapon.Type.WATER_GUN:
                return 2;
            case Weapon.Type.WATER_TURRET:
                return 5;
            case Weapon.Type.STICKY_SOAP:
                return 5;
            case Weapon.Type.BUCKET_CATAPULT:
                return 10;
            case Weapon.Type.NULL:
            default:
                return 0;
        }
    }

    public static int GetCost(Type type) {
        switch (type)
        {
            case Weapon.Type.WATER_GUN:
                return 2;
            case Weapon.Type.WATER_TURRET:
                return 5;
            case Weapon.Type.STICKY_SOAP:
                return 5;
            case Weapon.Type.BUCKET_CATAPULT:
                return 10;
            case Weapon.Type.NULL:
            default:
                return 0;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
