using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Crosshair : MonoBehaviour
{

    void Start()
    {
        Image image = GetComponent<Image>();
        image.sprite = IconManager.Instance.GetIcon(IconManager.CROSSHAIR_ICON);
        image.color = Color.black;

    }
}
