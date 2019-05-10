using System.Collections.Generic;
using UnityEngine;

public class IconManager : Singleton<IconManager>
{

    public Dictionary<string, Sprite> icons = new Dictionary<string, Sprite>();
    public static string INVENTORY_ICON = "Icons/inventory";
    public static string CLOSE_ICON = "Icons/close";
    public static string DELETE_ICON = "Icons/delete";
    public static string CART_ICON = "Icons/cart";
    public static string BUILDING_ICON = "Icons/building";
    public static string CROSSHAIR_ICON = "Icons/crosshair";
    public static string AXE_INVENTORY_ICON = "Icons/axe_inventory";

    private void Awake()
    {
        icons.Add(INVENTORY_ICON, Resources.Load<Sprite>(INVENTORY_ICON));
        icons.Add(CLOSE_ICON, Resources.Load<Sprite>(CLOSE_ICON));
        icons.Add(DELETE_ICON, Resources.Load<Sprite>(DELETE_ICON));
        icons.Add(CART_ICON, Resources.Load<Sprite>(CART_ICON));
        icons.Add(BUILDING_ICON, Resources.Load<Sprite>(BUILDING_ICON));
        icons.Add(CROSSHAIR_ICON, Resources.Load<Sprite>(CROSSHAIR_ICON));
        icons.Add(AXE_INVENTORY_ICON, Resources.Load<Sprite>(AXE_INVENTORY_ICON));
    }

    public Sprite GetIcon(string icon)
    {
        icons.TryGetValue(icon, out Sprite sprite);
        return sprite;
    }
}
