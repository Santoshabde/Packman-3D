using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class PlatformScoreUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private GameObject livesIndicator;
    [SerializeField] private Vector2 packManPositionsLimitToActivate_Min;
    [SerializeField] private Vector2 packManPositionsLimitToActivate_Max;

    private Transform packman;
    private List<GameObject> lifeIndicator;

    private void Awake()
    {
        packman = FindObjectOfType<PackmanLocomotion>().transform;

        //Life indicator
        if (livesIndicator != null)
        {
            lifeIndicator = new List<GameObject>();
            for (int i = 0; i < livesIndicator.transform.childCount; i++)
            {
                lifeIndicator.Add(livesIndicator.transform.GetChild(i).gameObject);
            }
        }

        ScoreAndLevelManager.OnScoreUpdate += OnScoreUpdate;
        ScoreAndLevelManager.OnLivesUpdate += OnLivesUpdate;
    }
    
    private void Update()
    {
        if (packman.position.z >= packManPositionsLimitToActivate_Min.y && packman.position.z <= packManPositionsLimitToActivate_Max.y
            &&
            packman.position.x >= packManPositionsLimitToActivate_Min.x && packman.position.x <= packManPositionsLimitToActivate_Max.x)
        {
            if (scoreText != null)
                scoreText.enabled = true;

            if (livesIndicator != null)
                livesIndicator.SetActive(true);
        }
        else
        {
            if (scoreText != null)
                scoreText.enabled = false;

            if (livesIndicator != null)
                livesIndicator.SetActive(false);
        }
    }

    private void OnScoreUpdate(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString("00");
        }
    }

    private void OnLivesUpdate(int livesLeft)
    {
        if (livesIndicator != null)
        {
            lifeIndicator.ForEach(t => t.SetActive(false));
            for (int i = 0; i < livesLeft; i++)
            {
                lifeIndicator[i].SetActive(true);
            }
        }
    }
}
