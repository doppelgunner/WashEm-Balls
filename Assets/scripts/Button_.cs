using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class Button_ : MonoBehaviour {

	public void GoToScene(string name) {
        SceneManager.LoadScene(name);
    }

    public void Quit() {
        Application.Quit();
    }
}
