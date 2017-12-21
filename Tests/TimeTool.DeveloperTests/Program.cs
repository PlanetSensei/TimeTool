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
      const int month = 12;

      var defaultWorkLength = new TimeSpan(0, 7, 42, 0);

      using (var access = new WorkdayAccess(@"c:\Users\hellmann\AppData\Roaming\TimeTool\TimeTool.db"))
      {
        var workdays = access.GetDays(year, month).ToArray();

        foreach (var day in workdays)
        {
          if (day.StartTime.Hour == 0 && day.StartTime.Minute == 0)
          {
            continue;
          }

          Console.WriteLine("Processing {0}", day.StartTime.Date);
          var endDate = UserInfo.SetLastDayWorkEndTimeIfEmpty(day.StartTime);

          //if (!(day.EndTime > DateTime.MinValue))
          //{
          day.EndTime = endDate;
          //}

          //if (day.EndTime > day.StartTime)
          //{
          //  continue;
          //}

          //var logOff = UserInfo.SetLastDayWorkEndTimeIfEmpty(day.StartTime.Date);
          //day.EndTime = logOff;

          //day.DefaultWorkLength = defaultWorkLength;

          //access.Save(day);
        }

        access.Save(workdays);
      }
      Console.WriteLine();
      Console.WriteLine("Finished.");

      Console.ReadKey();
    }
  }
}
