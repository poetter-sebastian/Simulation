﻿using UnityEngine;

namespace World.Agents.Animals.Fox
{
    public class RedFoxes : FaunaAgent
    {
        public RedFoxes()
        {
            domain = "Eukarya";
            kingdom = "Animalia";
            phylum = "Chordata";
            agentClass = "Mammalia";
            order = "Carnivora";
            family = "Canicae";
            genus = "Vulpes";
            species = "Vuples Vulpes";
        }

        public override void Handle()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSee(GameObject obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSeeRadius(GameObject obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSeeRadiusExit(GameObject obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnHear(GameObject obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnHearRadius(GameObject obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnHearRadiusExit(GameObject obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSmell(GameObject obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSmellRadius(GameObject obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSmellRadiusExit(GameObject obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnFoodFood(GameObject obj)
        {
            throw new System.NotImplementedException();
        }
    }
}