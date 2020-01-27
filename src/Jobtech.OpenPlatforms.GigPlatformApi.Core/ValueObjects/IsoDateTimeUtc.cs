using AF.GigPlatform.Core.Extensions;
using System;
using System.Collections.Generic;

namespace AF.GigPlatform.Core.ValueObjects
{
    /// <summary>
    /// Takes any valid date-time string and converts to DateTime in UTC.
    /// Returns a string in
    /// </summary>
    public class IsoDateTimeUtc : ValueObject
    {
        private DateTime _dateTime = DateTime.MinValue;

        private string _value;

        public string Value
        {
            get => _value = _value ?? _dateTime.ToIsoString();
            set => _dateTime = DateTime.Parse(value, System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime();
        }

        public DateTime GetDateTime() => _dateTime;

        public static IsoDateTimeUtc FromDateTime(DateTime dateTime) => new IsoDateTimeUtc { Value = dateTime.ToIsoString() };

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(IsoDateTimeUtc value) => value.Value;

        public static implicit operator IsoDateTimeUtc(string value) => new IsoDateTimeUtc { Value = value };
    }
}