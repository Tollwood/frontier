[System.Serializable]
public class StackItem  {

    public int amount;
    public string name;

    public StackItem(string name, int amount)
    {
        this.name= name;
        this.amount = amount;
    }

    public bool SameItem(StackItem item)
    {
        return item != null && item.name == this.name;
    }
}