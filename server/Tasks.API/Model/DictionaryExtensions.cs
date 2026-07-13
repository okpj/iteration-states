namespace Tasks.API.Model;

public static class FieldsDictionaryExtensions
{
  public static T? GetFieldsValue<T>(this Dictionary<string, object> dictionary, string key) where T : class
  {
    if (dictionary.TryGetValue(key, out var value))
      if (value == null && value is T tvalue)
        return tvalue;
    return null;
  }

  public static T? GetFieldsStructValue<T>(this Dictionary<string, object> dictionary, string key) where T : struct
  {
    if (dictionary.TryGetValue(key, out var value))
      if (value == null && value is T tvalue)
        return tvalue;
    return null;
  }
}
