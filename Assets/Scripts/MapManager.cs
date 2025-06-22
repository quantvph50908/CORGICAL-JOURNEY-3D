using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] public GameObject blockPrefab; // Kéo BlockPrefab vào đây
    [SerializeField] public Vector3[] blockPositions; // Danh sách vị trí ô, chỉnh sửa trong Inspector
    [SerializeField] public GameObject treePrefab; // Kéo TreePrefab vào đây
    [SerializeField] public Vector3[] treePositions; // Danh sách vị trí cây, chỉnh sửa trong Inspector
    [SerializeField] public GameObject pointPrefab; // Kéo PointPrefab vào đây
    [SerializeField] public Vector3 pointPosition; // Vị trí duy nhất của điểm, chỉnh sửa trong Inspector
    [SerializeField] public GameObject playerPrefab; // Kéo PlayerPrefab vào đây
    [SerializeField] private Vector3 playerPosition; // Vị trí duy nhất của player, chỉnh sửa trong Inspector

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        if (blockPositions != null)
        {
            foreach (Vector3 position in blockPositions)
            {
                Instantiate(blockPrefab, position, Quaternion.identity);
            }
        }

        if (treePositions != null)
        {
            foreach (Vector3 position in treePositions)
            {
                Instantiate(treePrefab, position + Vector3.up * 0.5f, Quaternion.identity);
            }
        }

        if (pointPosition != Vector3.zero) // Kiểm tra nếu vị trí điểm được đặt
        {
            Instantiate(pointPrefab, pointPosition, Quaternion.identity);
        }

        if (playerPosition != Vector3.zero) // Kiểm tra nếu vị trí player được đặt
        {
            Instantiate(playerPrefab, playerPosition + Vector3.up, Quaternion.identity);
        }
    }
}