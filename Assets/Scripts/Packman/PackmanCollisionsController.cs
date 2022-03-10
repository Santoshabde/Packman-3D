using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackmanCollisionsController : MonoBehaviour
{
    public static Action OnCoinCollected; 
    public static Action OnSpecialCoinCollected; 
    public static Action OnPackmanDeath; 
    public static Action OnFrightenedMonsterCollision; 

    private void OnTriggerEnter(Collider other)
    {
        MonsterStateController stateController = MonsterStateController.Instance;

        //Coins
        if (other.tag == "Coin")
        {
            other.gameObject.SetActive(false);
            OnCoinCollected?.Invoke();
        }

        //Special Coin
        if (other.tag == "SpecialCoin")
        {
            OnSpecialCoinCollected?.Invoke();

            if (stateController != null)
                stateController.OnPackmanPowerUp();

            other.gameObject.SetActive(false);

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySound("PackmanEatfruit");
        }

        //Monster
        if(other.GetComponent<Monster>() != null)
        {
            Monster collidedMonster = other.GetComponent<Monster>();
            if (collidedMonster.CurrentMonsterState == collidedMonster.monsterFrigtened)
            {      
                OnFrightenedMonsterCollision?.Invoke();
                stateController.ChangeMonsterState(collidedMonster, collidedMonster.monsterEaten);

                if (AudioManager.Instance != null)
                    AudioManager.Instance.PlaySound("PackmanEatGhost");
            }
            else
            {
                OnPackmanDeath?.Invoke();
            }
        }
    }
}
