using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDialog : MonoBehaviour
{
    /// <summary>
    /// Functionality on Opening Dialog
    /// </summary>
    public virtual void OnOpenDialog()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Functionality on Closing Dialog
    /// </summary>
    public virtual void OnCloseDialog()
    {
        gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        if (UIManager.dialogsInGame == null)
            UIManager.dialogsInGame = new List<BaseDialog>();

        for (int i = UIManager.dialogsInGame.Count - 1; i >= 0; i--)
        {
            if (UIManager.dialogsInGame[i] == null)
            {
                UIManager.dialogsInGame.RemoveAt(i);
            }
        }

        if (!UIManager.dialogsInGame.Contains(this))
        {
            UIManager.dialogsInGame.Add(this);
        }
    }
}
