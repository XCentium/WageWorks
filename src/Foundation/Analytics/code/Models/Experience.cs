namespace Wageworks.Foundation.Analytics.Models
{
    public class Experience
    {
        public Experience()
        {
            this.Contact = new Contact();
            this.Interaction = new Interaction();
        }

        public Contact Contact { get; set; }
        public Interaction Interaction { get; set; }
    }
}