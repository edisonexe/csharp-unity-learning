namespace Task2ToDoList;

public class Task
{
    public string Name { get; }
    public string? Description { get; }
    public Priority Priority { get; }
    public Category Category { get; }
    public Status Status { get; private set; }

    public Task(string name, string description, Priority priority, Category category, Status status)
    {
        Name = name;
        Description = description;
        Priority = priority;
        Category = category;
        Status = status;
    }

    public void Complete() => Status = Status.Done;
}