using To2dnd;
using System.Reflection;

public static class TBinarySerialization  
{
    public static byte[] Serialize(object target, bool callAfterDeserializationMethods = true)
    {
        TSerizalization serialization = new TSerizalization();
        return serialization.Serialize(target, callAfterDeserializationMethods);
    }

    public static T Deserialize<T>(byte[] bytes, bool callBeforeSerializationMethods = true)
    {

        TSerizalization serialization = new TSerizalization();
        return serialization.Deserialize<T>(bytes, Assembly.GetExecutingAssembly(), true);
    }

   

}
