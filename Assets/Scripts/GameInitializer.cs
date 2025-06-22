using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    void Start()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.InitializeCurrentLevel();
        }
    }

    void OnDestroy()
    {
        // Đảm bảo không tạo thêm instance khi scene bị hủy
        if (LevelManager.Instance == this)
        {
            LevelManager.Instance = null;
        }
    }
}