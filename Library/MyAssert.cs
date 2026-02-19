using System.Collections;


namespace Library
{
    public class MyTestFailedException : Exception 
    {
        public MyTestFailedException(string message) : base(message) { }
    }

    public static class MyAssert 
    {
        // 1. Проверка на равенство
        public static void AreEqual(object expected, object actual) 
        {
            if (!Equals(expected, actual)) 
                throw new MyTestFailedException($"Expected: {expected}, but got: {actual}");
        }

        // 2. Проверка на неравенство
        public static void AreNotEqual(object val1, object val2) 
        {
            if (Equals(val1, val2)) 
                throw new MyTestFailedException($"Values should not be equal, but both are: {val1}");
        }

        // 3. Истинность условия
        public static void IsTrue(bool condition) 
        {
            if (!condition) throw new MyTestFailedException("Expected True, but got False");
        }

        // 4. Ложность условия
        public static void IsFalse(bool condition) 
        {
            if (condition) throw new MyTestFailedException("Expected False, but got True");
        }

        // 5. Проверка на null
        public static void IsNull(object obj) 
        {
            if (obj != null) throw new MyTestFailedException("Expected null, but object is not null");
        }

        // 6. Проверка на НЕ null
        public static void IsNotNull(object obj) 
        {
            if (obj == null) throw new MyTestFailedException("Expected object instance, but got null");
        }

        // 7. Содержится ли элемент в коллекции
        public static void Contains(object item, IEnumerable collection) 
        {
            if (collection == null) throw new MyTestFailedException("Collection is null");
            
            bool found = false;
            foreach (var i in collection) 
            {
                if (Equals(i, item)) { found = true; break; }
            }
            
            if (!found) throw new MyTestFailedException($"Item '{item}' not found in collection");
        }

        // 8. Пуста ли коллекция
        public static void IsNotEmpty(IEnumerable collection) 
        {
            if (collection == null || !collection.Cast<object>().Any()) 
                throw new MyTestFailedException("Collection is empty or null");
        }

        // 9. Проверка типа объекта
        public static void IsInstanceOf<T>(object obj) 
        {
            if (!(obj is T)) 
                throw new MyTestFailedException($"Expected type {typeof(T).Name}, but got {obj?.GetType().Name ?? "null"}");
        }

        // 10. Проверка на выброс исключения 
        public static void Throws<T>(Action action) where T : Exception 
        {
            try 
            {
                action();
            }
            catch (T) 
            {
                return; 
            }
            catch (Exception ex) 
            {
                throw new MyTestFailedException($"Expected exception {typeof(T).Name}, but caught {ex.GetType().Name}");
            }
            throw new MyTestFailedException($"Expected exception {typeof(T).Name} was not thrown");
        }

        // 11: Проверка строк на вхождение 
        public static void StringContains(string substring, string fullString) 
        {
            if (string.IsNullOrEmpty(fullString) || !fullString.Contains(substring))
                throw new MyTestFailedException($"String '{fullString}' does not contain '{substring}'");
        }
    }
}