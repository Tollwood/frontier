using System.Collections.Generic;
using System.Reflection;
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
    public static string SEED_ICON = "Icons/sesame";
    public static string HAMMER_ICON = "Icons/stake_hammer";
    public static string REVOLVER_ICON = "Icons/revolver";
    public static string SHOVEL_ICON = "Icons/shovel";
    public static string WOOD_ICON = "Icons/wood_pile";
    public static string STONE_ICON = "Icons/stone_pile";


    private void Awake()
    {
        icons.Add(INVENTORY_ICON, Resources.Load<Sprite>(INVENTORY_ICON));
        icons.Add(CLOSE_ICON, Resources.Load<Sprite>(CLOSE_ICON));
        icons.Add(DELETE_ICON, Resources.Load<Sprite>(DELETE_ICON));
        icons.Add(CART_ICON, Resources.Load<Sprite>(CART_ICON));
        icons.Add(BUILDING_ICON, Resources.Load<Sprite>(BUILDING_ICON));
        icons.Add(CROSSHAIR_ICON, Resources.Load<Sprite>(CROSSHAIR_ICON));
        icons.Add(AXE_INVENTORY_ICON, Resources.Load<Sprite>(AXE_INVENTORY_ICON));
        icons.Add(SEED_ICON, Resources.Load<Sprite>(SEED_ICON));
        icons.Add(HAMMER_ICON, Resources.Load<Sprite>(HAMMER_ICON));
        icons.Add(REVOLVER_ICON, Resources.Load<Sprite>(REVOLVER_ICON));
        icons.Add(SHOVEL_ICON, Resources.Load<Sprite>(SHOVEL_ICON));
        icons.Add(WOOD_ICON, Resources.Load<Sprite>(WOOD_ICON));
        icons.Add(STONE_ICON, Resources.Load<Sprite>(STONE_ICON));
    }

    public Sprite GetIcon(string icon)
    {
        icons.TryGetValue(icon, out Sprite sprite);
        return sprite;
    }

    public Sprite GetIconByFieldName(string fieldName)
    {
        var field = typeof(IconManager).GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
        var value = (string)field.GetValue(null);

        icons.TryGetValue(value, out Sprite sprite);
        return sprite;

    }
}
