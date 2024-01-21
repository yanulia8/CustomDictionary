using MyLibrary;

namespace UseOfLibrary;

class Program
{
    static void Main()
    {
        CustomDictionary<int, string> dictionary = new CustomDictionary<int, string>();
        // event registration
        dictionary.ItemAdded += (sender, e) => Console.WriteLine($"Item with key {e.Key} was added");
        dictionary.ItemRemoved += (sender, e) => Console.WriteLine($"Item with key {e.Key} was removed");
        dictionary.ItemChanged += (sender, e) => Console.WriteLine($"Value of item with key {e.Key} was changed");
        dictionary.DictionaryCleared += (sender, e) => Console.WriteLine("Dictionary was cleared");

        Console.WriteLine("Adding entries:");
        dictionary.Add(1, "One");
        dictionary[2] = "Two";
        dictionary.Add(new KeyValuePair<int, string>(3, "Three"));

        Console.WriteLine("\nThe entries are:");
        foreach (KeyValuePair<int, string> item in dictionary)
            Console.WriteLine($"{item.Key}: {item.Value}");

        Console.WriteLine("\nChanging entry's value by index:");
        dictionary[2] = "Twooooo";
        Console.WriteLine($"2: {dictionary[2]}");

        Console.WriteLine("\nRemoving entry:");
        dictionary.Remove(1);

        Console.WriteLine("\nEntries after removal:");
        foreach (KeyValuePair<int, string> item in dictionary)
            Console.WriteLine($"{item.Key}: {item.Value}");

        Console.WriteLine("\nTrying to add entry which already exists");
        try
        {
            dictionary.Add(3, "Something");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception message is: {e.Message}");
        }

        Console.WriteLine("\nCopying all the entries to some array.");
        KeyValuePair<int, string>[] entries = new KeyValuePair<int,string>[dictionary.Count];
        dictionary.CopyTo(entries, 0);

        Console.WriteLine("The entries of array after copying are:");
        foreach (var entry in entries)
            Console.WriteLine($"{entry.Key}: {entry.Value}");

        Console.WriteLine("\nChecking if entry (4, \"Four\") exists:\n" + dictionary.Contains(new KeyValuePair<int,string>(4, "Four")));
        Console.WriteLine("\nChecking if entry with key 3 exists:\n" + dictionary.ContainsKey(3));

        dictionary.Clear();
        Console.WriteLine(dictionary.Count);
    }
}