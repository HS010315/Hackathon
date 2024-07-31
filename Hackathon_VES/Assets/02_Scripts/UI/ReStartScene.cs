using UnityEngine;
using UnityEngine.SceneManagement;


public class ReStartScene : MonoBehaviour
{
    public GameTimer gameTimer;
    public void RestartScene()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void RestartGame()
    {
        StopAllCoroutines();
        gameTimer.elapsedTime = 0f;
        gameTimer.timerStarted = false;
        gameTimer.countdownCoroutine = null;
    }
}
