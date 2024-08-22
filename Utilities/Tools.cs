using System.Globalization;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Windows.Data;
using System.Xml.Serialization;

namespace JobTracker.Utilities
{
    internal class Tools
    {
        /// <summary>
        /// Writes the given object instance to an XML file.
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an XML file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the XML file.</returns>
        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
            /// <summary>
            /// Attempts to update the save file if it was created in a previous version of the program
            /// </summary>
            /// <param name="filepath">The location of the file in storage</param>
            /// <param name="renameKeysOldNew">The dictionary of the replacement keys. Old key is first</param>
            /// <returns>True if a save file needed to be and was successfully updated</returns>
            public static bool TryUpdateSaveFile(string filepath, Dictionary<string, string> renameKeysOldNew)
        {
            var saveData = ReadString(filepath);
            var originalSaveData = saveData;
            bool wasChanged = false;
            {
                if (saveData != null)
                {
                    Dictionary<string, string> expandedRenameKeys = new Dictionary<string, string>();
                    string expandedOldKey, newValue, expandedNewValue;
                    //first expand the keys into the actual xml format used
                    foreach (string oldKey in renameKeysOldNew.Keys)
                    {
                        //this is for if the class being renamed is the base class
                        expandedOldKey = $"<ArrayOf{oldKey} ";

                        renameKeysOldNew.TryGetValue(oldKey, out newValue);
                        expandedNewValue = $"<ArrayOf{newValue} ";

                        expandedRenameKeys.Add(expandedOldKey, expandedNewValue);

                        //this is for the closing tag on the class

                        expandedOldKey = $"</ArrayOf{oldKey}>";

                        renameKeysOldNew.TryGetValue(oldKey, out newValue);
                        expandedNewValue = $"</ArrayOf{newValue}>";

                        expandedRenameKeys.Add(expandedOldKey, expandedNewValue);

                        //this is for every other case (opening)

                        expandedOldKey = $"<{oldKey}>";

                        renameKeysOldNew.TryGetValue(oldKey, out newValue);
                        expandedNewValue = $"<{newValue}>";

                        expandedRenameKeys.Add(expandedOldKey, expandedNewValue);

                        //this is for every other case (closing)

                        expandedOldKey = $"</{oldKey}>";

                        renameKeysOldNew.TryGetValue(oldKey, out newValue);
                        expandedNewValue = $"</{newValue}>";

                        expandedRenameKeys.Add(expandedOldKey, expandedNewValue);
                    }

                    foreach (string expandedOldKeyMember in expandedRenameKeys.Keys)
                    {
                        expandedRenameKeys.TryGetValue(expandedOldKeyMember, out expandedNewValue);

                        saveData = saveData.Replace(expandedOldKeyMember, expandedNewValue);
                    }

                    wasChanged = (saveData != originalSaveData);

                    if (wasChanged)
                    {
                        WriteString(filepath, saveData);

                        //and then create a backup as well
                        var backupFilePath = $"{Path.GetDirectoryName(filepath)}\\{Path.GetFileNameWithoutExtension(filepath)}_BACKUP{Path.GetExtension(filepath)}";
                        WriteString(backupFilePath, originalSaveData);
                        }   
                }
            }


            return wasChanged;
        }

        public static string ReadString(string filePath)
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                return reader.ReadToEnd();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public static void WriteString(string filePath, string value)
        {
            var writer = new StreamWriter(filePath);
            try
            {
                writer.Write(value);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
    }

    public class LookupConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)values[0];
            var dates = values[1] as HashSet<DateTime>;
            return dates.Contains(date);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
