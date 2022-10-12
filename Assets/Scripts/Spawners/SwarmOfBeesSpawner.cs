using Bee.Defenses;
using Bee.Enums;
using Bee.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bee.Spawners
{
    public class SwarmOfBeesSpawner : MonoBehaviour, ISpawner
    {
        [SerializeField]
        private GameObject SwarmOfBees;

        public void Spawn()
        {
            Instantiate(SwarmOfBees, position: new Vector3(), rotation: SwarmOfBees.transform.rotation);
        }

        public void Spawn(Transform transform)
        {
            Instantiate(SwarmOfBees, position: transform.position, rotation: SwarmOfBees.transform.rotation);
        }

        public void Spawn(GameObject target)
        {
            var createdSwarm = Instantiate(
                SwarmOfBees,
                position: new Vector3(),
                rotation: SwarmOfBees.transform.rotation
            );

            createdSwarm.GetComponent<SwarmOfBees>().SetEnemyToAttack(target);
        }
    }
}