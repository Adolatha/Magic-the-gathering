namespace MagicTheGatheringApp.Models
{
    public class IdentifiedItem<T>
    {
        public Guid Id { get; }
        public T Item { get; }

        public IdentifiedItem(T item, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            Item = item;
        }

        public static IdentifiedItem<T> FromPair(KeyValuePair<Guid, T> pair)
        {
            return new IdentifiedItem<T>(pair.Value, pair.Key);
        }
    }
}
