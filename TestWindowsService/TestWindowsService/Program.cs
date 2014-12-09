using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TestWindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            var scServices = ServiceController.GetServices();
            foreach (var item in scServices)
            {
                Console.WriteLine("Service:\{item.ServiceName} ");
                Console.WriteLine("DisplayName name:\{item.DisplayName}");
                Console.WriteLine("item status: \{item.Status}");
                ManagementObject wmiService;
                wmiService = new ManagementObject("Win32_Service.Name='\{item.ServiceName}'");
                wmiService.Get();
                Console.WriteLine("StartName: \{wmiService["StartName"]}");
                Console.WriteLine("Description:\{wmiService["Description"]}");
                EventLog log = new EventLog("",item.MachineName);
                //需要在此注册表下 添加子项 key值为TestWindowsService
                //HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Eventlog\Application
                if (!EventLog.SourceExists("TestWindowsService"))
                {
                    EventLog.CreateEventSource(item.ServiceName + ".Main", item.DisplayName);
                }
                log.Source = "TestWindowsService";
                log.WriteEntry(DateTime.Now.ToLongDateString()+ " displayname:" +item.DisplayName);
                
            }

            Console.Read();
        }
    }
}
