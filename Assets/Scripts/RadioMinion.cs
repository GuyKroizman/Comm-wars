using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioMinion : MonoBehaviour {

    public bool commandReceived = true;


    private enum MinionState { CASUAL, CHARGE }
    private MinionState m_minionState;
    public GameObject gotCommand;
    private GameObject gotCommandInstance=null;

    /// <summary>
    /// Hold the state of the rotation.
    /// Does the math using regular degrees and provide the quaternion.
    /// </summary>    
    class Direction
    {
        private float directionDegrees = 0.0f;
        private Quaternion directionQuaternion = Quaternion.identity;


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

    internal void SetCommandRecieved()
    {
        commandReceived = true;

        m_minionState = MinionState.CHARGE;

    }

    class RandomStroll
    {
        
        private float m_timeLeft;
        private Transform m_transform;
        private float m_speed;
        private Direction m_direction;
        
        enum State { WALKING, TURNING }
        State m_state;

        private bool m_isOn;

        public RandomStroll(Transform transform)
        {
            m_isOn = true;

            m_transform = transform;

            ResetStroll();

            m_state = State.WALKING;

            m_direction = new Direction();
;        }

        internal void ResetStroll()
        {
            m_timeLeft = UnityEngine.Random.Range(3, 10);
            m_speed = UnityEngine.Random.Range(3f, 8f);
        }

        internal void Update()
        {
            if(!m_isOn)
            {
                return;
            }


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

        internal void ActCasual()
        {
            m_isOn = true;
        }
    }

    RandomStroll m_randomStroll;

    public GameObject m_explosion;

    public float m_health;

    public float m_chargeSpeed;

    void Start()
    {
        m_randomStroll = new RandomStroll(transform);
        m_minionState = MinionState.CASUAL;
    }

    private void Update()
    {
        m_randomStroll.Update();


        if (m_minionState == MinionState.CHARGE)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * m_chargeSpeed);
        }

        if (commandReceived && gotCommandInstance == null)
        {
            Vector3 currentPos = transform.position;
            currentPos.y += 5f;
            gotCommandInstance = Instantiate(gotCommand, currentPos, Quaternion.Euler(0f, 0f, 0f));
            // bind gotCommand light with parent minion
            gotCommandInstance.transform.SetParent(this.gameObject.transform);

        }
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

        if (m_minionState != MinionState.CASUAL && collision.gameObject.tag == "Boundary")
        {
            // Stop special command and resume strolling
            m_randomStroll.ActCasual();
            m_minionState = MinionState.CASUAL;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Transmission_Charge")
        {
            m_minionState = MinionState.CHARGE;
            Debug.Log("Charging!!!");
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
