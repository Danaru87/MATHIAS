namespace mathiasModels
{
    public class MODULES
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string DLL { get; set; }
        public bool ACTIVE { get; private set; }
        public string VERS { get; private set; }
    }
}