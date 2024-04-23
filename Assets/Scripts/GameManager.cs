using Vector2 = UnityEngine.Vector2;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject block;
    [SerializeField] private int numBlocks;
    [SerializeField] private int[] bounds;
    [SerializeField] private float time;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text scoreText;
    private List<GameObject> activeBlocks;
    void Start()
    {
        activeBlocks = new List<GameObject>();
        for (int i = 0; i < numBlocks; i++)
        {
            Vector2 spawnPoint = new Vector2(Random.Range(bounds[0], bounds[1]), Random.Range(bounds[2], bounds[3]));
            GameObject go = Instantiate(block, spawnPoint, Quaternion.Euler(0, 0, 0));
            activeBlocks.Add(go);
        }
        scoreText.text = $"Crates Left: {numBlocks}";
        timeText.text = $"Crates Left: {Mathf.FloorToInt(time)}";
    }

    void Update()
    {
        // score updates
        int mark = -1;
        for(int i = 0; i < activeBlocks.Count; i++)
        {
            if(!activeBlocks[i].activeSelf)
            {
                // Destroy(activeBlocks[i]);
                mark = i;
            }
        }
        if(mark != -1) { activeBlocks.RemoveAt(mark); }
        if (activeBlocks.Count == 0) {
            PlayerPrefs.SetString("lastResult", "Won!");
            SceneManager.LoadScene(0);
        }
        scoreText.text = $"Crates Left: {activeBlocks.Count}";

        // time updates
        time -= Time.deltaTime;
        timeText.text = $"Time: {Mathf.FloorToInt(time)}";
        if(time <= 0)
        {
            timeText.text = "Time: 0";
            PlayerPrefs.SetString("lastResult", "Lost... Try Again!");
            SceneManager.LoadScene(0);
        }
    }
}