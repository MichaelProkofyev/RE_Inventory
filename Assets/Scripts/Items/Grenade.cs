using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Item {
    public override ItemRect UseRange
    {
        get
        {
            return new ItemRect(Position, 3, 3);
        }
    }

    public override void Use()
    {
        print("BOOM");
    }
}
