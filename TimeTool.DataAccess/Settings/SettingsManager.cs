using System;
using System.IO;
using System.Threading.Tasks;

namespace TimeTool.DataAccess.Settings
{
  public static class SettingsManager
  {
    private const string FileName = "NextDate.json";
    private const string BackupFileName = "NextDate.json.bak";

    internal static async Task<SettingsObject> Load()
    {
      SettingsObject userSettings;

      if (!File.Exists(FileName))
      {
        userSettings = new SettingsObject
        {
          LastFetchedDate = DateTime.Now
        };

        return userSettings;
      }

      await using var fileStream = File.OpenRead(FileName);
      userSettings = await JsonSerializer.DeserializeAsync<SettingsObject>(fileStream);

      return userSettings;
    }

    internal static async Task Save(SettingsObject settingsObject)
    {
      BackupSettingsFile();

      await using var fileStream = File.Create(FileName);
      await JsonSerializer.SerializeAsync(fileStream, settingsObject);
    }

    private static void BackupSettingsFile()
    {
      if (!(File.Exists(FileName)))
      {
        return;
      }

      // Old backup file is not needed anymore.
      if (File.Exists(BackupFileName))
      {
        File.Delete(BackupFileName);
      }

      // Rename is a MOVE in Windows API
      File.Move(FileName, BackupFileName);
    }
  }
}
