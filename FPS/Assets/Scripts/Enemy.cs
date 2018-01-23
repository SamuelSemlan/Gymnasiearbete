using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public enum State { Idle, Chasing, Attacking };
    State currentState;

    public ParticleSystem deathEffect;

    NavMeshAgent pathfinder;
    Transform target;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    bool hasTarget;
	bool dead = false;

    public float damage = 1;
	public float health;

    void Start()
    {
        pathfinder = GetComponent<NavMeshAgent>();


        if (GameObject.FindGameObjectWithTag("Chased") != null)
        {
            currentState = State.Chasing;
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("Chased").transform;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());
        }
    }

	public void TakeDamage(float amount){
		health -= amount;
		if(health <= 0f && !dead){
			Die();
		}
    }

	void Die(){
		Destroy(gameObject);
		dead = true;
	}

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    void Update(){
        if (hasTarget){
            if (Time.time > nextAttackTime){
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2)){
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }

    }

    IEnumerator Attack()
    {
        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0;

        bool hasAppliedDamage = false;

        while (percent <= 1)
        {

            if (percent >= .5f && !hasAppliedDamage){
				ApplyDamage();
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

	void ApplyDamage(){

	}
}