using UnityEngine;

public class PlantingManager : AbstractPrimaryActionManager
{
   

   protected override void ExecutePrimaryAction()
    {

        //plant select Item if ground is prepared
        Debug.Log("Plant corn");
    }

    protected override Capability GetCapability()
    {
        return Capability.Planting;
    }
}
