using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PackmanVFXManager : MonoBehaviour
{
    [SerializeField] private GameObject wallhitEffect;

    [Header("Path Trail")]
    [SerializeField] private GameObject packmanTrail;
    [SerializeField] private bool activateTrail;

    [Header("LightRay Effect")]
    [SerializeField] private GameObject lightRay;
    [SerializeField] private bool activateLightRay;

    [Header("Coins Collection Effect")]
    [SerializeField] private ParticleSystem coinCollectionEffect;
    [SerializeField] private ParticleSystem specialCoinCollectionEffect;

    [Header("Dissolve Effect")]
    [SerializeField] private Material packManMaterial;

    private GameObject packman;

    public static Action OnPackManRespawn;
    private void Awake()
    {
        packman = FindObjectOfType<PackmanLocomotion>().gameObject;

        PackmanLocomotion.OnWallHit += OnPackmanWallHit;
        PackmanCollisionsController.OnCoinCollected += () => { if(coinCollectionEffect != null) coinCollectionEffect.Play(); };
        PackmanCollisionsController.OnSpecialCoinCollected += () => { if(specialCoinCollectionEffect != null) specialCoinCollectionEffect.Play(); };
        PackmanCollisionsController.OnPackmanDeath += OnPackmanGettingEaten;

        SetupRequiredVFX();
    }

    private void OnPackmanWallHit(bool didHit)
    {
        if(wallhitEffect != null)
        wallhitEffect.SetActive(didHit);
    }

    private void SetupRequiredVFX()
    {
        //Trail!!
        if (lightRay != null)
        {
            if (activateTrail)
                packmanTrail.SetActive(true);
            else
                packmanTrail.SetActive(false);
        }

        //Light Ray!!
        if (lightRay != null)
        {
            if (activateLightRay)
                lightRay.SetActive(true);
            else
                lightRay.SetActive(false);
        }

        //Re-Dissolve
        if (packManMaterial != null)
            packManMaterial.SetFloat("_DissolveAmount", 0);
    }

    private void OnPackmanGettingEaten()
    {
        if (packManMaterial != null)
            packManMaterial.DOFloat(1, "_DissolveAmount", 2f).OnComplete(() => {
                if (!(GameStateManager.Instance.CurrentGameState == GameState.GameLose))
                {
                    OnPackManRespawn?.Invoke();
                    packManMaterial.SetFloat("_DissolveAmount", 0);
                }
            });
    }
}
