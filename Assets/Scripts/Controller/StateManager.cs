using System.Collections.Generic;
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
        public bool isDead;

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
            stats.health = 100;
            healthChangedFlag = true;

            if (isOfflineController)
                offlineActions.Execute(this);

            hashes = new AnimHashes();
        }

        private void FixedUpdate()
        {
            if (isDead)
                return;

            if (currentState != null)
            {
                currentState.FixedTick(this);
            }
        }

        private void Update()
        {
            if (isDead)
                return;

            delta = Time.deltaTime;

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

        public void PlayAnimation(string targetAnim, float fadeTime = .2f)
        {
            anim.CrossFade(targetAnim, fadeTime);
        }

        public void SpawnPlayer(Vector3 spawnPos, Quaternion rot)
        {
            healthChangedFlag = true;
            stats.health = 100;

            mTransform.position = spawnPos;
            mTransform.rotation = rot;

            PlayAnimation("Empty Override");
            isDead = false;
        }

        public void OnHit(StateManager shooter, Weapon w, Vector3 dir, Vector3 pos, Vector3 normal)
        {
            // Spawn blood FX
            GameObject hitParticle = GameManagers.GetObjectPooler().RequestObject("FX_BloodSplat_01");
            Quaternion rot = Quaternion.LookRotation(normal);
            hitParticle.transform.position = pos;
            hitParticle.transform.rotation = rot;

            if (Photon.Pun.PhotonNetwork.IsMasterClient)
            {
                if (!isDead)
                {
                    stats.health -= w.ammoType.damageValue;
                    MultiplayerManager mm = MultiplayerManager.singleton;
                    mm.BroadcastPlayerHealth(photonID, stats.health,shooter.photonID);

                    if(stats.health <= 0)
                    {
                        isDead = true;
                    }
                }
            }

            //if (stats.health <= 0)
            //{
            //    stats.health = 0;
            //    // Raise event for death
            //    if (!isDead)
            //    {
            //        Debug.Log("Dead");
            //        isDead = true;
            //        MultiplayerManager.singleton.BroadcastKillPlayer(photonID, shooter.photonID);
            //    }
            //}

            //healthChangedFlag = true;
        }

        public void KillPlayer()
        {
            //Debug.Log("Kill player");
            isDead = true;
            PlayAnimation("death" + Random.Range(1, 4), .4f);
        }

    }
}
