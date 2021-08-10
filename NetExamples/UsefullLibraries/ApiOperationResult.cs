using System;
using System.Net;

namespace UsefullLibraries
{
    public class ApiOperationResult
    {
        public static ApiOperationResult Success()
        {
            return new ApiOperationResult();
        }

        public static ApiOperationResult<TResult> Success<TResult>(TResult result)
        {
            return ApiOperationResult<TResult>.Success(result);
        }

        public static ApiOperationResult Failure(ErrorInfo errorInfo)
        {
            return new ApiOperationResult(errorInfo);
        }

        public static ApiOperationResult<TResult> Failure<TResult>(ErrorInfo errorInfo)
        {
            return ApiOperationResult<TResult>.Failure(errorInfo);
        }

        public ApiOperationResult<TResult> Failure<TResult>()
        {
            return Failure<TResult>(ErrorInfo);
        }

        protected ApiOperationResult()
        {
            IsSuccess = true;
        }

        internal ApiOperationResult(ErrorInfo errorInfo)
        {
            IsSuccess = false;
            ErrorInfo = errorInfo;
        }

        public bool IsSuccess { get; }
        public bool IsFail => !IsSuccess;
        public ErrorInfo ErrorInfo { get; }

        public override string ToString()
        {
            return
                $"ApiOperationResult: IsSuccess={IsSuccess}, ErrorCode='{ErrorInfo?.ErrorCode}', Message='{ErrorInfo?.Message}'";
        }
    }

    public class ApiOperationResult<TResult> : ApiOperationResult
    {
        public static ApiOperationResult<TResult> Success(TResult result)
        {
            return new ApiOperationResult<TResult>(result);
        }

        public new static ApiOperationResult<TResult> Failure(ErrorInfo errorInfo)
        {
            return new ApiOperationResult<TResult>(errorInfo);
        }

        /// ToDo(Mamzerov): Костыль! Ждёт рефакторинга тасок.
        public static ApiOperationResult<TResult> Failure(ErrorInfo errorInfo, TResult result)
        {
            return new ApiOperationResult<TResult>(errorInfo, result);
        }

        private ApiOperationResult(TResult result)
        {
            Result = result;
        }

        private ApiOperationResult(ErrorInfo errorInfo)
            : base(errorInfo)
        {
        }

        public ApiOperationResult(ErrorInfo errorInfo, TResult result) : base(errorInfo)
        {
            Result = result;
        }

        public TResult Result { get; }
    }

    public class ErrorInfo
    {
        internal ErrorInfo(string message, Urn errorCode, HttpStatusCode httpStatusCode)
        {
            Message = message;
            ErrorCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }

        public string Message { get; }
        public Urn ErrorCode { get; }
        public HttpStatusCode HttpStatusCode { get; }

        public override string ToString()
        {
            return $"message:{Message}, errorCode:{ErrorCode}, httpCode:{HttpStatusCode}";
        }
    }

	public sealed class Urn : IComparable<Urn>, IEquatable<Urn>
	{
		private const string Schema = "urn:";

		public Urn(string value)
		{
			if (value == null)
				throw new ArgumentNullException(nameof(value));

			if (0 != string.Compare(value, 0, Schema, 0, Schema.Length, StringComparison.OrdinalIgnoreCase))
				throw new Exception("Invalid URN schema");

			this.Value = value.Substring(Schema.Length);
		}

		public Urn(string nid, string nss)
		{
			if (nid == null)
				throw new ArgumentNullException(nameof(nid));

			if (nss == null)
				throw new ArgumentNullException(nameof(nss));

			Value = nid + ':' + nss;
		}

		public Urn(Urn parent, string nss)
		{
			if (parent == null)
				throw new ArgumentNullException(nameof(parent));

			if (nss == null)
				throw new ArgumentNullException(nameof(nss));

			Value = parent.Value + ':' + nss;
		}

		public string Nid
		{
			get
			{
				var index = Value.LastIndexOf(':');
				return index != -1 ? Value.Substring(0, index) : string.Empty;
			}
		}

		public string Nss
		{
			get
			{
				var index = Value.LastIndexOf(':');
				return index != -1 ? Value.Substring(index + 1) : Value;
			}
		}

		public string Value { get; private set; }

		public int CompareTo(Urn other)
		{
			if (other == null)
				return 1;

			return string.Compare(Value, other.Value, StringComparison.OrdinalIgnoreCase);
		}

		public bool Equals(Urn other)
		{
			return ((object)other) != null && 0 == string.Compare(Value, other.Value, StringComparison.OrdinalIgnoreCase);
		}

		public static Urn Parse(string value)
		{
			return new Urn(value);
		}

		public static bool TryParse(string value, out Urn result)
		{
			result = null;

			if (value == null || !value.ToLower().StartsWith(Schema))
				return false;

			result = Parse(value);
			return true;
		}

		public static bool operator ==(Urn a, Urn b)
		{
			if (ReferenceEquals(a, b))
				return true;

			if ((object)a == null)
				return false;

			return a.Equals(b);
		}

		public static bool operator !=(Urn a, Urn b)
		{
			return !(a == b);
		}

		public Urn CreateChild(string nss)
		{
			return new Urn(this, nss);
		}

		public bool IsParentOf(Urn urn)
		{
			if (urn == null)
				throw new ArgumentNullException(nameof(urn));

			if (urn.Value.Length <= Value.Length)
				return false;

			if (0 != string.Compare(Value, 0, urn.Value, 0, Value.Length, StringComparison.OrdinalIgnoreCase))
				return false;

			return urn.Value[Value.Length] == ':';
		}

		public bool IsChildOf(Urn urn)
		{
			if (urn == null)
				throw new ArgumentNullException(nameof(urn));

			return urn.IsParentOf(this);
		}

		public override int GetHashCode()
		{
			return Value.ToLowerInvariant().GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Urn);
		}

		public override string ToString()
		{
			return Schema + Value;
		}
	}
}
