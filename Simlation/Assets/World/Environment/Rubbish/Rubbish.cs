using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Playables;
using Utility;
using World.Agents;

public class Rubbish : WorldObject
{
    public PlayerHandler player;

    public override string LN()
    {
        return "Rubbish";
    }

    public override void MouseClick()
    {
        ILog.L(LN, "CASH!");
        player.AddMoney(Random.Range(10, 100));
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
