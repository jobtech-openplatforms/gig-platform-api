using System;
using System.Collections.Generic;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects
{
    #region String Identity

    public abstract class StringIdentity : ValueObject
    {
        public string Value { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(StringIdentity stringIdentity) => stringIdentity.Value;
    }

    public abstract class StringIdentity<T> : StringIdentity
    {
        private string _prefix = $"{typeof(T).Name}s/";
        private string _value;

        public new string Value
        {
            get { return _value; }
            set { _value = value.StartsWith(_prefix, true, System.Globalization.CultureInfo.InvariantCulture) ? value : $"{_prefix}{value}"; }
        }

        public string Short() => this.Value.Replace(_prefix, "");
    }

    #endregion String Identity

    #region String Token

    public abstract class StringToken : ValueObject
    {
        public string Value { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(StringToken StringToken) => StringToken.Value;
    }

    public abstract class StringToken<T> : StringToken
    {
        private string _value;

        public new string Value
        {
            get { return _value; }
            set
            {
                System.Guid.Parse(value); // Validation
                _value = value;
            }
        }
    }

    #endregion String Token
    #region Guid Identity

    public abstract class GuidIdentity : ValueObject
    {
        public Guid Value { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(GuidIdentity guidIdentity) => guidIdentity.Value;
    }

    public abstract class GuidIdentity<T> : GuidIdentity
    {
        private Guid _value;

        public new Guid Value
        {
            get { return _value; }
            set { _value =  value; }
        }
    }

    #endregion Guid Identity
}