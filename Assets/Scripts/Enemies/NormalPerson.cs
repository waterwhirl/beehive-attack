using Bee.Controllers;
using Bee.Enums;
using Bee.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bee.Enemies
{
    public class NormalPerson : PathFinderAi, IEnemy
    {
        [Header("Controllers")]
        private GameController GameController;

        void Awake()
        {
            GameController = GameObject.FindGameObjectWithTag(Tags.GameController)
               .GetComponent<GameController>();
        }

        public Transform[] GetPaths() => PathChosen;

        void OnMouseDown()
        {
            GameController.SetSelectedEnemy(gameObject.GetComponent<NormalPerson>());
        }
    }

}