namespace TimeTool.DataAccess.Settings
{
  internal static class Configurator
  {
    internal static void AssignSettings(MainWindow window, WindowSettings settings)
    {
      window.Location = settings.Location;
      ResetToScreenCenterIfWindowIsOutsideScreen(window, settings);
    }

    private static void ResetToScreenCenterIfWindowIsOutsideScreen(MainWindow window, WindowSettings settings)
    {
      if (!IsOnScreen(window))
      {
        settings.Location = FindScreenCenter(window);
        window.Location = settings.Location;
      }
    }

    private static Point FindScreenCenter(Form window)
    {
      return new Point((Screen.PrimaryScreen.Bounds.Size.Width / 2) - (window.Size.Width / 2), (Screen.PrimaryScreen.Bounds.Size.Height / 2) - (window.Size.Height / 2));
    }

    private static bool IsOnScreen(Control form)
    {
      var formRectangle = new Rectangle(form.Left, form.Top, form.Width, form.Height);

      var isOnAnyScreen = Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(formRectangle));
      return isOnAnyScreen;
    }
  }
}
