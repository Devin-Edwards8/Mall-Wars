using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private TMP_Text lastResultText;
    void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string lastResult = PlayerPrefs.GetString("lastResult");
        lastResultText.text = $"Last Result: {lastResult}";
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}