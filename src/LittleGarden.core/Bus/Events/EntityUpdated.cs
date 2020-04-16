using LittleGarden.Core.Entities;

namespace LittleGarden.Core.Bus.Events
{
    public abstract class EntityUpdated : Event
    {
        protected EntityUpdated(Entity entity)
        {
            Entity = entity;
        }

        public Entity Entity { get; set; }
    }

    public class EntityUpdated<T> : EntityUpdated
        where T : Entity
    {
        public EntityUpdated(Entity entity) : base(entity)
        {
        }

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public T Entity => (T) base.Entity;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    }
}