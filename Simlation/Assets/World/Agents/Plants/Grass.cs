using System;
using System.Collections;
using UnityEngine;
using World.Agents;
using World.Structure;
using Random = UnityEngine.Random;

public class Grass : FloraAgent
{
    public Grass(Ground ground) : base(ground)
    {
        
    }
    
    private void Start()
    {
        StartCoroutine(SpawnGrass());
    }

    private IEnumerator SpawnGrass()
    {
        while (health > 0)
        {
            yield return new WaitForSeconds(Random.Range(15, 60));
            World.Environment.WorldController.Instance.Spawn(gameObject);
        }
    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }


    public override void OnDeath(object s, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public override void OnConsumption(object s, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public override void OnHandle()
    {
        throw new System.NotImplementedException();
    }
}
