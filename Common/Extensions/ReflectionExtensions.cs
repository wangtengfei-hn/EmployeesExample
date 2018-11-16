using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="input"></param>
        /// <param name="additional"></param>
        /// <returns></returns>
        public static TOutput Translate<TInput, TOutput>(this TInput input, Action<TInput, TOutput> additional = null)
            where TOutput : new()
            => translate(input, additional);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="input"></param>
        /// <param name="additional"></param>
        /// <returns></returns>
        public static TOutput translate<TInput, TOutput>(TInput input, Action<TInput, TOutput> additional = null)
        {
            var output = (TOutput)translate(typeof(TOutput), typeof(TInput), input);

            additional?.Invoke(input, output);

            return output;
        }

        static object translate(Type outputType, Type inputType, object input)
        {
            if (input == null)
                return null;

            var outputTypeInfo = outputType.GetTypeInfo();
            var inputTypeInfo = inputType.GetTypeInfo();

            if (outputType.IsInstanceOfType(input))
                return input;

            if (outputType.IsAssignableFrom(inputType))
                return input;

            if (inputTypeInfo.IsEnum && (inputType == typeof(short) || inputType == typeof(ushort) || inputType == typeof(int) || inputType == typeof(uint) || inputType == typeof(long) || inputType == typeof(ulong)))
                return Convert.ChangeType(Enum.Format(inputType, input, "D"), outputType);

            if (outputTypeInfo.IsEnum && (inputType == typeof(short) || inputType == typeof(ushort) || inputType == typeof(int) || inputType == typeof(uint) || inputType == typeof(long) || inputType == typeof(ulong)))
                return Enum.ToObject(outputType, input);

            var typeofIEnumerable = typeof(IEnumerable);

            if (typeofIEnumerable.IsAssignableFrom(outputType) && typeofIEnumerable.IsAssignableFrom(inputType))
            {
                var outputElementType = outputType.HasElementType
                    ? outputTypeInfo.GetElementType()
                    : outputType.IsConstructedGenericType
                    ? outputType.GenericTypeArguments[0]
                    : null;
                var inputArray = Enumerable.ToArray((input as IEnumerable).Cast<object>());
                var outputArray = Array.CreateInstance(outputElementType ?? typeof(object), inputArray.Length);
                inputArray.Select(_ => outputElementType == null ? _ : translate(outputElementType, _.GetType(), _)).ToArray().CopyTo(outputArray, 0);
                return outputArray;
            }

            if (outputType == typeof(string))
                return input?.ToString();

            if (outputTypeInfo.IsValueType)
            {
                if ("Nullable`1" == outputType.Name)
                    try
                    {
                        return Convert.ChangeType(input, outputType.GenericTypeArguments[0]);
                    }
                    catch
                    {
                        return null;
                    }

                try
                {
                    return Convert.ChangeType(input, outputType);
                }
                catch { }
            }

            if (outputTypeInfo.IsInterface)
                return null;

            if (outputTypeInfo.IsAbstract)
                return null;

            var inputProperties = inputType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var outputProperties = outputType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var output = Activator.CreateInstance(outputType);
            foreach (var outputProperty in outputProperties)
            {
                if (!outputProperty.CanWrite)
                    continue;

                var inputProperty = inputProperties.FirstOrDefault(_ => _.Name == outputProperty.Name);
                if (inputProperty == null || !inputProperty.CanRead)
                    continue;

                outputProperty.SetValue(output, translate(outputProperty.PropertyType, inputProperty.PropertyType, inputProperty.GetValue(input)));
            }

            return output;
        }
    }
}
