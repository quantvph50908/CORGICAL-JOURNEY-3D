using UnityEngine;
using DG.Tweening;
using DemoKitStylizedAnimatedDogs;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    private MapManager mapManager;
    private float moveDistance = 2f;
    private float moveDuration = 0.3f;
    private float rotationDuration = 0.5f;
    private Animator playerAnimator;
    private bool isMoving = false;
    private bool isRotating = false;
    private bool isHoldingStick = true;
    [SerializeField] private GameObject stickPrefab;
    private GameObject currentStick;
    private float currentAngle = 0f;
    private float pickUpRange = 0.5f;
    public float stickLength = 2f;
    private float previousAngle = 0f;
    private float timeLeft = 60f;
    private TMP_Text timeText;
    private bool isGameOver = false;
    [SerializeField] private GameObject winParticlePrefab;
    [SerializeField] private GameObject loseParticlePrefab;

    void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        playerAnimator = GetComponent<Animator>();
        timeText = FindObjectOfType<TMP_Text>();
        if (stickPrefab != null && currentStick == null)
        {
            SpawnStick();
            isHoldingStick = true;
            playerAnimator.SetInteger("AnimationID", 0);
        }
    }

    private void Update()
    {
        if (isGameOver) return;

        if (Input.GetKeyDown(KeyCode.W) && !isMoving && !isRotating)
        {
            MovePlayer(0, moveDistance);
        }
        if (Input.GetKeyDown(KeyCode.S) && !isMoving && !isRotating)
        {
            MovePlayer(0, -moveDistance);
        }
        if (Input.GetKeyDown(KeyCode.A) && !isMoving && !isRotating)
        {
            MovePlayer(-moveDistance, 0);
        }
        if (Input.GetKeyDown(KeyCode.D) && !isMoving && !isRotating)
        {
            MovePlayer(moveDistance, 0);
        }
        if (Input.GetKeyDown(KeyCode.Q) && !isMoving && !isRotating)
        {
            Rotate(-90f);
        }
        if (Input.GetKeyDown(KeyCode.E) && !isMoving && !isRotating)
        {
            Rotate(90f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleStick();
        }

        UpdateTime();
        CheckWinCondition();
    }

    private void MovePlayer(float dx, float dz)
    {
        if (isGameOver) return;

        Vector3 newPosition = transform.position + new Vector3(dx, 0, dz);
        newPosition.y = transform.position.y;

        if (IsValidPosition(newPosition))
        {
            isMoving = true;
            transform.DOMove(newPosition, moveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => isMoving = false);

            AudioManager.Instance?.PlayMove();

            if (playerAnimator != null && isHoldingStick)
            {
                playerAnimator.SetInteger("AnimationID", 2);
            }
        }
    }

    private void Rotate(float angleDelta)
    {
        if (isGameOver) return;

        if (isRotating) return;

        isRotating = true;
        previousAngle = currentAngle;
        currentAngle = NormalizeAngle(currentAngle + angleDelta);
        float playerTargetAngle = Mathf.Round(currentAngle / 90f) * 90f;

        if (IsCollidingWithTree(playerTargetAngle))
        {
            transform.DOKill();
            if (isHoldingStick && currentStick != null)
            {
                currentStick.transform.DOKill();
            }

            transform.DORotate(new Vector3(0, previousAngle, 0), rotationDuration, RotateMode.Fast)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => isRotating = false);

            if (isHoldingStick && currentStick != null)
            {
                currentStick.transform.DORotate(new Vector3(0, NormalizeAngle(previousAngle + 90f), 0), rotationDuration, RotateMode.Fast)
                    .SetEase(Ease.OutQuad);
            }

            playerAnimator.SetInteger("AnimationID", 1);
            return;
        }

        transform.DORotate(new Vector3(0, playerTargetAngle, 0), rotationDuration, RotateMode.Fast)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => isRotating = false);

        if (isHoldingStick && currentStick != null)
        {
            float stickTargetAngle = NormalizeAngle(playerTargetAngle + 90f);
            currentStick.transform.DORotate(new Vector3(0, stickTargetAngle, 0), rotationDuration, RotateMode.Fast)
                .SetEase(Ease.OutQuad);
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetInteger("AnimationID", 1);
        }
    }

    private float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if (angle < 0) angle += 360f;
        return angle;
    }

    private void ToggleStick()
    {
        if (isGameOver) return;

        if (isMoving || isRotating) return;

        if (isHoldingStick)
        {
            DropStick();
            playerAnimator.SetInteger("AnimationID", 5);
            isHoldingStick = false;
        }
        else
        {
            PickUpStick();
            if (isHoldingStick)
            {
                playerAnimator.SetInteger("AnimationID", 4);
            }
        }
    }

    private void SpawnStick()
    {
        if (stickPrefab != null && currentStick == null)
        {
            currentStick = Instantiate(stickPrefab, transform.position + new Vector3(0, 0.3f, 0.25f), Quaternion.Euler(0, 90, 0));
            currentStick.transform.SetParent(transform);
            currentStick.tag = "Stick";
            MeshCollider meshCollider = currentStick.GetComponent<MeshCollider>();
            if (meshCollider != null)
            {
                meshCollider.convex = true;
                meshCollider.isTrigger = true;
            }
        }
    }

    private void DropStick()
    {
        if (currentStick != null)
        {
            currentStick.transform.SetParent(null);
            isHoldingStick = false;
        }
    }

    private void PickUpStick()
    {
        if (currentStick != null && !isHoldingStick)
        {
            float distance = Vector3.Distance(transform.position, currentStick.transform.position);
            if (distance <= pickUpRange)
            {
                currentStick.transform.position = transform.position + new Vector3(0, 0.3f, 0.25f);
                currentStick.transform.SetParent(transform);
                isHoldingStick = true;
            }
        }
    }

    private bool IsCollidingWithTree(float playerTargetAngle)
    {
        if (currentStick == null || currentStick.GetComponent<MeshCollider>() == null)
        {
            return false;
        }

        float originalAngle = currentStick.transform.eulerAngles.y;
        currentStick.transform.rotation = Quaternion.Euler(0, playerTargetAngle + 90f, 0);

        Collider[] hitColliders = Physics.OverlapBox(currentStick.transform.position, currentStick.GetComponent<MeshCollider>().bounds.size / 2, currentStick.transform.rotation);
        bool isColliding = false;

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Tree"))
            {
                isColliding = true;
                break;
            }
        }

        currentStick.transform.rotation = Quaternion.Euler(0, originalAngle, 0);

        return isColliding;
    }

    private bool IsValidPosition(Vector3 position)
    {
        if (mapManager == null) return false;

        Vector2 roundedXZ = new Vector2(
            Mathf.Round(position.x),
            Mathf.Round(position.z)
        );

        bool isOnBlock = false;
        if (mapManager.blockPositions != null)
        {
            foreach (Vector3 blockPos in mapManager.blockPositions)
            {
                Vector2 blockXZ = new Vector2(blockPos.x, blockPos.z);
                if (Vector2.Equals(blockXZ, roundedXZ))
                {
                    isOnBlock = true;
                    break;
                }
            }
        }

        bool isTreePresent = false;
        if (mapManager.treePositions != null)
        {
            foreach (Vector3 treePos in mapManager.treePositions)
            {
                Vector2 treeXZ = new Vector2(
                    Mathf.Round(treePos.x),
                    Mathf.Round(treePos.z)
                );
                if (Vector2.Equals(treeXZ, roundedXZ))
                {
                    isTreePresent = true;
                    break;
                }
            }
        }

        return isOnBlock && !isTreePresent;
    }

    private void CheckWinCondition()
    {
        if (mapManager == null || isGameOver) return;

        Vector2 playerXZ = new Vector2(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.z)
        );
        Vector2 pointXZ = new Vector2(
            Mathf.Round(mapManager.pointPosition.x),
            Mathf.Round(mapManager.pointPosition.z)
        );

        if (Vector2.Equals(playerXZ, pointXZ) && isHoldingStick)
        {
            if (winParticlePrefab != null)
            {
                ParticleSystem ps = Instantiate(winParticlePrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            AudioManager.Instance?.Win();
            isGameOver = true;
            GameManager.Instance.OnWin();
        }
        else if (Vector2.Equals(playerXZ, pointXZ) && !isHoldingStick)
        {
        }
    }

    private void UpdateTime()
    {
        if (isGameOver) return;

        timeLeft -= Time.deltaTime;
        if (timeText != null)
        {
            timeText.text = $"Time: {Mathf.Max(0, timeLeft):F1}";
        }

        if (timeLeft <= 0)
        {
            if (loseParticlePrefab != null)
            {
                ParticleSystem ps = Instantiate(loseParticlePrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            AudioManager.Instance?.Lose();
            isGameOver = true;
            timeLeft = 0;
        }
    }
}