using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Maverick.TypeLoader
{
    /// <summary>
    /// A class to load types from assemblies on disk that inherit / implement the specified base type of T 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TypeLoader<T> where T: class
    {
        /// <summary>
        /// Creates an instance of the specified typeName if found in an assembly in the list of fileNames. The type must have a default constructor. 
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public T LoadImplementingTypeFromFiles(IEnumerable<string> fileNames, string typeName)
        {
            var implementingTypes = FindImplementingTypesInMultipleFiles(fileNames);
            var typeFound = SelectTypeFromListFound(typeName, implementingTypes);
            return (T)Activator.CreateInstance(typeFound.Assembly.FullName, typeFound.FullName).Unwrap();
        }

        private static Type SelectTypeFromListFound(string typeName, IEnumerable<Type> implementingTypes)
        {
            var typeFound = implementingTypes.First(t => t.Name == typeName);
            return typeFound;
        }

        private IEnumerable<Type> FindImplementingTypesInMultipleFiles(IEnumerable<string> fileNames)
        {
            var implementingTypes = new List<Type>();
            foreach (var file in fileNames)
            {
                var typesFound = FindImplementingTypesInSingleFile(file).ToArray();
                implementingTypes.AddRange(typesFound);
            }
            return implementingTypes;
        }

        private IEnumerable<Type> FindImplementingTypesInSingleFile(string assemblyFileName)
        {
            return Assembly.LoadFile(assemblyFileName)
                           .GetExportedTypes()
                           .Where(t => typeof(T).IsAssignableFrom(t));
        }
    }
}
