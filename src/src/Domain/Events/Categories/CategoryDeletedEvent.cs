using Domain.Common;
using Domain.Entities;

namespace Domain.Events.Categories;

public class CategoryDeletedEvent : BaseEvent
{
    public CategoryDeletedEvent(Category item)
    {
        Category = item;
    }

    public Category Category { get; }
}
