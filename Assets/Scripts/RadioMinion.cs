using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioMinion : MonoBehaviour {

    /// <summary>
    /// Hold the state of the rotation.
    /// Does the math using regular degrees and provide the quaternion.
    /// </summary>    
    class Direction
    {
        private float directionDegrees = 0.0f;
        private Quaternion directionQuaternion = Quaternion.identity;
        public bool commandReceived = true;

        internal void TurnRight()
        {
            directionDegrees += 90.0f;
            directionQuaternion = Quaternion.Euler(0.0f, directionDegrees, 0.0f);
        }

        internal void TurnLeft()
        {
            directionDegrees -= 90.0f;
            directionQuaternion = Quaternion.Euler(0.0f, directionDegrees, 0.0f);
        }

        internal void TurnRandom()
        {
            directionDegrees -= UnityEngine.Random.Range(0f, 360f);
            directionQuaternion = Quaternion.Euler(0.0f, directionDegrees, 0.0f);
        }

        internal Quaternion getDirection()
        {
            return directionQuaternion;
        }

        internal void Turn()
        {
            directionDegrees *= -1;
            directionQuaternion = Quaternion.Euler(0.0f, directionDegrees, 0.0f);
        }
    }


    class RandomStroll
    {
        
        private float m_timeLeft;
        private Transform m_transform;
        private float m_speed;
        private Direction m_direction;
        
        enum State { WALKING, TURNING }
        State m_state;

        public RandomStroll(Transform transform)
        {
            m_transform = transform;

            ResetStroll();

            m_state = State.WALKING;

            m_direction = new Direction();
;        }

        internal void ResetStroll()
        {
            m_timeLeft = UnityEngine.Random.Range(3, 10);
            m_speed = UnityEngine.Random.Range(2f, 6f);
        }

        internal void Update()
        {
            m_timeLeft -= Time.deltaTime;

            if (m_timeLeft > 0)
            {
                if(m_state == State.WALKING)
                    MoveStep();
                else
                {
                    RotateToDestination(m_direction.getDirection(), 15);         
                }
            }
            else
            {
                if(m_state == State.WALKING)
                {
                    m_state = State.TURNING;
                    m_direction.TurnRandom();
                    // do something
                }
                else
                {
                    m_state = State.WALKING;
                    
                }
                ResetStroll();
            }            
        }

        /// <summary>
        /// Rotate the current player in the direction provided, using the speed provided
        /// </summary>
        /// <param name="rotateDestinationQuaternion">The direction to turn to</param>
        /// <param name="turnSpeed">The turning speed</param>
        private void RotateToDestination(Quaternion rotateDestinationQuaternion, float turnSpeed)
        {
            m_transform.rotation = Quaternion.RotateTowards(m_transform.rotation, rotateDestinationQuaternion, turnSpeed * Time.deltaTime);
        }

        private void MoveStep()
        {
            m_transform.Translate(Vector3.forward * Time.deltaTime * m_speed);
        }

        internal void ChangeDirection()
        {
            m_direction.Turn();
        }
    }

    RandomStroll m_randomStroll;

    public GameObject m_explosion;

    public float m_health;

    void Start()
    {
        m_randomStroll = new RandomStroll(transform);
    }

    void Update()
    {
        m_randomStroll.Update();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(tag == "RadioMinion" && collision.gameObject.tag == "TvMinion")
        {
            Explode(collision);

        }

        if (tag == "TvMinion" && collision.gameObject.tag == "RadioMinion")
        {
            Explode(collision);
        }
    }

    private void ExplodeGameObject(GameObject gameObject, GameObject explosion, float explotionScale)
    {
        GameObject thisExposion = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);

        thisExposion.transform.localScale = new Vector3(explotionScale, explotionScale, explotionScale);

        Destroy(gameObject);
    }

    private void Explode(Collision collision)
    {
        ExplodeGameObject(collision.gameObject, m_explosion, 4);
        AreaDamageEnemies(collision.gameObject.transform.position, 20);

        ExplodeGameObject(gameObject, m_explosion, 4);
        AreaDamageEnemies(gameObject.transform.position, 20);

        Destroy(gameObject);
        Destroy(collision.gameObject);
    }

    void AreaDamageEnemies(Vector3 location, float radius)
    {
        Collider[] objectsInRange = Physics.OverlapSphere(location, radius);
        foreach (Collider col in objectsInRange)
        {
            RadioMinion enemy = col.GetComponent<RadioMinion>();
            if (enemy != null)
            {
                // linear falloff of effect
                float proximity = (location - enemy.transform.position).magnitude;                
                float effect = 100 - (proximity * 15);

                if (effect <= 0)
                    continue;

                enemy.ApplyDamage(effect);
            }
        }
    }

    private void ApplyDamage(float v)
    {
        m_health -= v;

        if(m_health < 0)
        {
            ExplodeGameObject(gameObject, m_explosion, 3);
        }
    }
}
