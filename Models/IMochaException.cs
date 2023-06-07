[System.Serializable]
public class IMochaException : System.Exception
{
    public IMochaException() { }
    public IMochaException(string message) : base(message) { }
    public IMochaException(string message, System.Exception inner) : base(message, inner) { }
    protected IMochaException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}