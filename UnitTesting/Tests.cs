using MyLibrary;

namespace UnitTesting;

public class Tests
{
    [Test]
    public void Add_AddingDuplicateKey_ArgumentExceptionThrown()
    {
        // Arrange
        var dictionary = new CustomDictionary<int, string>();
        dictionary.Add(1, "One");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => dictionary.Add(1, "Duplicate"));
    }

    [Test]
    public void Remove_RemoveNonExistingItem_ReturnsFalse()
    {
        // Arrange
        var dictionary = new CustomDictionary<int, string>();

        // Act
        bool result = dictionary.Remove(1);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void Remove_RemoveItem()
    {
        // Arrange
        var dictionary = new CustomDictionary<int, string>();
        dictionary.Add(1, "One");
        dictionary.Add(2, "Two");

        // Act
        dictionary.Remove(2);
        var enumerator = dictionary.GetEnumerator();

        // Assert
        enumerator.MoveNext();
        Assert.AreEqual(1, enumerator.Current.Key);
        Assert.AreEqual("One", enumerator.Current.Value);

        bool result = enumerator.MoveNext();
        Assert.AreEqual(false, result);
    }

    [Test]
    public void Clear_ClearDictionary_CountEqualsZero()
    {
        // Arrange
        var dictionary = new CustomDictionary<int, string>();
        dictionary.Add(1, "One");

        // Act
        dictionary.Clear();

        // Assert
        Assert.AreEqual(dictionary.Count, 0);
    }

    [Test]
    public void TryGetValue_GetExistingValue_ReturnsTrueAndCorrectValue()
    {
        // Arrange
        var dictionary = new CustomDictionary<int, string>();
        dictionary.Add(1, "One");

        // Act
        bool result = dictionary.TryGetValue(1, out var value);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual("One", value);
    }

    [Test]
    public void TryGetValue_GetNonExistingValue_ReturnsFalseAndDefault()
    {
        // Arrange
        var dictionary = new CustomDictionary<int, string>();

        // Act
        bool result = dictionary.TryGetValue(1, out var value);

        // Assert
        Assert.IsFalse(result);
        Assert.IsNull(value);
    }
    
    [Test]
    public void GetEnumerator_EnumerateThroughDictionary_ItemsAreInReversedOrder()
    {
        // Arrange
        var dictionary = new CustomDictionary<int, string>();
        dictionary.Add(1, "One");
        dictionary.Add(2, "Two");

        // Act
        var enumerator = dictionary.GetEnumerator();

        // Assert
        enumerator.MoveNext();
        Assert.AreEqual(2, enumerator.Current.Key);
        Assert.AreEqual("Two", enumerator.Current.Value);

        enumerator.MoveNext();
        Assert.AreEqual(1, enumerator.Current.Key);
        Assert.AreEqual("One", enumerator.Current.Value);
    }

    [Test]
    public void Keys_GetCollectionOfKeys()
    {
        // Arrange
        var dictionary = new CustomDictionary<int, string>();
        dictionary.Add(1, "One");
        dictionary.Add(2, "Two");

        // Act
        ICollection<int> keys = dictionary.Keys;
        var enumerator = keys.GetEnumerator();

        // Assert
        enumerator.MoveNext();
        Assert.AreEqual(2, enumerator.Current);

        enumerator.MoveNext();
        Assert.AreEqual(1, enumerator.Current);
    }
    
    [Test]
    public void Values_GetCollectionOfValues()
    {
        // Arrange
        var dictionary = new CustomDictionary<int, string>();
        dictionary.Add(1, "One");
        dictionary.Add(2, "Two");

        // Act
        ICollection<string> values = dictionary.Values;
        var enumerator = values.GetEnumerator();

        // Assert
        enumerator.MoveNext();
        Assert.AreEqual("Two", enumerator.Current);

        enumerator.MoveNext();
        Assert.AreEqual("One", enumerator.Current);
    }

    [Test]
    public void Indexer_ChangingValueByIndex()
    {
        // Arrange
        var dictionary = new CustomDictionary<int, string>();
        dictionary.Add(1, "One");

        // Act
        dictionary[1] = "Two";

        // Assert
        Assert.AreEqual("Two", dictionary[1]);
    }
    
    [Test]
    public void ContainsKey_ContainsExistingAndNonExistingKey()
    {
        // Arrange
        var dictionary = new CustomDictionary<int, string>();
        dictionary.Add(1, "One");

        // Act
        bool existing = dictionary.ContainsKey(1);
        bool nonExisting = dictionary.ContainsKey(2);

        // Assert
        Assert.IsTrue(existing);
        Assert.IsFalse(nonExisting);
    }
    
    [Test]
    public void Contains_ContainsExistingEntry()
    {
        // Arrange
        var dictionary = new CustomDictionary<int, string>();
        dictionary.Add(1, "One");

        // Act
        bool existing = dictionary.Contains(new KeyValuePair<int, string>(1, "One"));

        // Assert
        Assert.IsTrue(existing);
    }
    
    [Test]
    public void CopyTo_CopyEntriesIntoArray()
    {
        // Arrange
        var dictionary = new CustomDictionary<int, string>();
        dictionary.Add(1, "One");
        dictionary.Add(2, "Two");
        KeyValuePair<int, string>[] entries = new KeyValuePair<int, string>[3];

        // Act
        entries[0] = new KeyValuePair<int, string>(5, "Five");
        dictionary.CopyTo(entries, 1);

        // Assert
        Assert.AreEqual(2, entries[1].Key);
        Assert.AreEqual("Two", entries[1].Value);
        Assert.AreEqual(1, entries[2].Key);
        Assert.AreEqual("One", entries[2].Value);
    }
}