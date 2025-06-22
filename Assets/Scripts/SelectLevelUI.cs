using UnityEngine;
using UnityEngine.UI;

public class SelectLevelUI : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons; 

    void Start()
    {
        if (levelButtons == null || levelButtons.Length != 15)
        {
            return;
        }
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i; 
            levelButtons[i].onClick.AddListener(() => OnLevelSelected(levelIndex));
        }
    }

    private void OnLevelSelected(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < 15)
        {
            LevelManager.Instance.LoadLevel(levelIndex); 
            gameObject.SetActive(false); 
        }
    }
}