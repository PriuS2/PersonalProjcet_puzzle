using System;
using System.Collections;
using UnityEngine;
//using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyFSM : MonoBehaviour
    {
        public enum EnemyState
        {
            Idle,
            Move,
            Attack,
            AttackDelay,
            Damage,
            Die
        };

        private EnemyState _state = EnemyState.Idle;
        private Transform _thisTransform;


        //private NavMeshAgent _navMeshAgent;

        private GameObject _target;
        private float _attackDelayTimeStack = 0;
        private float _idleTimeStack = 0;
        private float _damagedTimeStack = 0;
        
        public float idleTime = 3;
        public float moveSpeed = 6.0f;
        public float attackDistance = 1.0f;
        public int damage = 5;
        public float attackDelay = 2.0f;


        public int maxHP = 20;
        private int currentHp;

        private CapsuleCollider _myCol;
        public Slider slider;
        
        
        private void Start()
        {
            _thisTransform = transform;
            _state = EnemyState.Idle;
            currentHp = maxHP;
            slider.value = 1;
            _myCol = GetComponent<CapsuleCollider>();
        }


        private void Update()
        {
            if (GameManager.gm.gameState != GameManager.GameState.Run)
            {
                return;
            }
            
            // 현재상태에 따라서 행동할 함수들을 실행한다.
            // print(_state);
            
            switch (_state)
            {
                case EnemyState.Idle:
                {
                    Idle();
                    break;
                }

                case EnemyState.Move:
                {
                    Move();
                    break;
                }
                case EnemyState.Attack:
                {
                    Attack();
                    break;
                }
                case EnemyState.AttackDelay:
                {
                    AttackDelay();
                    break;
                }

                case EnemyState.Damage:
                {
                    // OnDamaged();
                    break;
                }
                case EnemyState.Die:
                {
                    Die();
                    break;
                }
            }


        }


        private void Idle()
        {
            // StartCoroutine(Idle(idleTime));
            _idleTimeStack += Time.deltaTime;
            if (_idleTimeStack > idleTime)
            {
                _idleTimeStack = 0;
                _state = EnemyState.Move;
            }
        }

        private void Move()
        {
            if (!_target)
            {
                _target = GameObject.FindWithTag("Player");
                if (GetComponent<Billboard>())
                {
                    var temp = GetComponent<Billboard>();
                    temp.target = _target.transform;
                    temp.uVector3 = new Vector3(0,-1,0);
                }
            }
            else
            {
                var dir = _target.transform.position - _thisTransform.position;

                var targetRotY = Quaternion.LookRotation(dir.normalized);
                _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation, targetRotY, Time.deltaTime * 10);

                _thisTransform.position += dir.normalized * moveSpeed * Time.deltaTime;


                if (dir.magnitude <= attackDistance)
                {
                    _state = EnemyState.Attack;
                }
            }

            //_navMeshAgent.destination = _target.transform.position;
            //만일 타겟이 없다면 플레이어를 검색하여 타겟으로 지정
            //else 타겟을 향해 이동
        }

        private void Attack()
        {
            var dir = _target.transform.position - _thisTransform.position;

            // if (dir.magnitude <= attackDistance)
            // {
            //     _state = EnemyState.Attack;
            // }
            //대미지 주기
            _target.GetComponent<PlayerMovement>().OnDamaged(damage);
            _state = EnemyState.AttackDelay;
        }

        private void AttackDelay()
        {
            _attackDelayTimeStack += Time.deltaTime;
            if (_attackDelayTimeStack > attackDelay)
            {
                _attackDelayTimeStack = 0;
                // _state = EnemyState.Attack;
                _state = EnemyState.Move;
            }
        }

        public void OnDamaged(int damage)
        {
            currentHp -= damage;
            slider.value = currentHp / (float) maxHP;
            if (currentHp <= 0)
            {
                currentHp = 0;
                _state = EnemyState.Die;
            }
            else
            {
                _damagedTimeStack += Time.deltaTime;

                if (_damagedTimeStack > .5f)
                {
                    _damagedTimeStack = 0;
                    _state = EnemyState.Move;
                }
            }
        }

        private void Die()
        {
            _myCol.enabled = false;
            
            Destroy(gameObject, 3.0f);
            
        }

        private void OnDestroy()
        {
            GameManager.gm.IncrementKillCount();
        }
    }
}