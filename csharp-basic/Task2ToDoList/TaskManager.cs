using Task2ToDoList.TaskClassifiers;

namespace Task2ToDoList;

public class TaskManager
    {
        private readonly List<TaskItem> _tasks = new();

        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                PrintMenu();
                var choice = ReadInt("Ваш выбор: ");

                switch (choice)
                {
                    case 1:
                        AddTask();
                        break;
                    case 2:
                        ShowAllTasks();
                        break;
                    case 3:
                        FilterByStatus();
                        break;
                    case 4:
                        FilterByCategory();
                        break;
                    case 5:
                        MarkAsDone();
                        break;
                    case 6:
                        DeleteByIndex();
                        break;
                    case 0:
                        Console.WriteLine("Выход.");
                        return;
                    default:
                        Console.WriteLine("Неизвестный пункт меню. Повторите ввод.");
                        break;
                }
            }
        }

        private void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Журнал заданий");
            Console.WriteLine("1. Добавить задачу");
            Console.WriteLine("2. Посмотреть все задачи");
            Console.WriteLine("3. Фильтр по статусу");
            Console.WriteLine("4. Фильтр по категории");
            Console.WriteLine("5. Отметить задачу как выполненную");
            Console.WriteLine("6. Удалить задачу");
            Console.WriteLine("0. Выход");
        }

        private void AddTask()
        {
            Console.WriteLine("\n— Добавление задачи —");
            var name = ReadNonEmptyString("Название: ");
            var description = ReadOptionalString("Описание (можно оставить пустым): ");

            var priority = ReadEnumValue<Priority>("Приоритет (Low/Medium/High): ");
            var category = ReadEnumValue<Category>("Категория (Study/Work/Home/Other): ");
            var status = ReadEnumValue<Status>("Статус (New/InProgress/Done). Enter — по умолчанию New: ", 
                true, Status.New);

            _tasks.Add(new TaskItem(name, description, priority, category, status));
            Console.WriteLine("Задача добавлена!");
        }

        private void ShowAllTasks()
        {
            Console.WriteLine("\n— Все задачи —");
            if (_tasks.Count == 0)
            {
                Console.WriteLine("Список пуст.");
                return;
            }

            PrintTasks(_tasks);
        }

        private void FilterByStatus()
        {
            Console.WriteLine("\n— Фильтр по статусу —");
            var status = ReadEnumValue<Status>("Введите статус (New/InProgress/Done): ");
            var filtered = _tasks.Where(t => t.Status == status).ToList();

            if (filtered.Count == 0)
            {
                Console.WriteLine("Нет задач с таким статусом.");
                return;
            }

            PrintTasks(filtered);
        }

        private void FilterByCategory()
        {
            Console.WriteLine("\n— Фильтр по категории —");
            var category = ReadEnumValue<Category>("Введите категорию (Study/Work/Home/Other): ");
            var filtered = _tasks.Where(t => t.Category == category).ToList();

            if (filtered.Count == 0)
            {
                Console.WriteLine("Нет задач в этой категории.");
                return;
            }

            PrintTasks(filtered);
        }

        private void MarkAsDone()
        {
            Console.WriteLine("\n— Отметить задачу как выполненную —");

            if (_tasks.Count == 0)
            {
                Console.WriteLine("Список пуст.");
                return;
            }

            PrintTasks(_tasks);
            var index = ReadInt("Введите номер задачи: ");

            if (!IsValidIndex(index, _tasks.Count))
            {
                Console.WriteLine("Некорректный номер.");
                return;
            }

            _tasks[index - 1].Complete();
            Console.WriteLine("Задача отмечена как выполненная.");
        }

        private void DeleteByIndex()
        {
            Console.WriteLine("\n— Удаление задачи —");

            if (_tasks.Count == 0)
            {
                Console.WriteLine("Список пуст.");
                return;
            }

            PrintTasks(_tasks);
            var index = ReadInt("Введите номер задачи: ");

            if (!IsValidIndex(index, _tasks.Count))
            {
                Console.WriteLine("Некорректный номер.");
                return;
            }

            _tasks.RemoveAt(index - 1);
            Console.WriteLine("Задача удалена.");
        }

        private void PrintTasks(IReadOnlyList<TaskItem> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var t = list[i];
                var desc = t.Description ?? "<нет описания>";
                Console.WriteLine($"{i + 1}. [{t.Status}] {t.Name}");
                Console.WriteLine($"   Категория: {t.Category} | Приоритет: {t.Priority}");
                Console.WriteLine($"   Описание: {desc}");
            }
        }

        private static bool IsValidIndex(int index, int count) => index >= 1 && index <= count;

        private static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("Поле не может быть пустым, попробуйте снова.");
            }
        }

        private static string? ReadOptionalString(string prompt)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? null : input.Trim();
        }

        private static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out var number))
                    return number;

                Console.WriteLine("Введите целое число.");
            }
        }

        private static TEnum ReadEnumValue<TEnum>(string prompt, bool allowEmptyForDefault = false, TEnum? defaultValue = null)
            where TEnum : struct, Enum
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();

                if (allowEmptyForDefault && string.IsNullOrWhiteSpace(input) && defaultValue.HasValue)
                    return defaultValue.Value;

                if (Enum.TryParse<TEnum>(input, true, out var value))
                    return value;

                Console.WriteLine($"Некорректное значение. Допустимые: {string.Join(", ", Enum.GetNames(typeof(TEnum)))}");
            }
        }
    }