using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class NewsLoader
{
    private readonly string _path;
    private readonly Action<string> _onError;

    public NewsLoader(Action<string> onError)
    {
        _onError = onError;
        _path = Path.Combine(Application.streamingAssetsPath, "news.json");
    }

    public async Task<List<NewsItem>> LoadNewsAsync()
    {
        try
        {
            if (!File.Exists(_path))
                throw new FileNotFoundException("Файл news.json не найден");

            var json = await File.ReadAllTextAsync(_path);

            if (string.IsNullOrWhiteSpace(json))
                throw new Exception("JSON пустой");
            
            if (json.TrimStart().StartsWith("["))
                json = "{\"items\":" + json + "}";

            var wrapper = JsonUtility.FromJson<NewsWrapper>(json);

            if (wrapper == null || wrapper.items == null)
                throw new Exception("JSON некорректен");

            foreach (var item in wrapper.items)
                item.Init();
            
            return wrapper.items;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            _onError?.Invoke("Ошибка загрузки: " + ex.Message);
            return new List<NewsItem>();
        }
    }
}