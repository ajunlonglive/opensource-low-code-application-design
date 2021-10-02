using System;

namespace Domain.ValueObjects
{
    public abstract class Enumeration<T> : IEquatable<Enumeration<T>>
    {
        #region IEquatable Members

        public bool Equals(Enumeration<T> other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((Enumeration<T>)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(Enumeration<T> left, Enumeration<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Enumeration<T> left, Enumeration<T> right)
        {
            return !Equals(left, right);
        }

        #endregion IEquatable Members

        protected Enumeration(T value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public T Value { get; }

        public string DisplayName { get; }
    }
}