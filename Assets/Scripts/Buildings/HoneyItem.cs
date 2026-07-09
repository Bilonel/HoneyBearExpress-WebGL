namespace HoneyBearExpress.Buildings
{
    public class HoneyItem
    {
        public int Id { get; private set; }
        public float Weight { get; private set; }
        public Conveyor CurrentConveyor { get; private set; }
        
        public HoneyItemType Type { get; private set;}

        public HoneyItem(int id, HoneyItemType type = HoneyItemType.Honeycomb, float weight = 1f)
        {
            Id = id;
            Weight = weight;
            Type = type;
        }
        
        public void SetCurrentConveyor(Conveyor conveyor)
        {
            CurrentConveyor = conveyor;
        }
        // Yeni metot: GC üretmeden mevcut objeyi dönüştürür
        public void ChangeType(HoneyItemType newType)
        {
            Type = newType;
        }
    }
}
