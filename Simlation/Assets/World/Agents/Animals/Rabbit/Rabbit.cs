using System;
using System.Collections;
using UnityEngine;
using World.Environment;
using Random = UnityEngine.Random;

namespace World.Agents.Animals.Rabbit
{
    public class Rabbit : FaunaAgent
    {
        public Rabbit()
        {
            domain = "Eukarya";
            kingdom = "Animalia";
            phylum = "Chordata";
            agentClass = "Mammalia";
            order = "Lagomorpha";
            family = "Leporidae";
            genus = "Lepus";
            species = "Lepus europaeus";
            behaviour = new RabbitBehavior();
            maxPossibleSpeed = 5;
            health = 200;
            offspring = 6;
        }

        private int baby = 0;

        private void Start()
        {
            StartCoroutine(BurnEnergy());
        }

        private void Update()
        {
            if (health < 1)
            {
                Destroy(gameObject);
            }
        }

        public override void OnConsumption(object s, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void OnHandle(WorldController world)
        {
            behaviour.Next();
        }

        protected override void OnSee(GameObject obj)
        {
            throw new NotImplementedException();
        }
        
        protected override void OnSeeRadius(GameObject obj)
        {
            throw new NotImplementedException();
        }
        
        protected override void OnSeeRadiusExit(GameObject obj)
        {
            throw new NotImplementedException();
        }

        protected override void OnHear(GameObject obj)
        {
            throw new NotImplementedException();
        }

        protected override void OnHearRadius(GameObject obj)
        {
            throw new NotImplementedException();
        }

        protected override void OnHearRadiusExit(GameObject obj)
        {
            throw new NotImplementedException();
        }

        protected override void OnSmell(GameObject obj)
        {
            if (obj.layer == 11 && !target)
            {
                target = obj.transform;
                nav.SetDestination(obj.transform.position);
            }
        }

        protected override void OnSmellRadius(GameObject obj)
        {
            if (obj.layer == 11 && !target)
            {
                target = obj.transform;
                nav.SetDestination(obj.transform.position);
            }
        }

        protected override void OnSmellRadiusExit(GameObject obj)
        {
            throw new NotImplementedException();
        }

        protected override void OnFoodFood(GameObject obj)
        {
            if (obj.layer == 11 &obj)
            {
                StartCoroutine(Eat(obj.GetComponent<FloraAgent>()));
            }
        }

        private IEnumerator Eat(Agent plant)
        {
            plant.health = 0;
            health = 200;
            baby++;
            yield return null;
            if (baby == 10)
            {
                var rand = Random.Range(0, offspring);
                for (var i = 0; i < rand; i++)
                {
                    Instantiate(GetComponentInParent<Animals>().rabbit, transform.position, transform.rotation,
                        transform.parent);
                }
                baby = 0;
            }
        }
        
        private IEnumerator BurnEnergy()
        {
            while (health > 0)
            {
                yield return new WaitForSeconds(0.1f);
                health -= 1;
            }
        }
        
        public override string LN()
        {
            return "rabbit";
        }
    }
}