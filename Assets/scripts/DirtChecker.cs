using UnityEngine;
using System.Collections;

public class DirtChecker : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "dirts") {
            if (!GameManager.IsGameOver()) {
                GameManager.AddLife(-1);
                GameManager.CheckLose();
            }
        } 

        if (other.tag == "ball") {
            StartCoroutine(DestroyAfter(1f, other.gameObject));
            GameManager.DecreaseBallsOnScene();
        }
    }
    
    IEnumerator DestroyAfter(float f, GameObject go) {
        yield return new WaitForSeconds(f);
        Destroy(go);
    }
}
