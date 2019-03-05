using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateStand : KitchenStand
{
    protected override void Init()
    {
        base.Init();
        currentState = KitchenStandState.CONTAIN_PLATE;
    }

    protected override void Execute(Command command)
    {
        base.Execute(command);
        switch(currentState)
        {
            case KitchenStandState.CONTAIN_PLATE:
                if (command == Command.DROP)
                {
                    currentState = KitchenStandState.CONATIN_VEGITABLE; //ready to pick up
                }
                break;

            case KitchenStandState.CONATIN_VEGITABLE:
                if(command == Command.PICKUP)
                {
                    currentState = KitchenStandState.CONTAIN_PLATE;
                }
                break;
        }
    }
}
