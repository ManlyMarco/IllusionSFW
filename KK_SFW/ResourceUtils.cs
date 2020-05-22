using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SFWmod
{
    /// <summary>Utility methods for working with embedded resources.</summary>
    internal static class ResourceUtils
    {
        /// <summary>
        /// Read all bytes starting at current position and ending at the end of the stream.
        /// </summary>
        public static byte[] ReadAllBytes(this Stream input)
        {
            byte[] buffer = new byte[16384];
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int count;
                while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
                    memoryStream.Write(buffer, 0, count);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Get a file set as "Embedded Resource" from the assembly that is calling this code, or optionally from a specified assembly.
        /// The filename is matched to the end of the resource path, no need to give the full path.
        /// If 0 or more than 1 resources match the provided filename, an exception is thrown.
        /// For example if you have a file "ProjectRoot\Resources\icon.png" set as "Embedded Resource", you can use this to load it by
        /// doing <code>GetEmbeddedResource("icon.png"), assuming that no other embedded files have the same name.</code>
        /// </summary>
        /// <exception cref="T:System.IO.IOException">Thrown if none or more than one resources were found matching the given resourceFileName</exception>
        public static byte[] GetEmbeddedResource(string resourceFileName, Assembly containingAssembly = null)
        {
            bool flag = false;
            if (containingAssembly == null)
            {
                containingAssembly = Assembly.GetCallingAssembly();
                flag = true;
            }
            List<string> list = ((IEnumerable<string>)containingAssembly.GetManifestResourceNames()).Where<string>((Func<string, bool>)(str => str.EndsWith(resourceFileName))).Take<string>(2).ToList<string>();
            if (list.Count == 0 & flag)
            {
                Assembly assembly = containingAssembly;
                containingAssembly = new StackFrame(1).GetMethod().DeclaringType.Assembly;
                if (assembly != containingAssembly)
                {
                    list = ((IEnumerable<string>)containingAssembly.GetManifestResourceNames()).Where<string>((Func<string, bool>)(str => str.EndsWith(resourceFileName))).Take<string>(2).ToList<string>();
                    if (list.Count == 0)
                        throw new IOException(string.Format("Could not find resource with name {0} inside assembly {1} or {2} - make sure the name and assembly are correct. Two assemblies were checked likely because your method has been harmony patched", (object)list, (object)containingAssembly, (object)assembly));
                }
            }
            if (list.Count == 0)
                throw new IOException(string.Format("Could not find resource with name {0} inside assembly {1} - make sure the name and assembly are correct", (object)list, (object)containingAssembly));
            if (list.Count == 2)
                throw new IOException(string.Format("Found more than one resource with name {0} inside assembly {1} - include more of the path in the name to make it not ambiguous", (object)list, (object)containingAssembly));
            using (Stream manifestResourceStream = containingAssembly.GetManifestResourceStream(list[0]))
            {
                Stream input = manifestResourceStream;
                if (input == null)
                    throw new InvalidOperationException(string.Format("The resource {0} was not found inside assembly {1} or it failed to load", (object)resourceFileName, (object)containingAssembly));
                return input.ReadAllBytes();
            }
        }
    }
}