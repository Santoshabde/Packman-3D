using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterState
{
    void OnEnter(Monster monster);
    void OnExecute(Monster monster);
    void OnExit(Monster monster);
}
