using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TimeTool.Contracts;

namespace TimeTool.DataAccess.Settings
{
    /// <summary>
    /// Handles deep cloning on objects that implement the required interface.
    /// </summary>
    public static class SettingsWriter
    {
        #region Public Methods

        /// <summary>
        /// Saves the serialized data into the given file name. If the file already exists the file will be overridden.
        /// </summary>
        /// <param name="objectToBeSaved">The object to be saved.</param>
        /// <param name="fileName">Path and file name to define the saving location.</param>
        /// <param name="saveOption">Defines if a backup file should be created if a file with the same name already exists.</param>
        public static void Save(IDeepCloneable objectToBeSaved, string fileName, BackupOption saveOption = BackupOption.CreateBackup)
        {
            var serializedData = GetSerializedData(objectToBeSaved);

            BackupFileIfExists(fileName, saveOption);

            using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                fileStream.Write(serializedData, 0, serializedData.Length);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// If the file with the given name already exists it is renamed with a ".bak" extension.
        /// </summary>
        /// <param name="fileName">Path and file name to define the saving location.</param>
        /// <param name="saveOption">Defines if a backup file should be created if a file with the same name already exists.</param>
        private static void BackupFileIfExists(string fileName, BackupOption saveOption)
        {
            if (saveOption != BackupOption.CreateBackup)
            {
                return;
            }

            if (!(File.Exists(fileName)))
            {
                return;
            }

            // Get file name
            string backupFile = $"{fileName}.{Files.BackupFileExtension}";

            // Old backup file is not needed anymore.
            if (File.Exists(backupFile))
            {
                File.Delete(backupFile);
            }

            // Rename is a MOVE in Windows API
            File.Move(fileName, backupFile);
        }

        /// <summary>
        /// Serializes the given object.
        /// </summary>
        /// <param name="objectToBeSerialized">The object to be serialized.</param>
        /// <returns>Returns the serialized data.</returns>
        private static byte[] GetSerializedData(IDeepCloneable objectToBeSerialized)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                StreamingContext streamingContext = new StreamingContext(StreamingContextStates.Remoting);
                BinaryFormatter binaryFormatter = new BinaryFormatter(null, streamingContext);

                binaryFormatter.Serialize(memoryStream, objectToBeSerialized);

                memoryStream.Seek(0, SeekOrigin.Begin);
                byte[] serializedData = memoryStream.ToArray();
                return serializedData;
            }
        }

        #endregion Private Methods
    }
}
