﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQ.Scripts;

//namespace GameDevHQ.FileBase.Missle_Launcher
namespace GameDevHQ.Scripts
{
    public class Missle_Launcher : Tower
    {

        [SerializeField]
        private GameObject _missilePrefab; //holds the missle gameobject to clone
        [SerializeField]
        private MissileType _missileType; //type of missle to be launched
        [SerializeField]
        private GameObject[] _misslePositions; //array to hold the rocket positions on the turret
        [SerializeField]
        private float _fireDelay; //fire delay between rockets
        [SerializeField]
        private float _launchSpeed; //initial launch speed of the rocket
        [SerializeField]
        private float _power; //power to apply to the force of the rocket
        [SerializeField]
        private float _fuseDelay; //fuse delay before the rocket launches
        [SerializeField]
        private float _reloadTime; //time in between reloading the rockets
        [SerializeField]
        GameObject _turret; //Part of tower to rotate towards enemy
        private bool _launched; //bool to check if we launched the rockets

        private int _missilePosCount;
        GameObject[] _missilePool;

        [SerializeField]
        private int _missileDamage;

        //TODO: Switch _missilePool to ObjectPool

        public override void OnEnable()
        {
            base.OnEnable();
            _missilePosCount = 6;
            _missilePool = new GameObject[_misslePositions.Length];
        }

        protected override void AttackTarget(Vector3 targetDirection)
        {
            targetDirection.y = 0;

            var newRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            _turret.transform.rotation = Quaternion.Slerp(_turret.transform.rotation, newRotation, Time.deltaTime * _rotateSpeed);

            if (_launched == false)
            {
                _launched = true; //set the launch bool to true
                StartCoroutine(FireRocketsRoutine());
            }
        }

        protected override void StopAttacking()
        {
            
        }

        IEnumerator FireRocketsRoutine()
        {
            MissileLocationCycle();
            LoadRocket(_missilePosCount);        
            yield return new WaitForSeconds(_fireDelay); //wait for the firedelay
            _misslePositions[_missilePosCount].SetActive(true);

            _launched = false; //set launch bool to false
        }

        private void MissileLocationCycle()
        {
            _missilePosCount++;
            if (_missilePosCount >= _misslePositions.Length)
                _missilePosCount = 0;
        }

        private void LoadRocket(int rocketLocation)
        {
            if (_missilePool[rocketLocation] == null)
            {
                _missilePool[rocketLocation] = Instantiate(_missilePrefab) as GameObject; //instantiate a rocket
                _missilePool[rocketLocation].transform.parent = _misslePositions[rocketLocation].transform; //set the rockets parent to the missle launch position
            }
            else
            {
                _missilePool[rocketLocation].transform.position = _misslePositions[rocketLocation].transform.position;
                _missilePool[rocketLocation].SetActive(true);
            }
            _missilePool[rocketLocation].transform.localPosition = Vector3.zero; //set the rocket position values to zero
            _missilePool[rocketLocation].transform.localEulerAngles = new Vector3(-90, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction
            _missilePool[rocketLocation].GetComponent<Missle>().AssignMissleRules(_missileType, targets[0].gameObject.transform, _launchSpeed, _power, _fuseDelay, _missileDamage);

            //_misslePositions[rocketLocation].SetActive(false); //hide missile in place to look like it shoots;
        }
    }
}