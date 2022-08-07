using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using World.Agents;
using Random = UnityEngine.Random;

public class Grass : FloraAgent
{
    private void Start()
    {
        StartCoroutine(SpawnGrass());
    }

    private IEnumerator SpawnGrass()
    {
        while (health > 0)
        {
            yield return new WaitForSeconds(Random.Range(15, 60));
            World.Environment.World.Instance.Spawn(gameObject);
        }
    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void Handle()
    {
        throw new System.NotImplementedException();
    }
}
