using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWinDialog : BaseDialog
{
    [SerializeField] private Button tryAgain;
    [SerializeField] private Text currentCompletionTime;
    [SerializeField] private GameObject isNewRecord;

    private void Awake()
    {
        CountDownTimer.OnTimerStopped += OnTimerStopped;

        tryAgain.onClick.AddListener(OnTryAgainButtonClicked);
    }

    private void OnTimerStopped(float minutes, float seconds)
    {
        currentCompletionTime.text = minutes.ToString("00") + ":" + Mathf.Round(seconds).ToString("00");
    }

    private void OnTryAgainButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnOpenDialog()
    {
        base.OnOpenDialog();
        Debug.Log("Game Win Dialog Opened!!");
    }

    public override void OnCloseDialog()
    {
        base.OnCloseDialog();
    }
}
