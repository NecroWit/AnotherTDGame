using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AnotherDTGame
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float dieDelay = 0.1f;
        public int currentNPos = 0;

        private EnemySettings _currentSettings;

        private int _currentHealth;
        
        //Visual
        [SerializeField] private HealthBar healthBar;
        

        public void StartMove(Vector3 startPos, EnemySettings enemySettings)
        {
            transform.localPosition = startPos;
            InitSettings(enemySettings);
            MoveToNextPoint();
        }

        public void InitSettings(EnemySettings enemySettings)
        {
            _currentSettings = enemySettings;
        }

        public void Hit(int damage)
        {
            _currentHealth -= damage;
            if (healthBar != null)
            {
                healthBar.ChangeHealth((float)_currentHealth / _currentSettings.health);
            }

            if (_currentHealth <= 0)
            {
                BoardMaster.Instance.RemoveEnemy(this);
                Die();
            }
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        public void MoveToNextPoint()
        {
            if (BoardMaster.Instance.IsInFinalPoint(this))
            {
                Invoke(nameof(Die), dieDelay);
            }

            Vector3 nextPos = BoardMaster.Instance.GetPositionOnPath(currentNPos);

            if (nextPos != Vector3.zero)
                StartCoroutine(StepMove(transform.localPosition, nextPos));

            currentNPos++;
        }

        private IEnumerator StepMove(Vector3 from, Vector3 to)
        {
            float path = (to - from).magnitude;
            float t = 0;
            float step = 0;
            while (t <= 1.0f)
            {

                {
                    step += _currentSettings.speed * Time.deltaTime;
                    t = (step * path) / path;
                    transform.localPosition = Vector3.Lerp(from, to, t);
                    yield return null;
                }
            }

            transform.localPosition = to;
            MoveToNextPoint();
        }
    }
}
