using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private GameObject[] levelPrefabs;

    private int currentLevelIndex = 0;

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

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelPrefabs.Length)
        {
            currentLevelIndex = levelIndex;
            SceneManager.LoadScene("Game");
        }
    }

    public void LoadNextLevel()
    {
        if (currentLevelIndex < levelPrefabs.Length - 1)
        {
            currentLevelIndex++;
            ClearCurrentLevel();
            Instantiate(levelPrefabs[currentLevelIndex], Vector3.zero, Quaternion.identity);
        }
    }

    public void ResetCurrentLevel()
    {
        ClearCurrentLevel();
        Instantiate(levelPrefabs[currentLevelIndex], Vector3.zero, Quaternion.identity);
    }

    private void ClearCurrentLevel()
    {
        GameObject[] levelObjects = GameObject.FindGameObjectsWithTag("Level");
        if (levelObjects.Length > 0)
        {
            foreach (GameObject obj in levelObjects)
            {
                Destroy(obj);
            }
        }

        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj != gameObject &&
                obj.GetComponent<LevelManager>() == null &&
                obj.GetComponent<GameInitializer>() == null &&
                obj.GetComponent<PlayerMovement>() == null &&
                obj.GetComponent<MapManager>() == null &&
                obj.GetComponent<Camera>() == null &&
                obj.GetComponent<Light>() == null &&
                obj.GetComponent<UnityEngine.Rendering.Volume>() == null &&
                obj.GetComponent<UnityEngine.EventSystems.EventSystem>() == null &&
                obj.GetComponent<GameManager>() == null &&
                obj.GetComponent<AudioSource>() == null &&
                obj.GetComponent<AudioListener>() == null)
            {
                Destroy(obj);
            }
        }
    }

    public void InitializeCurrentLevel()
    {
        if (levelPrefabs != null && levelPrefabs.Length > 0 && Instance != null && gameObject.activeInHierarchy)
        {
            ClearCurrentLevel();
            GameObject levelInstance = Instantiate(levelPrefabs[currentLevelIndex], Vector3.zero, Quaternion.identity);

            // Trì hoãn gắn sự kiện để đảm bảo scene sẵn sàng
            StartCoroutine(SetupButtons(levelInstance));
        }
    }

    private IEnumerator SetupButtons(GameObject levelInstance)
    {
        // Chờ 1 frame để đảm bảo hierarchy được cập nhật
        yield return null;

        // Tìm tất cả button trong hierarchy con
        Button[] buttons = levelInstance.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            if (button != null)
            {
                if (button.gameObject.name == "Home")
                {
                    button.onClick.AddListener(LoadMenu);
                }
                else if (button.gameObject.name == "Reset")
                {
                    button.onClick.AddListener(ResetCurrentLevel);
                }
            }
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}