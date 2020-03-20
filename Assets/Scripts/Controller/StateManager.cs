﻿using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class StateManager : MonoBehaviour, IHittable
    {
        public int photonID;

        public MovementValues movementValues;
        public PlayerStats stats;
        public Inventory inventory;

        [HideInInspector] public List<Rigidbody> ragdollRBs = new List<Rigidbody>();
        [HideInInspector] public List<Collider> ragdollCols = new List<Collider>();

        [System.Serializable]
        public struct MovementValues
        {
            public float horizontal;
            public float vertical;
            public float moveAmount;

            public Vector3 moveDirection;
            public Vector3 lookDirection;
            public Vector3 aimPosition;
        }

        public bool isLocal;
        public bool isAiming;
        public bool isInteracting;
        public bool isShooting;
        public bool isCrouching;
        public bool isReloading;
        public bool isVaulting;
        public bool isGrounded;

        public bool shootingFlag;
        public bool reloadingFlag;
        public bool vaultingFlag;
        public bool healthChangedFlag;

        public void SetCrouching()
        {
            isCrouching = !isCrouching;
        }

        public void SetReloading()
        {
            isReloading = true;
        }

        public State currentState;

        [HideInInspector]
        public Animator anim;

        [HideInInspector]
        public float delta;
        [HideInInspector]
        public Transform mTransform;
        [HideInInspector]
        public Rigidbody rigid;
        [HideInInspector]
        public LayerMask ignoreLayers;
        [HideInInspector]
        public AnimatorHook animHook;

        [Header("Vaulting Options")]
        public VaultData vaultData;
        public AnimHashes hashes;

        [Header("Ballistics")]
        public Ballistics ballisticsAction;

        public MultiplayerListener multiplayerListener;

        public bool isOfflineController;
        public StateActions offlineActions;

        private void Start()
        {
            mTransform = transform;
            rigid = GetComponent<Rigidbody>();

            if (isOfflineController)
                offlineActions.Execute(this);

            hashes = new AnimHashes();
        }

        private void FixedUpdate()
        {
            delta = Time.deltaTime;

            if (currentState != null)
            {
                currentState.FixedTick(this);
            }
        }

        private void Update()
        {
            if (currentState != null)
            {
                currentState.Tick(this);
            }
        }

        public void SetCurrentState(State targetState)
        {
            if (currentState)
                currentState.OnExit(this);

            currentState = targetState;

            currentState.OnEnter(this);
        }

        public void PlayAnimation(string targetAnim)
        {
            anim.CrossFade(targetAnim, .2f);
        }

        public void OnHit(StateManager shooter, Weapon w, Vector3 dir, Vector3 pos, Vector3 normal)
        {
            // Spawn blood FX
            GameObject hitParticle = GameManagers.GetObjectPooler().RequestObject("FX_BloodSplat_01");
            Quaternion rot = Quaternion.LookRotation(normal);
            hitParticle.transform.position = pos;
            hitParticle.transform.rotation = rot;

            stats.health -= w.ammoType.damageValue;
            if (stats.health <= 0)
            {
                stats.health = 0;
                // Raise event for death
            }

            healthChangedFlag = true;
        }
    }
}
