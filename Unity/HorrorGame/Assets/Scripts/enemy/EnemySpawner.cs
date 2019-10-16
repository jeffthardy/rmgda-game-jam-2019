using System.Collections;
using UnityEngine;


namespace TopZombies
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemy;
        public bool triggersNightmareMode = false;
        public bool lookAtEnemy = true;
        public float lookTime = 3.0f;

        private bool spawned = false;

        public void SpawnEnemy()
        {
            StartCoroutine(SpawnEnemyRoutine());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!spawned && other.gameObject.GetComponent<FPSController>() != null)
            {
                SpawnEnemy();
            }
        }

        private IEnumerator SpawnEnemyRoutine()
        {
            spawned = true;

            enemy.SetActive(true);

            if (lookAtEnemy)
            {
                var fPSController = GameObject.Find("Player").GetComponent<FPSController>();
                fPSController.InputControl(false);
                fPSController.CameraTarget(enemy);
                yield return new WaitForSeconds(lookTime);
                fPSController.ResetMouseView();
                fPSController.InputControl(true);
            }

            if (triggersNightmareMode)
            {
                var nightmareController = GameObject.Find("NightmareController").GetComponent<NightmareController>();
                nightmareController.SwitchToNightmare();
            }

            var playSeriesOfAudioClips = GetComponent<PlaySeriesOfAudioClips>();
            if (playSeriesOfAudioClips != null)
            {
                playSeriesOfAudioClips.PlaySeries();
            }
        }
    }
}
