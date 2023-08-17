using System.Reflection;

namespace Authentication.UnitTest.TestTools;
public static class UtilPrivateMethods
{
    public static TReturn CallPrivateMethod<TReturn>(this object instance, string methodName, params object[] parameters)
    {
        Type type = instance.GetType();
        BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Instance;
        MethodInfo method = type.GetMethod(methodName, bindingAttr)!;

        TReturn result = (TReturn)method.Invoke(instance, parameters)!;

        if (result is Task task)
            task.GetAwaiter().GetResult();

        return result;
    }
}
