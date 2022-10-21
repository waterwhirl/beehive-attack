using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bee.Enemies;
using Bee.Enums;
using Bee.Interfaces;
using Bee.Spawners;
using UnityEngine;

namespace Bee.Controllers
{
    public class EnemiesController : MonoBehaviour
    {
        [Header("Enemies quantity control")]
        [SerializeField]
        private int QuantityOfNormalEnemies = 5;

        [SerializeField]
        private int NormalEnemiesSpawned = 0;

        [SerializeField]
        private int QuantityOfFakeEnemies = 0;

        [SerializeField]
        private int FakeEnemiesSpawned = 0;

        private int TotalNumberOfEnemies
        {
            get
            {
                return QuantityOfNormalEnemies + QuantityOfFakeEnemies;
            }
        }

        private int TotalNumberOfSpawnedEnemies
        {
            get
            {
                return NormalEnemiesSpawned + FakeEnemiesSpawned;
            }
        }

        [Header("Spawn")]
        [SerializeField]
        private float TimeToAwaitToSpawn = 3;

        [SerializeField]
        private GameObject AllEnemiesSpawner;

        [SerializeField]
        private GameObject EnemiesParent;

        [SerializeField]
        private Transform PositionToCreate;

        /// <summary>
        /// Define the enemies spawner, currently it will be an EnemiesSpawner, but can be another
        /// type of the enemies that implements ISpawner
        /// </summary>
        private ISpawner EnemiesSpawner;

        [SerializeField]
        private List<GameObject> EnemiesCreated;

        [Header("Controllers")]
        [SerializeField]
        private GameController GameController;
        
        private PunctuationController PunctuationController;

        void Awake()
        {
            EnemiesCreated = new List<GameObject>();
            EnemiesSpawner = AllEnemiesSpawner.GetComponent<EnemiesSpawner>();
            PunctuationController = GameObject.FindGameObjectWithTag(Tags.PunctuationController)
                .GetComponent<PunctuationController>();
            GameController = GameObject.FindGameObjectWithTag(Tags.GameController)
                .GetComponent<GameController>();
        }

        void Start()
        {
            PunctuationController.SetQuantityOfBeesByEnemies(QuantityOfNormalEnemies);
            StartCoroutine(CreateEnemies());
        }

        void Update()
        {
            CleanDeadEnemies();

            if (!AllEnemiesHaveDied())
                return;

            GameController.NextLevel();
            StartCoroutine(CreateEnemies());
        }

        private void CleanDeadEnemies()
        {
            EnemiesCreated = EnemiesCreated.Where(x => x != null).ToList();
        }

        public bool AllEnemiesHaveDied()
        {
            // Not all enemies were spawned yet
            if (NormalEnemiesSpawned < TotalNumberOfEnemies)
                return false;

            return EnemiesCreated.Count == 0;
        }

        /// <summary>
        /// Mechanics that have to happen when the player go to the next phase
        /// </summary>
        public void OnNextLevel()
        {
            NormalEnemiesSpawned = 0;
            QuantityOfNormalEnemies++;
            QuantityOfFakeEnemies++;
            TimeToAwaitToSpawn = TimeToAwaitToSpawn - (TimeToAwaitToSpawn * 0.05f);

            PunctuationController.SetQuantityOfBeesByEnemies(QuantityOfNormalEnemies);
        }

        public IEnumerator CreateEnemies()
        {
            var spawnFake = Random.Range(0, 2) == 1;

            var createdEnemy = EnemiesSpawner.Spawn(PositionToCreate, EnemiesParent.transform);

            if (spawnFake && FakeEnemiesSpawned < QuantityOfFakeEnemies)
            {
                var pathFinder = createdEnemy.GetComponent<PathFinderAi>();
                pathFinder.SetAsFakeEnemy();

                FakeEnemiesSpawned++;
            }
            else if (NormalEnemiesSpawned < QuantityOfNormalEnemies)
            {
                EnemiesCreated.Add(createdEnemy);
                NormalEnemiesSpawned++;
            }

            yield return new WaitForSeconds(TimeToAwaitToSpawn);

            if (TotalNumberOfSpawnedEnemies < TotalNumberOfEnemies)
            {
                StartCoroutine(CreateEnemies());
                yield return null;
            }

            yield return null;
        }
    }
}
