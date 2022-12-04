using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine.Playables;
using Utility;
using World.Agents;
using Random = UnityEngine.Random;

public class Rubbish : WorldObject
{
    public PlayerHandler player;

    public void Start()
    {
        player.AddRubbish();
    }

    public override string LN()
    {
        return "Rubbish";
    }

    public override void MouseClick()
    {
        player.ui.PlayMetal();
        ILog.L(LN, "CASH!");
        player.AddMoney(Random.Range(50, 100));
        player.RemoveRubbish();
        Destroy(gameObject);
    }
        
    public override void MouseOver()
    {
        throw new System.NotImplementedException();
    }

    public override void MouseExit()
    {
        throw new System.NotImplementedException();
    }
}
