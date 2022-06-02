using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/** Created by Sebatián Jiménez F.
 * Class Enemies.
 */

public enum EnemyType
{
    Slime,
    RangedBot,
    MeleeBot1,
    MeleeBot2
}

public abstract class Enemies : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] public EnemyType enemyType;
    [SerializeField] protected int health;
    [SerializeField] public int atkDmg;
    [SerializeField] protected float speed;

    [Header("Enemy Effects")]
    [SerializeField] protected bool invisible;
    [SerializeField] protected bool regen;
    [SerializeField] protected bool elite;

    public bool boss;
    public bool freezed;
    public bool poisoned;
    private bool alreadyPoisoned;
    [SerializeField] protected float poisonTime;
    [SerializeField] protected float freezeTime;

    [Header("Tests")]
    public bool activateCons;
    public SpriteRenderer sprite;
    public SpriteRenderer spriteShadow;
    public NavMeshAgent enemyAgent; //TEST PUBLIC
    public Animator animEnemyShadow;
    public int resultDice = 0;

    private Fading fade;
    private DiceRoll dices;
    public Transform target;
    protected Animator animEnemy;
    protected bool isAttacking;
    protected bool isDead;
    protected AudioManager audioM;


    private void Awake()
    {
        audioM = AudioManager.Instance;
        target = FindObjectOfType<PlayerController>().transform;
        sprite = GetComponentInChildren<SpriteRenderer>();
        animEnemy = GetComponentInChildren<Animator>();
        enemyAgent = GetComponent<NavMeshAgent>();
        fade = GetComponentInChildren<Fading>();
        dices = FindObjectOfType<DiceRoll>();
    }

    protected virtual void Start()
    {
        if(health <= 0) health = 4;
        if(atkDmg <=0) atkDmg = 1;
        if (speed <= 0) speed = 5F;
        if (freezeTime <= 0) freezeTime = 1.5F;
        if (poisonTime <= 0) poisonTime = 6F;

        enemyAgent.stoppingDistance = 2F;
        enemyAgent.speed = speed;

        int speedR = (int)Mathf.Round(Random.Range(speed - 1, speed + 1));
        speed = speedR;
        enemyAgent.speed = speed;

        enemyAgent.SetDestination(target.position);

    }

    private void Update()
    {
        if (dices.resultUnlucky != 0)
            resultDice = dices.resultUnlucky;
        if (dices.resultUnlucky != 0 && !activateCons)
            EnemyCons();

        if(regen && health < 6)
            StartCoroutine(ActivateRegen());

        if (poisoned && health > 1 && !alreadyPoisoned)
            StartCoroutine(Poisoned());

        if (freezed)
            StartCoroutine(Freezed());


        if (health <= 0 && !isDead)
        {
            animEnemy.Play("Dead");
            StartCoroutine(Dead());
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Movement();

            if (target != null && !isAttacking)
                StartCoroutine(Attack());
        }
    }

    private void LateUpdate()
    {
        Flip();
    }

    #region Actions

    protected virtual void Movement()
    {
        enemyAgent.SetDestination(target.position);
    }

    protected void Flip()
    {
        float cameraY = Camera.main.transform.eulerAngles.y;
        float diferenciaX = target.position.x - this.transform.position.x;
        float diferenciaY = target.position.z - this.transform.position.z;

        Vector2 vector2 = new Vector2(diferenciaX, diferenciaY);
        float angulo = Mathf.Atan2(vector2.y, vector2.x)*Mathf.Rad2Deg;

        bool cuadPrim = cameraY < 45 || cameraY > 315;
        bool cuadSeg = cameraY > 45f && cameraY < 135F;
        bool cuadTerc = cameraY > 135F && cameraY < 225F;
        bool cuadCuart = cameraY > 225F && cameraY < 315F;

        if (cuadPrim)
        {
            //Debug.Log("PRIMER CUAD");
            sprite.flipX = (angulo < 90 && angulo > -90);
            spriteShadow.flipX = (angulo < 90 && angulo > -90);
        }
        else if(cuadSeg)
        {
            //Debug.Log("SEGUN CUAD");
            sprite.flipX = !(angulo > 0 && angulo < 180);
            spriteShadow.flipX = !(angulo > 0 && angulo < 180);
        }
        else if (cuadTerc)
        {
            //Debug.Log("TERCER CUAD");
            sprite.flipX = (angulo > 90 || angulo < -90);
            spriteShadow.flipX = (angulo > 90 || angulo < -90);
        }
        else if (cuadCuart)
        {
            //Debug.Log("CUARTO CUAD");
            sprite.flipX = (angulo > 0 && angulo < 180);
            spriteShadow.flipX = (angulo > 0 && angulo < 180);
        }
    }

    protected abstract IEnumerator Attack();


    public void LoseHealth(int dmg)
    {
        health-= dmg;
        audioM.PlayOneShot("DMG");
    }

    private IEnumerator Dead()
    {
        audioM.PlayOneAtTime("Dead");
        isDead= true;
        enemyAgent.isStopped = true;
        yield return new WaitForSeconds(2.5F);
        gameObject.SetActive(false);
        FindObjectOfType<Exit>().isEnd = true;
        Destroy(gameObject, 1F);
    }
    #endregion


    #region EnemyEffects
    private void EnemyCons()
    {
        activateCons = true;

        switch (resultDice)
        {
            case 1:
                atkDmg += 1;
                break;
            case 2:
                ActivateEliteMode();
                break;
            case 3:
                speed += 1;
                enemyAgent.speed = speed;
                break;
            case 4:
                regen = true;
                break;
            case 5:
                ActivateInvisible();
                break;
            case 6:
                health += 1;
                break;
            default:
                Debug.LogError("This result isnt possible.");
                break;
        }
        if (boss && !elite)
            ActivateEliteMode();
    }

    private void ActivateEliteMode()
    {
        elite = true;
        regen = true;

        atkDmg += 1;
        health += 1;
        speed += 2;
        enemyAgent.speed = speed;

        ActivateInvisible();

        if (boss)
            transform.localScale = new Vector3(2f, 2f, 2f);
    }

    private void ActivateInvisible()
    {
        invisible = true;
        fade.fadeOutIn = invisible;
    }

    private IEnumerator ActivateRegen()
    {
        regen = false;
        yield return new WaitForSeconds(10F);
        health += 1;
        regen = true;
    }
    private IEnumerator Poisoned()
    {
        alreadyPoisoned = true;
        poisoned = false;
        yield return new WaitForSeconds(poisonTime);
        animEnemy.Play("Poisoned");
        health -= 1;
        yield return new WaitForSeconds(poisonTime);
        animEnemy.Play("Poisoned");
        health -= 1;

        alreadyPoisoned = false;
    }

    private IEnumerator Freezed()
    {
        freezed = false;
        animEnemy.Play("Freezed");

        enemyAgent.isStopped = true;
        animEnemyShadow.enabled = false;

        yield return new WaitForSeconds(freezeTime);

        enemyAgent.isStopped = false;
        animEnemy.enabled = true;

    }
    #endregion
}
