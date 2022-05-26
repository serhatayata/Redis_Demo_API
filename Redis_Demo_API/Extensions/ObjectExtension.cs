using Newtonsoft.Json;

namespace Redis_Demo_API.Extensions
{
    public static class ObjectExtension
    {
        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
