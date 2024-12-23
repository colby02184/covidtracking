using System.Text.Json;

namespace HealthMonitor.Services.CQRS
{
    public class MessagePayload
    {

        public MessagePayload()
        {
            
        }
        public string Body { get; set; } = "";
        public string TypeName { get; set; } = "";

        public MessagePayload(object request)
        {
            Body = GetBody(request);
            TypeName = request.GetType().FullName + ", " + request.GetType().Assembly.GetName().Name;
        }

        public string GetBody(object request) => JsonSerializer.Serialize(request, request.GetType());

        public string GetJson() => JsonSerializer.Serialize(this, GetType());

        public TReturn GetBodyObject<TReturn>() => (TReturn)JsonSerializer.Deserialize(Body, typeof(TReturn));

        public object GetBodyObject() => JsonSerializer.Deserialize(Body, Type.GetType(TypeName, true));
    }
}