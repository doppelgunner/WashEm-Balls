using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject[] paths;
    
    private Sprite _currentDirtSprite;
    private int _currentDirtSpriteIndex;
    private GameObject _currentPath;
    private int _currentPathIndex;

    private Rigidbody2D _rb;

    private bool _finish = false;
    private Dirt dirt;

    private float origSpeed;
    private bool slowed;
    private float slowLimit;
    private float slowCounter;

	// Use this for initialization
	void Start () {
        slowed = false;
        origSpeed = speed;
        _currentDirtSpriteIndex = 0;
        _currentPathIndex = 0;
        _currentPath = paths[_currentPathIndex];

        _rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!_finish) {
             float z = Mathf.Atan2(
             (_currentPath.transform.position.y - transform.position.y),
             (_currentPath.transform.position.x - transform.position.x)
             ) * Mathf.Rad2Deg + 90;

                transform.eulerAngles = new Vector3(0, 0, z);
                _rb.AddForce(transform.up * -speed);
        }

        if (_currentPath.transform.position.y > transform.position.y) {
            if (++_currentPathIndex < paths.Length) {
                _currentPath = paths[_currentPathIndex];
            } else {
                _finish = true;
            }
        }

        if (slowed) {
            slowCounter += Time.deltaTime;
            if (slowCounter > slowLimit) {
                SetOrigSpeed();
            }
        }

            /*
        if (_finish) {
            //Damage();
            //Disappear();
        }
        */
    }

    public void SetSpeedTemporary(float f, float time) {
        if (!IsSlowed()) {
            speed = f;
            slowed = true;
            slowLimit = time;
            slowCounter = 0;
        } else {
            slowCounter = 0;
            slowLimit = time;
        }
    }

    public float GetSpeed() {
        return speed;
    }

    public bool IsSlowed() {
        return slowed;
    }

    void SetOrigSpeed() {
        speed = origSpeed;
        slowed = false;
    }

    public void AddSpeed(float f) {
        speed += f;
        origSpeed = speed;
    }
}
