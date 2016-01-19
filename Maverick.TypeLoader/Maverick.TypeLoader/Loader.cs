using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Maverick.TypeLoader
{
    /// <summary>
    /// A class to load types from assemblies on disk or in memory that inherit / implement the specified base type of T.
    /// The loading is not statically typed to prevent the caller from inheriting a dependncny on an actual Type and thus
    /// allowing dynamic loading of any class inheriting / implementing the specified type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Loader<T> where T : class
    {
        /// <summary>
        /// Creates an instance of the specified typeName if found in an assembly in the list of fileNames. The type must have a default constructor.
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public T LoadTypeFromFiles(IEnumerable<string> fileNames, string typeName)
        {
            var types = FindImplementingTypesInMultipleFiles(fileNames);
            return CreateTypeInstance(types, typeName);
        }

        public T LoadTypeFromAssembly(Assembly assembly, string typeName)
        {
            var types = FindImplementingTypesInAssembly(assembly);
            return CreateTypeInstance(types, typeName);
        }

        private T CreateTypeInstance(IEnumerable<Type> types, string typeName)
        {
            var typeFound = SelectTypeFromListFound(typeName, types);
            return (T)Activator.CreateInstance(typeFound.Assembly.FullName, typeFound.FullName).Unwrap();
        }

        private Type SelectTypeFromListFound(string typeName, IEnumerable<Type> implementingTypes)
        {
            try
            {
                var typeFound = implementingTypes.First(t => t.Name == typeName);
                return typeFound;
            }
            catch (InvalidOperationException ex)
            {
                throw new TypeLoadException($"Type '{typeName}' not found ", ex);
            }
        }

        private IEnumerable<Type> FindImplementingTypesInMultipleFiles(IEnumerable<string> fileNames)
        {
            var implementingTypes = new List<Type>();
            foreach (var file in fileNames)
            {
                var assembly = Assembly.LoadFile(file);
                var types = FindImplementingTypesInAssembly(assembly);
                implementingTypes.AddRange(types);
            }
            return implementingTypes;
        }

        private IEnumerable<Type> FindImplementingTypesInAssembly(Assembly assembly)
        {
            return assembly.GetExportedTypes()
                           .Where(t => typeof(T).IsAssignableFrom(t));
        }
    }
}