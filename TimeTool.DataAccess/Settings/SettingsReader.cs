using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TimeTool.DataAccess.Settings
{
    internal static class SettingsReader
    {
        internal static WindowSettings Read()
        {
            var windowSettings = Load<WindowSettings>(Files.SettingsFile);

            return windowSettings ?? new WindowSettings();
        }

        /// <summary>
        /// Loads the serialized data into the given file name. If the file already exists the file will be overridden.
        /// </summary>
        /// <param name="fileName">Path and file name to define the saving location.</param>
        /// <returns>Returns the object that was created using the data of the serialized file.</returns>
        private static T Load<T>(string fileName)
        {
            T loadedInstance;

            if (!(File.Exists(fileName)))
            {
                loadedInstance = default(T);
                return loadedInstance;
            }

            byte[] serializedData;
            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int length = (int)fileStream.Length;
                serializedData = new byte[length];

                fileStream.Read(serializedData, 0, length);
            }

            object objectInstance = DeserializeData(serializedData);
            loadedInstance = (T)objectInstance;

            return loadedInstance;
        }

        /// <summary>
        /// Deserializes the raw bytes into a new object instance.
        /// </summary>
        /// <param name="serializedData">The serialized object in raw byte form.</param>
        /// <returns>Returns the created object.</returns>
        private static object DeserializeData(byte[] serializedData)
        {
            using (MemoryStream memoryStream = new MemoryStream(serializedData))
            {
                StreamingContext streamingContext = new StreamingContext(StreamingContextStates.Remoting);
                BinaryFormatter binaryFormatter = new BinaryFormatter(null, streamingContext);
                object cloneInstance = binaryFormatter.Deserialize(memoryStream);

                return cloneInstance;
            }
        }
    }
}
