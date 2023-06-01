using Domain.Common;
using Domain.Entities;

namespace Domain.Events.Categories;

public class CategoryCreatedEvent : BaseEvent
{
    public CategoryCreatedEvent(Category item)
    {
        Category = item;
    }

    public Category Category { get; }
}
