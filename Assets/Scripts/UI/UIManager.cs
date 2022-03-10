using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SerializeSingleton<UIManager>
{
    public static List<BaseDialog> dialogsInGame;

    public void OpenDialog<T>() where T: BaseDialog
    {
        foreach (var dialog in dialogsInGame)
        {
            if (Types.Equals(typeof(T), dialog.GetType()))
            {
                dialog.OnOpenDialog();
            }
        }
    }

    public void CloseDialog<T>() where T: BaseDialog
    {
        foreach (var dialog in dialogsInGame)
        {
            if (Types.Equals(typeof(T), dialog.GetType()))
            {
                dialog.OnCloseDialog();
            }
        }
    }

    public void CloseAllDialogs()
    {
        foreach (var dialog in dialogsInGame)
        {
            dialog.OnCloseDialog();
        }
    }
}
