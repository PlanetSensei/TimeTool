//-----------------------------------------------------------------------
// <copyright file="FileSystem.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.DataAccess
{
  using System;
  using System.IO;

  /// <summary>
  /// Encapsulates access to th e file system.
  /// </summary>
  public static class FileSystem
  {
    /// <summary>
    /// The name of the application.
    /// </summary>
    private const string ApplicationName = "TimeTool";

    /// <summary>
    /// The name of the database file.
    /// </summary>
    private const string DatabaseFile = "TimeTool.db";

    /// <summary>
    /// Finds the folder where the user specific application files will be stored.
    /// </summary>
    /// <returns>Returns the fully assembled application path.</returns>
    public static string FindUserAppFolder()
    {
      var roaminFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      var appFolder = Path.Combine(roaminFolder, ApplicationName);

      CreateIfNotExists(appFolder);

      return appFolder;
    }

    /// <summary>
    /// Finds the fully qualified folder and file name to the work time database.
    /// </summary>
    /// <returns>Returns the fully qualified database file loaction.</returns>
    public static string FindDatabaseFile()
    {
      var appFolder = FindUserAppFolder();
      var databaseLocation = Path.Combine(appFolder, DatabaseFile);

      return databaseLocation;
    }

    /// <summary>
    /// Creates a new folder at the specified path <paramref name="appFolder"/> if it does not yet exist.
    /// </summary>
    /// <param name="appFolder">The folder to be created.</param>
    private static void CreateIfNotExists(string appFolder)
    {
      if (!Directory.Exists(appFolder))
      {
        Directory.CreateDirectory(appFolder);
      }
    }
  }
}