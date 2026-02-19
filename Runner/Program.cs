using System.Reflection;
using Library;
using Tests;

namespace Runner;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var assembly = Assembly.GetAssembly(typeof(ServiceTests));
            await RunTests(assembly);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке сборки: {ex.Message}");
        }
    }

    static async Task RunTests(Assembly assembly)
    {
        int passed = 0;
        int failed = 0;
        int skipped = 0;
        
        var testClasses = assembly.GetTypes()
            .Where(t => t.GetCustomAttribute<MyTestClassAttribute>() != null);

        foreach (var type in testClasses)
        {
            Console.WriteLine($"\nЗапуск тестов в классе: {type.Name}");
            
            var setupMethod = type.GetMethods().FirstOrDefault(m => m.GetCustomAttribute<MyBeforeTestAttribute>() != null);
            var teardownMethod = type.GetMethods().FirstOrDefault(m => m.GetCustomAttribute<MyAfterTestAttribute>() != null);
            
            var testMethods = type.GetMethods()
                .Where(m => m.GetCustomAttribute<MyTestAttribute>() != null);

            foreach (var method in testMethods)
            {
                var testAttr = method.GetCustomAttribute<MyTestAttribute>();
                
                if (!string.IsNullOrEmpty(testAttr?.Skip))
                {
                    Console.WriteLine($"  [SKIP] {method.Name} (Причина: {testAttr.Skip})");
                    skipped++;
                    continue;
                }
                
                var testCases = method.GetCustomAttributes<MyTestCaseAttribute>().ToList();
               
                var casesToRun = testCases.Any() 
                    ? testCases.Select(c => c.Params).ToList() 
                    : new List<object[]> { null };
                
                foreach (var parameters in casesToRun)
                {
                    var instance = Activator.CreateInstance(type);
                    string paramInfo = parameters != null ? $"({string.Join(", ", parameters)})" : "";

                    try
                    {
                        setupMethod?.Invoke(instance, null);

                        await InvokeMethodWithTaskSupport(method, instance, parameters);

                        teardownMethod?.Invoke(instance, null);

                        Console.WriteLine($"  [PASS] {method.Name}{paramInfo}");
                        passed++;
                    }
                    catch (TargetInvocationException ex) 
                    {
                        var inner = ex.InnerException;

                        if (inner is MyTestFailedException) 
                        {
                            Console.WriteLine($"  [FAILED] {method.Name}{paramInfo}: {inner.Message}");
                            failed++;
                        }
                        else 
                        {
                            Console.WriteLine($"  [ERROR] {method.Name}{paramInfo}: Исключение типа {inner?.GetType().Name}: {inner?.Message}");
                            failed++;
                        }
                    }
                     catch (Exception ex)
                    {
                        Console.WriteLine($"  [ARG_ERROR] {method.Name}{paramInfo}: {ex.Message}");
                        failed++;
                    }
                }
            }
        }
        
        Console.WriteLine($"РЕЗУЛЬТАТЫ: Пройдено: {passed}, Провалено: {failed}, Пропущено: {skipped}");
    }
    
    static async Task InvokeMethodWithTaskSupport(MethodInfo method, object instance, object[] parameters)
    {
        var result = method.Invoke(instance, parameters);
        
        if (result is Task task)
        {
            await task;
        }
    }
}