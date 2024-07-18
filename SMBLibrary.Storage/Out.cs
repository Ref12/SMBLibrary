using System.Runtime.CompilerServices;

namespace Codex.Utilities
{
    public ref struct Out<T>
    {
        public bool IsValid { get; }

        public readonly ref T Value => ref _ref;

        private ref T _ref;

        public unsafe Out(out T value)
        {
            value = default;
            _ref = ref Unsafe.AsRef(ref value);
            IsValid = true;
        }

        public unsafe Out(ref T value, int i)
        {
            _ref = ref Unsafe.AsRef(ref value);
            IsValid = true;
        }

        public void Set(T value)
        {
            if (IsValid)
            {
                _ref = value;
            }
        }

        public static implicit operator T(Out<T> o)
        {
            return o.Value;
        }
    }

    public static class Out
    {
        public static Out<T> Create<T>(ref T value) => new Out<T>(ref value, default);

        public static void SetOrCreate<T>(this ref Out<T> box, in T value)
        {
            if (box.IsValid)
            {
                box.Set(value);
            }
            else
            {
                box = Create(ref Unsafe.AsRef(value));
            }
        }

        public static ref T Ref<T>(out T value) => ref new Out<T>(out value).Value;

        public static bool TryBoth(bool left, bool right)
        {
            return left || right;
        }

        public static bool VarIf<T>(bool condition, in T input, out T value)
        {
            value = condition ? input : default;
            return condition;
        }

        public static T Var<T>(out T value, T input)
        {
            value = input;
            return value;
        }

        public static bool TrueVar<T>(out T value, T input)
        {
            value = input;
            return true;
        }
    }
}