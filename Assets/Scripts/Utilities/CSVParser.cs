using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public static class CSVParser
{
    public static string DictionaryToTable(this List<Dictionary<string, string>> dictList, string pathToSave, int skipLineHeaders = 1)
    {
        System.Text.Encoding encode = Encoding.Unicode;
        List<string> lines = new List<string>();
        if (dictList.Count == 0)
        {
            return string.Empty;
        }
        // Header
        lines.Add(string.Join("\t", dictList[0].Keys));
        skipLineHeaders--;
        while (skipLineHeaders > 0)
        {
            lines.Add("");
            skipLineHeaders--;
        }
        foreach (var dict in dictList)
        {
            List<string> values = new List<string>();
            foreach (var key in dict.Keys)
            {
                values.Add(dict[key]);
            }
            lines.Add(string.Join("\t", values));
        }

        string result = string.Join("\n", lines);
        System.IO.File.WriteAllText(pathToSave, result, encode);
        return result;
    }

    public static string ToTable<T>(this IEnumerable<T> source, string pathToSave, char delimiter = '|', int skipLineHeaders = 1)
    {
        //System.Text.Encoding encode = new UTF8Encoding(false);
        System.Text.Encoding encode = Encoding.Unicode;
        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);

        List<string> lines = new List<string>();

        // Header
        lines.Add(string.Join("\t", fields.Select(f => f.Name)));
        skipLineHeaders--;
        while(skipLineHeaders > 0)
        {
            lines.Add("");
            skipLineHeaders--;
        }

        foreach (var item in source)
        {
            List<string> values = new List<string>();

            foreach (var field in fields)
            {
                var value = field.GetValue(item);
                if (value is int[] intArray)
                {
                    values.Add(string.Join(delimiter.ToString(), intArray));
                }
                else if (value is float[] floatArray)
                {
                    values.Add(string.Join(delimiter.ToString(), floatArray));
                }
                else if (value is string[] stringArray)
                {
                    values.Add(string.Join(delimiter.ToString(), stringArray));
                }
                else
                {
                    values.Add(value?.ToString() ?? string.Empty);
                }
            }

            lines.Add(string.Join("\t", values));
        }

        string result = string.Join("\n", lines);
        System.IO.File.WriteAllText(pathToSave, result, encode);
        return result;
    }
    public static List<T> FromTable<T>(string textContent, char delimiter = '|', int skipLineHeaders = 1) where T : new()
    {
        List<T> result = null;
        if (!string.IsNullOrEmpty(textContent))
        {
            string[] lines = textContent.Split('\n');
            result = FromTable<T>(lines, delimiter, skipLineHeaders);
        }
        return result;
    }

    public static List<T> FromTable<T>(string inputFilePath, System.Text.Encoding encode, char delimiter = '|', int skipLineHeaders = 1) where T : new()
    {
        string[] lines = System.IO.File.ReadAllLines(inputFilePath, encode);
        return FromTable<T>(lines, delimiter, skipLineHeaders);
    }
    public static List<T> FromTable<T>(string[] lines, char delimiter = '|', int skipLineHeaders = 1) where T : new()
    {
        List<T> result = new List<T>();

        if (lines.Length < 2)
        {
            return result;
        }

        FieldInfo[] fields = typeof(T).GetFields();

        for (int i = skipLineHeaders; i < lines.Length; i++)
        {
            string[] values = lines[i].Split('\t');

            T item = new T();

            for (int j = 0; j < fields.Length && j < values.Length; j++)
            {
                var field = fields[j];
                if (field.FieldType == typeof(int))
                {
                    int.TryParse(values[j], out int value);
                    field.SetValueDirect(__makeref(item), value);
                }
                else if (field.FieldType == typeof(float))
                {
                    float.TryParse(values[j], out float value);
                    field.SetValueDirect(__makeref(item), value);
                }
                else if (field.FieldType == typeof(string))
                {
                    field.SetValueDirect(__makeref(item), values[j]);
                }
                else if (field.FieldType == typeof(bool))
                {
                    bool.TryParse(values[j], out bool value);
                    field.SetValueDirect(__makeref(item), value);
                }
                else if (field.FieldType == typeof(int[]))
                {
                    var list = values[j].Split(delimiter).Select(s => { int.TryParse(s, out int v); return v; }).ToArray();
                    field.SetValueDirect(__makeref(item), list);
                }
                else if (field.FieldType == typeof(string[]))
                {
                    var list = values[j].Split(delimiter).ToArray();
                    field.SetValueDirect(__makeref(item), list);
                }
                else if (field.FieldType == typeof(float[]))
                {
                    var list = values[j].Split(delimiter).Select(s => { float.TryParse(s, out float v); return v; }).ToArray();
                    field.SetValueDirect(__makeref(item), list);
                }
            }

            result.Add(item);
        }

        return result;
    }

    public static List<T> Parse<T>(string inputFilePath, System.Text.Encoding encode, char delimiter = '|') where T : new()
    {
        List<T> result = new List<T>();

        string[] lines = System.IO.File.ReadAllLines(inputFilePath, encode);

        if (lines.Length < 2)
        {
            return result;
        }

        string[] headers = lines[0].Split('\t');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split('\t');

            T item = new T();

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                System.Reflection.FieldInfo field = typeof(T).GetField(headers[j]);

                if (field != null)
                {
                    if (field.FieldType == typeof(int))
                    {
                        int.TryParse(values[j], out int value);
                        field.SetValueDirect(__makeref(item), value);
                    }
                    else if (field.FieldType == typeof(float))
                    {
                        float.TryParse(values[j], out float value);
                        field.SetValueDirect(__makeref(item), value);
                    }
                    else if (field.FieldType == typeof(string))
                    {
                        field.SetValueDirect(__makeref(item), values[j]);
                    }
                    else if (field.FieldType == typeof(bool))
                    {
                        bool.TryParse(values[j], out bool value);
                        field.SetValueDirect(__makeref(item), value);
                    }
                    else if (field.FieldType == typeof(int[]))
                    {
                        var list = values[j].Split(delimiter).Select(s => { int.TryParse(s, out int v); return v; }).ToArray();
                        field.SetValueDirect(__makeref(item), list);
                    }
                    else if (field.FieldType == typeof(string[]))
                    {
                        var list = values[j].Split(delimiter).ToArray();
                        field.SetValueDirect(__makeref(item), list);
                    }
                    else if (field.FieldType == typeof(float[]))
                    {
                        var list = values[j].Split(delimiter).Select(s => { float.TryParse(s, out float v); return v; }).ToArray();
                        field.SetValueDirect(__makeref(item), list);
                    }
                }
            }

            result.Add(item);
        }

        return result;
    }

    public static List<Dictionary<string, string>> ParseDictionary(string inputFilePath, System.Text.Encoding encode)
    {
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

        string[] lines = System.IO.File.ReadAllLines(inputFilePath, encode);

        if (lines.Length < 2)
        {
            return result;
        }

        string[] headers = lines[0].Split('\t');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split('\t');

            Dictionary<string, string> item = new Dictionary<string, string>();

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                item.TryAdd(headers[j], values[j]);
            }

            result.Add(item);
        }

        return result;
    }
}
