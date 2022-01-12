using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Movement move;
    //public static bool firstPlay;
    

    
    public void PlayWithClearData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("PlayedBefore", 0);
        SceneManager.LoadScene("Game");
        //firstPlay = true;
    }

    public void PlayAgain() //Issue: clicking play again sets playedbefore to true, i don't want that
    {
        //firstPlay = false;
        
        PlayerPrefs.SetInt("PlayedBefore", 1);
        SceneManager.LoadScene("Game");
        
    }

    public void Menu()
    {
        SceneManager.LoadScene("Title Scene");
    }

    
}
