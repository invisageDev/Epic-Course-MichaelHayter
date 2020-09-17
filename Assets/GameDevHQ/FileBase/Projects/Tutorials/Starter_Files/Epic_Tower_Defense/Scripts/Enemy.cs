﻿using UnityEngine;
using UnityEngine.AI;
using GameDevHQ.Scripts.Managers;
using System.Collections;

namespace GameDevHQ.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class Enemy : MonoBehaviour, IHealth
    {
        [SerializeField]
        private float _speed;

        [SerializeField]
        private int _warfund;

        [SerializeField]
        private int _health;

        [SerializeField]
        ParticleSystem _damageFX;

        [SerializeField]
        ParticleSystem _deathFX;

        [SerializeField]
        public int Health { get; set; }

        [SerializeField]
        Animator _animator;

        [SerializeField]
        float _cleanUpDelay;

        private Transform _target;
        private NavMeshAgent _agent;

        private int _ID;
        private bool _isAlive = true;
        private int _currentHealth;

        private void OnEnable()
        {
            //reset mech
            _isAlive = true;
            //_animator.SetTrigger("Alive");
            _animator.SetBool("IsAlive", true);
            _currentHealth = _health;

            //get target
            _target = GameManger.Instance.RequestTarget();

            //move to target
            _agent = GetComponent<NavMeshAgent>();
            if (_agent != null)
            {
                _agent.speed = _speed;
                _agent.SetDestination(_target.position);
            }
        }

        private void Start()
        {
            //set ID when instantiated
            _ID = SpawnManager.Instance.GetNextID();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T)) //for debugging to remove
                TakeDamage(25);
        }

        public void TakeDamage(int damage)
        {
            //take damage
            print("taking damage");
            _currentHealth -= damage;
            //play damage FX
            if (_damageFX != null)
                _damageFX.Play();
            //check to see if health is zero or less
            if (_currentHealth <= 0)
                Destroyed();
        }

        private void Destroyed ()
        {
            //if destroyed play death FX
            if (_deathFX != null && _isAlive)
            {
                _deathFX.Play();
                _isAlive = false;
                //stop movement
                _agent.isStopped = true;
                //play death animation
                //_animator.SetTrigger("Death");
                _animator.SetBool("IsAlive", false);
                //and disable after x seconds
                StartCoroutine(Disabled());
            }
        }

        IEnumerator Disabled()
        {
            yield return new WaitForSeconds(_cleanUpDelay);
            this.gameObject.SetActive(false);
        }

        public int GetID ()
        {
            return _ID;
        }

        public bool IsAlive ()
        {
            return _isAlive;
        }    
    }
}