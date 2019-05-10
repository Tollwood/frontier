using UnityEngine;

public class RadialMenuSpawner : MonoBehaviour
{
 
   public static RadialMenuSpawner ins;

    public RadialMenu radialMenuPrefab;


    private void Awake()
    {
        ins = this;
    }

    public void SpawnMenu(MenuAction[] options)
    {
        RadialMenu radialMenu = Instantiate(radialMenuPrefab,this.transform,false);
        radialMenu.Create(options);
        radialMenu.transform.position = Input.mousePosition;
    }
}
