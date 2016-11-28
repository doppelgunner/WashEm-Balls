using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    [SerializeField]
    private bool placeable = false;

    private SpriteRenderer _sr;

    private static Color green = Color.green;
    private static Color red = Color.red;
    private static Color white = new Color(1,1,1,1);

    // Use this for initialization
    void Start () {
        _sr = GetComponent<SpriteRenderer>();
        if (placeable) {
            gameObject.AddComponent<BoxCollider2D>();
        } else {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnMouseEnter()
    {
        switch (GameManager.GetSelectedWeapon()) {
            case Weapon.Type.NULL:
                return;
            case Weapon.Type.WATER_GUN:
            case Weapon.Type.WATER_TURRET:
            case Weapon.Type.STICKY_SOAP:
            case Weapon.Type.BUCKET_CATAPULT:
                break;
        }
        HighlightTile();
        GameManager.SetSelectedTile(gameObject);
    }

    void OnMouseExit() {
       UnHighlightTile();
        GameManager.RemoveSelectedTile(gameObject);
    }

    public void HighlightTile() {
        if (placeable)
        {
            _sr.color = green;
        }
        else
        {
            _sr.color = red;
        }
    }

    public void UnHighlightTile() {
        _sr.color = white;
    }

    public bool IsPlaceable() {
        return placeable;
    }

    public void SetPlaceable(bool b) {
        placeable = b;
    }
}
