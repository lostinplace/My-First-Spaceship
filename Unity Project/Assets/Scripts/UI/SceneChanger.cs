using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger
{
    public static string gameOverMessage;
    public static void LoadGame() {
        SceneManager.LoadScene("new_layout");
    }
    public static void GameOver( string message )
    {
        gameOverMessage = message;
        UnityEngine.GameObject test = UnityEngine.GameObject.Instantiate(new GameObject());
        UnityEngine.GameObject.DontDestroyOnLoad(test);
        foreach (GameObject current in test.scene.GetRootGameObjects())
            UnityEngine.GameObject.Destroy(current);
        SceneManager.LoadScene("GameOver");
    }
    public static void GoToMainMenu() {
        SceneManager.LoadScene("main_menu");
    }
}
