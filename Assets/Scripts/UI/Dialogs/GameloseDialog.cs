using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameloseDialog : BaseDialog
{
    [SerializeField] private Button tryAgain;

    private void Awake()
    {
        tryAgain.onClick.AddListener(OnTryAgainButtonClicked);
    }

    private void OnTryAgainButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnOpenDialog()
    {
        base.OnOpenDialog();

    }

    public override void OnCloseDialog()
    {
        base.OnCloseDialog();

    }
}
