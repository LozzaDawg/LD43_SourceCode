using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour {

	public void startGame()
    {
        SceneManager.LoadScene("World_Map");
    }
    public void toTitleScreen()
    {
        SceneManager.LoadScene("Title_Screen");
    }
    public void quitGame()
    {
        Debug.Log("quit game");
        Application.Quit();
    }
}
