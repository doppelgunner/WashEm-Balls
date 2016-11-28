using UnityEngine;
using System.Collections;

public class Dirt : MonoBehaviour {

    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private Type type;

    private int _currentSpriteIndex;

    private SpriteRenderer _sr;
    private float _currentHealth;

    // Use this for initialization
    void Start () {
        _sr = GetComponent<SpriteRenderer>();

        _currentSpriteIndex = 0;
        SetCurrentSprite();

        _currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void SetCurrentSprite() {
        _sr.sprite = sprites[_currentSpriteIndex];
    }

    public void WashDamage(float f) {
        if (_currentHealth > 0) {
            _currentHealth -= f;

            ComputeSprite();
        } else {
            //Destroy(); or remove
        }
    }

    private void ComputeSprite() {
        float p = GetHealthPercentage();

        if (p <= 0) {

            GameManager.AddMoney(GetMoneyDrop());
            Destroy(gameObject);
        }   else if (p <= 20) {
            _currentSpriteIndex = _20_DOWN;
            SetCurrentSprite();
        }   else if (p <= 40) {
            _currentSpriteIndex = _40_DOWN;
            SetCurrentSprite();
        }   else if (p <= 60) {
            _currentSpriteIndex = _60_DOWN;
            SetCurrentSprite();
        }   else if (p <= 80) {
            _currentSpriteIndex = _80_DOWN;
            SetCurrentSprite();
        }
    }

    public float GetHealthPercentage() {
        float quotient = _currentHealth / maxHealth;
        return quotient * 100;
    }

    public static int
        _80_DOWN = 1,
        _60_DOWN = 2,
        _40_DOWN = 3,
        _20_DOWN = 4;

    public enum Type {
        COMB,PEE,POOP,DIRT,NULL
    }

    public bool isClean() {
        return _currentHealth <= 0;
    }

    public int GetMoneyDrop() {
        switch (type) {
            case Type.DIRT:
                return 1;
            case Type.PEE:
                return 2;
            case Type.POOP:
                return 3;
            case Type.COMB:
                return 5;
            default:
                return 0;
        }
    }

    public float GetSpeed() {
        return speed;
    }

    public void AddMaxHealth(float f) {
        maxHealth += f;
        _currentHealth += f;
    }

    public void MultiplyMaxHealth(float factor) {
        maxHealth *= factor;
        _currentHealth *= factor;
    }
}
