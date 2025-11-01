using Task2ToDoList.TaskClassifiers;

namespace Task2ToDoList;

public class TaskItem
{
    public string Name { get; }
    public string? Description { get; }
    public Priority Priority { get; }
    public Category Category { get; }
    public Status Status { get; private set; }

    public TaskItem(string name, string? description, Priority priority, Category category, Status status)
    {
        Name = name;
        Description = string.IsNullOrWhiteSpace(description) ? null : description;
        Priority = priority;
        Category = category;
        Status = status;
    }

    public void Complete() => Status = Status.Done;
}