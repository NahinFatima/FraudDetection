namespace FraudDetection.Application.Results
{
    public class Result<T>
    {
        public bool Success { get; private init; }
        public T? Value { get; private init; }
        public string Error { get; private init; } = " ";
        public static Result<T> Ok(T value) => new() { Success = true, Value = value };
        public static Result<T> Fail(string error) => new() { Error = error };
    }
}
