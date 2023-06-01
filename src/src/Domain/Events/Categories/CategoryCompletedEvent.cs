using Domain.Common;
using Domain.Entities;

namespace Domain.Events.Categories;

public class CategoryCompletedEvent : BaseEvent
{
    public CategoryCompletedEvent(Category item)
    {
        Category = item;
    }

    public Category Category { get; }
}
