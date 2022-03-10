using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [Header("Base Coin Properties")]
    [SerializeField] private GameObject basicCoin;
    [SerializeField] private Vector3 basicCoinSpawnOffset;
    [SerializeField] private List<Vector2Int> coinSpawnIgnoreNodePositions;
    [SerializeField] private Vector3 initialCoinSpawn;
    [SerializeField] private float spawnSpeed;
    [SerializeField] private Transform coinParentTransform;

    [Header("Special Coin Properties")]
    [SerializeField] private GameObject specialCoin;
    [SerializeField] private List<Vector2Int> specialCoinNodePositions;
    [SerializeField] private Transform specialCoinParentTransform;

    [Header("Blocker Platforms")]
    [SerializeField] private List<GameObject> blockerPlatforms;
    [SerializeField] private Material blockerStateIndicator;

    [Header("Camers")]
    [SerializeField] private GameObject cinematicCamera;
    [SerializeField] private GameObject packmanCamera;
    [SerializeField] private Vector3 cameraInitialPosition;
    [SerializeField] private Vector3 cameraInitialRotation;

    public static bool buildLevel = false;
    public static bool startGame = false;

    private int totalCoint;
    void Start()
    {
        MonsterStateController.OnStateChange += OnMonsterStateChange;

        CalculateTotalCoinsToWinTheGame();
        StartCoroutine(StartGame());
    }

    private void CalculateTotalCoinsToWinTheGame()
    {
        Node[,] gridNodes = GridBuilder.Instance.GridNodes;
        for (int i = 0; i < GridBuilder.Instance.Width; i++)
        {
            for (int j = 0; j < GridBuilder.Instance.Height; j++)
            {
                if (!CanSpawnCoin(i, j))
                    continue;

                totalCoint += 1;
            }
        }

        ScoreAndLevelManager.Instance.SetTotalScoreToCompleteLevel(totalCoint);
    }

    private IEnumerator StartGame()
    {
        while(!buildLevel)
        {
            yield return null;
        }

        cinematicCamera.transform.DOMove(cinematicCamera.transform.position + (cinematicCamera.transform.forward * 100), 50f);
        StartCoroutine(CoinSpawnAnimator());
        SpecialCoinSpawning();
    }

    private IEnumerator CoinSpawnAnimator()
    {
        Node[,] gridNodes = GridBuilder.Instance.GridNodes;
        for (int i = 0; i < GridBuilder.Instance.Width; i++)
        {
            for (int j = 0; j < GridBuilder.Instance.Height; j++)
            {
                if (!CanSpawnCoin(i, j))
                    continue;

                GameObject coin = Instantiate(basicCoin, (gridNodes[i, j].position + basicCoinSpawnOffset) + initialCoinSpawn, Quaternion.identity);
                if (coinParentTransform != null)
                    coin.transform.SetParent(coinParentTransform);
                coin.transform.DOMove(gridNodes[i, j].position + basicCoinSpawnOffset, 5);
                yield return new WaitForSeconds(1 / (spawnSpeed) * Time.deltaTime);
            }
        }

        foreach (var item in blockerPlatforms)
        {
            yield return new WaitForSeconds(0.2f);
            item.transform.DOMove(item.transform.position + new Vector3(0, -100, 0), 2).SetEase(Ease.OutBounce);
        }

        OnGameStart();
    }

    private void OnGameStart()
    {
        startGame = true;
        AudioManager.Instance.PlaySound("PackmanChomp");

        cinematicCamera.transform.DOLocalMove(cameraInitialPosition, 4f);
        cinematicCamera.transform.DOLocalRotate(cameraInitialRotation, 4f).OnComplete(() =>
        {
            cinematicCamera.SetActive(false);
            packmanCamera.SetActive(true);
        });

    }

    private void SpecialCoinSpawning()
    {
        Node[,] gridNodes = GridBuilder.Instance.GridNodes;
        foreach (var item in specialCoinNodePositions)
        {
            Transform specialCoinSpawnObject = Instantiate(specialCoin.transform, (gridNodes[item.x, item.y].position), Quaternion.identity);
            if (specialCoinParentTransform != null)
                specialCoinSpawnObject.SetParent(specialCoinParentTransform);
        }
    }

    private bool CanSpawnCoin(int i, int j)
    {
        return !(GridBuilder.Instance.GridNodes[i, j].NodeType == NodeType.Block)
               && !coinSpawnIgnoreNodePositions.Contains(new Vector2Int(i, j))
               && !specialCoinNodePositions.Contains(new Vector2Int(i, j));
    }

    private void OnMonsterStateChange(CurrentGameMode currentGameMode)
    {
        if(currentGameMode == CurrentGameMode.Chase)
        {
            //blockerStateIndicator.color = Color.red;
            //blockerStateIndicator.SetColor("_EmissionColor", Color.red);
        }

        else if(currentGameMode == CurrentGameMode.Scatter)
        {
            //blockerStateIndicator.color = new Color(0, 0.8126423f, 1,1);
            //blockerStateIndicator.SetColor("_EmissionColor", new Color(0, 12.11845f, 14.92853f));
        }
    }
}
