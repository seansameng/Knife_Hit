using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Hide main menu and start GamePanel
        // Assuming you have GamePanel in your scene
        SceneManager.LoadScene("GameScene"); // Your game scene name
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
