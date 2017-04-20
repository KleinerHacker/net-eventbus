namespace net.eventbus.Type
{
    internal sealed class MethodKey
    {
        public MethodKey(string name, System.Type paramType)
        {
            Name = name;
            ParamType = paramType;
        }

        public string Name { get; }
        public System.Type ParamType { get; }

        private bool Equals(MethodKey other)
        {
            return string.Equals(Name, other.Name) && Equals(ParamType, other.ParamType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj is MethodKey && Equals((MethodKey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^
                       (ParamType != null ? ParamType.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(ParamType)}: {ParamType}";
        }
    }
}