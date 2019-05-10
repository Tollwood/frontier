using System;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    public RadialButton radialMenuButtonPrefab;
    public RadialButton selected = null;
    private float radius = 80f;

    internal void Create(MenuAction[] options)
    {

        for(int i = 0; i < options.Length; i++)
        {
            RadialButton newButton = Instantiate(radialMenuButtonPrefab, transform, false);

            double theta = (2 * Math.PI / options.Length) * i;
            float xPos = (float)Math.Sin(theta);
            float yPos = (float)Math.Cos(theta);
            newButton.transform.localPosition = new Vector3(xPos, yPos, 0) * radius;

            newButton.circle.color = Color.gray;
            newButton.icon.sprite = options[i].sprite;
            newButton.title = options[i].hint;
            newButton.parent = this;
            newButton.interaction = options[i].interaction;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            EventManager.TriggerEvent(Events.OnCloseMenu);
            if (selected != null)
            {
                selected.interaction();
            }
            Destroy(gameObject);
        }
    }

}