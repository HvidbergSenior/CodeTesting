using System.Reflection;

namespace deftq.BuildingBlocks.Domain
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        private List<PropertyInfo>? _properties;

        private List<FieldInfo>? _fields;

        public static bool operator ==(ValueObject obj1, ValueObject obj2)
        {
            if (object.Equals(obj1, null))
            {
                return object.Equals(obj2, null);
            }

            return obj1.Equals(obj2);
        }

        public static bool operator !=(ValueObject obj1, ValueObject obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(ValueObject? other)
        {
            return Equals(other as object);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return GetProperties().All(p => PropertiesAreEqual(obj, p))
                && GetFields().All(f => FieldsAreEqual(obj, f));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (var prop in GetProperties())
                {
                    object? value = prop.GetValue(this, index: null);
                    hash = HashValue(hash, value);
                }

                foreach (var field in GetFields())
                {
                    object? value = field.GetValue(this);
                    hash = HashValue(hash, value);
                }

                return hash;
            }
        }

        private bool PropertiesAreEqual(object obj, PropertyInfo p)
        {
            return object.Equals(p.GetValue(this, index: null), p.GetValue(obj, index: null));
        }

        private bool FieldsAreEqual(object obj, FieldInfo f)
        {
            var response = object.Equals(f.GetValue(this), f.GetValue(obj));
            return response;
        }

        private IEnumerable<PropertyInfo> GetProperties()
        {
            return this._properties ??= GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToList();
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            return this._fields ??= GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .ToList();
        }

        private static int HashValue(int seed, object? value)
        {
            var currentHash = value?.GetHashCode() ?? 0;

            return (seed * 23) + currentHash;
        }
    }
}
