using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace UsefullLibraries
{
    public class ApiOperationResultNext
    {
        public static ApiOperationResultNext Success()
        {
            return new ApiOperationResultNext();
        }

        public static ApiOperationResultNext Failure(Error error)
        {
            return new ApiOperationResultNext(error);
        }

        public static ApiOperationResultNext Failure(StatusCode statusCode, [NotNull] Urn id, [CanBeNull] string message)
        {
            return new ApiOperationResultNext(new Error(id, statusCode, message));
        }

        protected ApiOperationResultNext()
        {
            IsSuccess = true;
        }

        protected ApiOperationResultNext(Error error)
        {
            IsSuccess = false;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFail => !IsSuccess;
        public Error Error { get; }
    }


    public enum StatusCode
    {
        Unknown = 0,

        #region Informational 1xx

        Continue = 100,
        SwitchingProtocols = 101,

        #endregion

        #region Successful 2xx

        // ReSharper disable once InconsistentNaming
        OK = 200,
        Created = 201,
        Accepted = 202,
        NonAuthoritativeInformation = 203,
        NoContent = 204,
        ResetContent = 205,
        PartialContent = 206,

        #endregion

        #region Redirection 3xx

        MultipleChoices = 300,
        MovedPermanently = 301,
        Found = 302,
        SeeOther = 303,
        NotModified = 304,
        UseProxy = 305,
        Unused = 306,
        TemporaryRedirect = 307,

        #endregion

        #region Client Error 4xx

        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeout = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        RequestEntityTooLarge = 413,
        RequestUriTooLong = 414,
        UnsupportedMediaType = 415,
        RequestedRangeNotSatisfiable = 416,
        ExpectationFailed = 417,
        UpgradeRequired = 426,
        TooManyRequests = 429,
        ConnectFailure = 450,
        ReceiveFailure = 451,
        SendFailure = 452,
        UnknownFailure = 453,
        Canceled = 454,
        StreamReuseFailure = 455,
        StreamInputFailure = 456,

        #endregion

        #region Server Error 5xx

        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        HttpVersionNotSupported = 505

        #endregion
    }

    [JsonObject]
    public sealed class Error
    {
        [NotNull]
        [JsonProperty("id")]
        [JsonConverter(typeof(UrnConverter))]
        public Urn Id { get; }

        [JsonProperty("statusCode")]
        public StatusCode StatusCode { get; }

        [CanBeNull]
        [JsonProperty("message")]
        public string Message { get; }

        [NotNull]
        [ItemNotNull]
        [JsonProperty("details")]
        public IReadOnlyList<Error> Details { get; }

        [JsonConstructor]
        public Error([NotNull] Urn id, StatusCode statusCode, string message, IReadOnlyList<Error> details = null)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            StatusCode = statusCode;
            Message = message;
            Details = details ?? new Error[0];
        }

        public Error([NotNull] Urn id, StatusCode statusCode, string message, [NotNull] Error detailedError)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            StatusCode = statusCode;
            Message = message;
            Details = new[] { detailedError ?? throw new ArgumentNullException(nameof(detailedError)) };
        }

        private bool Equals(Error other)
        {
            return Id.Equals(other.Id) &&
                   StatusCode == other.StatusCode &&
                   string.Equals(Message, other.Message, StringComparison.InvariantCultureIgnoreCase) &&
                   Details.SequenceEqual(other.Details);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj is Error error && Equals(error);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)StatusCode;
                hashCode = (hashCode * 397) ^ (Message != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Message) : 0);
                Details.ForEach(detailedError => hashCode = (hashCode * 397) ^ detailedError.GetHashCode());
                return hashCode;
            }
        }

        public static bool operator ==(Error left, Error right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Error left, Error right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new KeApiJsonSerializerSettings());
        }
    }
}
