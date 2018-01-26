using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public void ClickStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
}
