namespace TakLib
{
    public class Player
    {
        public string Name { get; set; }
        public virtual bool IsAI => false;
    }
}