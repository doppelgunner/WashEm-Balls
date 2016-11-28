using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mouse.x, mouse.y, transform.position.z);
	}


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "tiles") return;
        other.GetComponent<Tile>().HighlightTile();
        GameManager.AddHighlightedCell(other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag != "tiles") return;
        other.GetComponent<Tile>().UnHighlightTile();
        GameManager.RemoveHighlightedCell(other.gameObject);
    }
}
