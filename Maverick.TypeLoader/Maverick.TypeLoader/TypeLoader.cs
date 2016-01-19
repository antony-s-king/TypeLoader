using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Maverick.TypeLoader
{
    public class TypeLoader<T> where T: class
    {
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
