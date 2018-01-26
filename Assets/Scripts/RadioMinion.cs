using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioMinion : MonoBehaviour {

    public float speed;


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
        //m_randomStroll.ChangeDirection();
    }
}
