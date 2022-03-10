using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField, ReadOnly] protected string currentDebugStateString;
    [SerializeField] protected Transform raycastFeetPoint;
    [SerializeField] protected LayerMask ignoreRayCastLayer;
    [SerializeField] protected float speed;
    [SerializeField] protected PackmanLocomotion packman;

    [Header("Scatter Mode Nodes")]
    [SerializeField] protected List<Vector2Int> idleRoamNodes;
    [SerializeField] protected MeshRenderer bodyMesh;
    [SerializeField] protected GameObject monsterParticleEffect;

    protected Node currentNode;
    protected Vector3 target;

    protected bool freezeMotion = false;

    public Node CurrentNode { get { return currentNode; } }
    public float Speed { get { return speed; } set { speed = value; } }
    public Vector3 Target => target;
    public List<Vector2Int> IdleRoamNodes => idleRoamNodes;
    public MeshRenderer BodyMesh => bodyMesh;
    public GameObject MonsterParticleEffect => monsterParticleEffect;
    public IMonsterState CurrentMonsterState { get { return currentMonsterState; } set { currentMonsterState = value; } }

    #region States
    //Current State and previous state
    private IMonsterState currentMonsterState;
    private IMonsterState previousMonsterState;

    //States
    public Monster_Chase monsterChase;
    public Monster_Frigtened monsterFrigtened;
    public Monster_Scatter monsterScatter;
    public Monster_Eaten monsterEaten;
    #endregion

    protected virtual void Awake()
    {
        MonsterStateController.OnStateChange += OnStateChanged;
        ScoreAndLevelManager.OnGameOver += () => freezeMotion = true;
    }

    protected virtual void Start()
    {
        //States
        //Chase State!!
        monsterChase = new Monster_Chase();
        //Frightened State!!
        monsterFrigtened = new Monster_Frigtened();
        //Scatter State - Idle!!
        monsterScatter = new Monster_Scatter(this);
        //Eaten
        monsterEaten = new Monster_Eaten(this);

        //currentMonsterState = monsterScatter;
        previousMonsterState = null;
    }

    protected virtual void Update()
    {
        if (freezeMotion)
            return;

        target = CalculateTarget(packman);

        if (currentMonsterState == null)
            return;

        currentDebugStateString = currentMonsterState.ToString();
        currentNode = GetMonsterCurrentStandingNode();

        if (currentMonsterState != previousMonsterState || previousMonsterState == null)
        {
            if (previousMonsterState != null)
                previousMonsterState.OnExit(this);

            previousMonsterState = currentMonsterState;
            currentMonsterState.OnEnter(this);
        }

        currentMonsterState.OnExecute(this);
    }

    public Node GetMonsterCurrentStandingNode()
    {
        Debug.DrawRay(raycastFeetPoint.position, -raycastFeetPoint.up);
        RaycastHit hit;
        if(Physics.Raycast(raycastFeetPoint.position, -raycastFeetPoint.up, out hit))
        {
            if(hit.transform.GetComponent<Node>())
            {
                return hit.transform.GetComponent<Node>();
            }
        }

        return null;
    }

    private void OnStateChanged(CurrentGameMode gameMode)
    {
        switch (gameMode)
        {
            case CurrentGameMode.Scatter:
                if (currentMonsterState != monsterEaten)
                    currentMonsterState = monsterScatter;
                break;

            case CurrentGameMode.Frightened:
                currentMonsterState = monsterFrigtened;
                break;

            case CurrentGameMode.Chase:
                if (currentMonsterState != monsterEaten)
                    currentMonsterState = monsterChase;
                break;

            case CurrentGameMode.Eaten:
                currentMonsterState = monsterEaten;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Every enemy in packman calculates target in its own way!!
    /// </summary>
    /// <param name="packman"></param>
    /// <returns></returns>
    protected virtual Vector3 CalculateTarget(PackmanLocomotion packman)
    {
        return packman.transform.position;
    }

    private void OnDestroy()
    {
        MonsterStateController.OnStateChange -= OnStateChanged;
    }
}
