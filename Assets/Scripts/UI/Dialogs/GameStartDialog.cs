using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameStartDialog : BaseDialog
{
    [SerializeField] private Button playButton;
    [SerializeField] private Image titleImage;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        transform.GetComponent<Image>().material.SetFloat("_Size", 4.3f);
    }

    public override void OnOpenDialog()
    {
        base.OnOpenDialog();
        transform.GetComponent<Image>().material.SetFloat("_Size", 4.3f);
    }

    public override void OnCloseDialog()
    {
        base.OnCloseDialog();
        transform.GetComponent<Image>().material.SetFloat("_Size", 4.3f);
    }

    private void OnPlayButtonClicked()
    {
        transform.GetComponent<Image>().material.DOFloat(0, "_Size", 3);

        playButton.interactable = false;
        titleImage.transform.DOMoveY(titleImage.transform.position.y + 600, 1);
        playButton.transform.DOMoveY(playButton.transform.position.y - 330, 1).OnComplete(() => {
            LevelBuilder.buildLevel = true;
            UIManager.Instance.CloseDialog<GameStartDialog>();
        });    
    }
}
