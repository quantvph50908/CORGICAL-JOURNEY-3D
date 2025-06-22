using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.SetActive(true);
         
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void OnWin()
    {
        if (gameObject.activeInHierarchy) 
        {
            StartCoroutine(TransitionToNextLevel());
        }
        else
        {
            gameObject.SetActive(true);
            StartCoroutine(TransitionToNextLevel());
        }
    }

    private IEnumerator TransitionToNextLevel()
    {
        yield return new WaitForSeconds(2f); 
        if (LevelManager.Instance != null && gameObject.activeInHierarchy)
        {
            LevelManager.Instance.LoadNextLevel();
        }
    }
}