using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperTests
{
  using TimeTool.BusinessLogic;
  using TimeTool.DataAccess;

  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine();
      Console.WriteLine("Starting...");

      const int year = 2017;
      const int month = 9;

      var defaultWorkLength = new TimeSpan(0, 7, 42, 0);

      using (var access = new WorkdayAccess(@"c:\Users\hellmann\AppData\Roaming\TimeTool\TimeTool.db"))
      {
        var workdays = access.GetDays(year, month);

        foreach (var day in workdays)
        {
          Console.WriteLine("Processing {0}", day.StartTime.Date);

          //if (day.EndTime > day.StartTime)
          //{
          //  continue;
          //}

          //var logOff = UserInfo.SetLastDayWorkEndTimeIfEmpty(day.StartTime.Date);
          //day.EndTime = logOff;

          day.DefaultWorkLength = defaultWorkLength;

          access.Save(day);
        }
      }

      Console.WriteLine();
      Console.WriteLine("Finished.");

      Console.ReadKey();
    }
  }
}
