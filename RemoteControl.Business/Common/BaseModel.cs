namespace RemoteControl.Business.Common
{
    public abstract class BaseModel
    {
        public Guid Id { get; set; }

        private DateTime createdAt;

        private DateTime updatedAt;

        public DateTime CreatedAt
        {
            get => DateTime.SpecifyKind(createdAt, DateTimeKind.Utc);
            set => createdAt = value;
        }

        public DateTime UpdatedAt
        {
            get => DateTime.SpecifyKind(updatedAt, DateTimeKind.Utc);
            set => updatedAt = value;
        }
    }
}
