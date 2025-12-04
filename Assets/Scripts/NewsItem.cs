#nullable enable
using System;
using System.Globalization;

[Serializable]
public class NewsItem
{
    public string? title;
    public string? content;
    public string? timestamp;
    private DateTime _parsedDate; 
    
    public void Init()
    {
        if (!string.IsNullOrWhiteSpace(timestamp) && DateTime.TryParseExact(timestamp, "yyyy-MM-dd", 
                                                                            CultureInfo.InvariantCulture,
                                                                        DateTimeStyles.None, out var dt))
        {
            _parsedDate = dt;
        }
        else
        {
            _parsedDate = default;
        }
    }
    
    public string SafeTitle => string.IsNullOrWhiteSpace(title) ? "Без заголовка" : title!;
    public string SafeContent => string.IsNullOrWhiteSpace(content) ? "Контент не указан" : content!;
    public DateTime? Date => _parsedDate == default ? null : _parsedDate;
    public string SafeDate => Date == null ? "Дата не указана" : Date.Value.ToString("dd.MM.yyyy");

}

