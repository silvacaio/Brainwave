namespace Brainwave.ManagementPayment.Business
{
    public class Order
    {
        public Guid CourseId { get; set; }
        public Guid StudentId { get; set; }
        public decimal Value { get; set; }
    }
}
