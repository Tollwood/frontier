using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class InteractableBuild : Interactable
{
    public Material disolveMaterial;
    public int numOfIncrement = 10;

    private Material origMaterial;
    private float currentY, max, min, increment;
    private Renderer ren;

    private Renderer[] childRends;
    private Material[] childMats;

    public override string hint()
    {
        return "Build house";
    }

    public override void Interact()
    {
        if(currentY < max)
        {
            currentY += increment;
            SetDisolveY();
        }
        else
        {
            ren.material = origMaterial;

            childRends = GetComponentsInChildren<Renderer>();

            for (int i = 0; i < childRends.Length; i++)
            {
                childRends[i].material = childMats[i];
            }
            GetComponent<MeshCollider>().convex = false;
            Destroy(this);
        }
    }

    private void Start()
    {
        ren = GetComponent<Renderer>();
    }

    public void Reset()
    {
        if (ren == null)
        {
            ren = GetComponent<Renderer>();
        }

        min = ren.bounds.min.y;
        max = ren.bounds.max.y;

        childRends = GetComponentsInChildren<Renderer>();
        childMats = new Material[childRends.Length];

        for(int i =0; i < childRends.Length; i ++)
        {
            Renderer childRen = childRends[i];
            if (childRen.bounds.min.y < min)
            {
                min = childRen.bounds.min.y;
            }
            if (childRen.bounds.max.y > max)
            {
                max = childRen.bounds.max.y;
            }

            childMats[i] = childRen.material;
            childRen.material = disolveMaterial;
            childRen.material.mainTexture = childMats[i].mainTexture;
        }

        currentY = min + .2f;
        SetDisolveY();
        increment = (max - min) / numOfIncrement;
        GetComponent<MeshCollider>().convex = true;
    }

    private void SetDisolveY()
    {
        foreach (Renderer childRen in childRends)
        {
            childRen.material.SetFloat("_DisolveY", currentY);
        }
    }
}
