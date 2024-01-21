using System;
using System.Collections;
using System.Collections.Generic;

namespace MyLibrary;

public class CustomDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    private Node head;
    public int Count { get; private set; }
    public bool IsReadOnly => false;
    
    public event EventHandler<KeyEventArgs<TKey>> ItemAdded;
    public event EventHandler<KeyEventArgs<TKey>> ItemRemoved;
    public event EventHandler<KeyEventArgs<TKey>> ItemChanged; 
    public event EventHandler DictionaryCleared;

    private class Node
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public Node Next { get; set; }

        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Next = null;
        }
    }

    public CustomDictionary()
    {
        head = null;
        Count = 0;
    }

    public TValue this[TKey key]
    {
        get
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");
            }
            
            Node current = head;
            while (current != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(current.Key, key))
                {
                    return current.Value;
                }
                current = current.Next;
            }

            // Key not found
            throw new KeyNotFoundException();
        }
        set
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");
            }
            
            Node current = head;
            while (current != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(current.Key, key))
                {
                    current.Value = value;
                    // Trigger the ItemChanged event
                    ItemChanged?.Invoke(this, new KeyEventArgs<TKey>(key));
                    return;
                }
                current = current.Next;
            }

            // Key not found, add a new node
            Add(key, value);
        }
    }

    public ICollection<TKey> Keys
    {
        get
        {
            TKey[] keys = new TKey[Count];
            Node current = head;
            for (int i = 0; current != null; i++)
            {
                keys[i] = current.Key;
                current = current.Next;
            }
            return keys;
        }
    }

    public ICollection<TValue> Values
    {
        get
        {
            TValue[] values = new TValue[Count];
            Node current = head;
            for (int i = 0; current != null; i++)
            {
                values[i] = current.Value;
                current = current.Next;
            }
            return values;
        }
    }

    public void Add(TKey key, TValue value)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key), "Key cannot be null.");
        }
        if (ContainsKey(key))
        {
            throw new ArgumentException($"An element with key '{key}' already exists in the dictionary.");
        }
        
        Node newNode = new Node(key, value);
        newNode.Next = head;
        head = newNode;
        Count++;
        
        // Trigger the ItemAdded event
        ItemAdded?.Invoke(this, new KeyEventArgs<TKey>(key));
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        head = null;
        Count = 0;
        
        // Trigger the DictionaryCleared event
        DictionaryCleared?.Invoke(this, EventArgs.Empty);
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        if (item.Key == null)
        {
            throw new ArgumentNullException(nameof(item), "Key cannot be null.");
        }
        
        Node current = head;
        while (current != null)
        {
            if (EqualityComparer<TKey>.Default.Equals(current.Key, item.Key) && EqualityComparer<TValue>.Default.Equals(current.Value, item.Value))
            {
                return true;
            }
            current = current.Next;
        }
        return false;
    }

    public bool ContainsKey(TKey key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key), "Key cannot be null.");
        }
        
        Node current = head;
        while (current != null)
        {
            if (EqualityComparer<TKey>.Default.Equals(current.Key, key))
            {
                return true;
            }
            current = current.Next;
        }
        return false;
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        if (array == null)
        {
            throw new ArgumentNullException();
        }
        if (arrayIndex < 0 || arrayIndex > array.Length)
        {
            throw new IndexOutOfRangeException();
        }
        if (array.Length - arrayIndex < Count)
        {
            throw new ArgumentException("Not enough space to fit all elements");
        }
        
        Node current = head;
        while (current != null)
        {
            array[arrayIndex++] = new KeyValuePair<TKey, TValue>(current.Key, current.Value);
            current = current.Next;
        }
    }

    public bool Remove(TKey key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key), "Key cannot be null.");
        }
        
        if (head == null)
        {
            return false;
        }

        if (EqualityComparer<TKey>.Default.Equals(head.Key, key))
        {
            head = head.Next;
            Count--;
            
            // Trigger the ItemRemoved event
            ItemRemoved?.Invoke(this, new KeyEventArgs<TKey>(key));
            return true;
        }

        Node current = head;
        while (current.Next != null && !EqualityComparer<TKey>.Default.Equals(current.Next.Key, key))
        {
            current = current.Next;
        }

        if (current.Next != null)
        {
            current.Next = current.Next.Next;
            Count--;
            
            // Trigger the ItemRemoved event
            ItemRemoved?.Invoke(this, new KeyEventArgs<TKey>(key));
            return true;
        }

        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key), "Key cannot be null.");
        }
        Node current = head;
        while (current != null)
        {
            if (EqualityComparer<TKey>.Default.Equals(current.Key, key))
            {
                value = current.Value;
                return true;
            }
            current = current.Next;
        }

        value = default;
        return false;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        Node current = head;
        while (current != null)
        {
            yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
            current = current.Next;
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}