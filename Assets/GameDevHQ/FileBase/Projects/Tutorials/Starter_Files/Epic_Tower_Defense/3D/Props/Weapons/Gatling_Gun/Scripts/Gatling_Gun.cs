﻿using GameDevHQ.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


namespace GameDevHQ.FileBase.Gatling_Gun
{
    /// <summary>
    /// This script will allow you to view the presentation of the Turret and use it within your project.
    /// Please feel free to extend this script however you'd like. To access this script from another script
    /// (Script Communication using GetComponent) -- You must include the namespace (using statements) at the top. 
    /// "using GameDevHQ.FileBase.Gatling_Gun" without the quotes. 
    /// 
    /// For more, visit GameDevHQ.com
    /// 
    /// @authors
    /// Al Heck
    /// Jonathan Weinberger
    /// </summary>

    [RequireComponent(typeof(AudioSource))] //Require Audio Source component
    public class Gatling_Gun : Tower
    {

        [SerializeField]
        GameObject _turret;

        [SerializeField]
        private Transform _gunBarrel; //Reference to hold the gun barrel
        public GameObject Muzzle_Flash; //reference to the muzzle flash effect to play when firing
        public ParticleSystem bulletCasings; //reference to the bullet casing effect to play when firing
        public AudioClip fireSound; //Reference to the audio clip

        private AudioSource _audioSource; //reference to the audio source component

        [SerializeField]
        int _damageAmount;

        [SerializeField]
        float _damageDelay;
        private bool _startWeaponNoise = true;
        private bool _canDamage = true;

        // Use this for initialization
        void Start()
        {
            Muzzle_Flash.SetActive(false); //setting the initial state of the muzzle flash effect to off
            _audioSource = GetComponent<AudioSource>(); //ssign the Audio Source to the reference variable
            _audioSource.playOnAwake = false; //disabling play on awake
            _audioSource.loop = true; //making sure our sound effect loops
            _audioSource.clip = fireSound; //assign the clip to play
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
                RotateBarrel();
        }

        protected override void AttackTarget(Vector3 targetDirection)
        {
            _turret.transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            //damage target at set times
            if (_canDamage == true)
                StartCoroutine(DamageTarget());

            RotateBarrel(); //Call the rotation function responsible for rotating our gun barrel
            Muzzle_Flash.SetActive(true); //enable muzzle effect particle effect
            bulletCasings.Emit(1); //Emit the bullet casing particle effect  

            if (_startWeaponNoise == true) //checking if we need to start the gun sound
            {
                _audioSource.Play(); //play audio clip attached to audio source
                _startWeaponNoise = false; //set the start weapon noise value to false to prevent calling it again
            }
        }

        IEnumerator DamageTarget ()
        {
            _canDamage = false;
            if (targets.Count > 0)
                targets[0].GetComponent<Enemy>().TakeDamage(_damageAmount);
            yield return new WaitForSeconds(_damageDelay);

            _canDamage = true;
        }

        protected override void StopAttacking()
        {
            Muzzle_Flash.SetActive(false); //turn off muzzle flash particle effect
            _audioSource.Stop(); //stop the sound effect from playing
            _startWeaponNoise = true; //set the start weapon noise value to true
        }

        // Method to rotate gun barrel 
        void RotateBarrel() 
        {
            _gunBarrel.Rotate(Vector3.forward * Time.deltaTime * -500.0f); //rotate the gun barrel along the "forward" (z) axis at 500 meters per second
        }
    }

}
