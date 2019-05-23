public abstract class Item
{
    protected float Weight;
    protected float Volume;
    protected ItemClass ItemClass;

    protected Item(float weight, float volume, ItemClass itemClass)
    {
        Weight = weight;
        Volume = volume;
        ItemClass = itemClass;
    }
}
