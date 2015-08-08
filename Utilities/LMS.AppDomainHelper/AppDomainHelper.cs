namespace LMS.AppDomainHelper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;

    using mscoree;

    public class AppDomainHelper
    {
        public delegate void CrossAppDomainCallDelegate(params object[] payload);
        public delegate object CrossAppDomainCallRetValueDelegate(params object[] payload);

        public static AppDomain GetAppDomainByName(string name)
        {
            return GetAppDomains().FirstOrDefault(appDomain => appDomain.FriendlyName == name);
        }

        public static void InvokeOnAppDomain(AppDomain appDomain, CrossAppDomainCallDelegate targetFunc, params object[] payload)
        {
            appDomain.SetData(appDomain.FriendlyName + "_payload", payload);
            appDomain.SetData(appDomain.FriendlyName + "_func", targetFunc);
            appDomain.DoCallBack(InvokedOnAppDomain);
        }

        public static T InvokeOnAppDomain<T>(AppDomain appDomain, CrossAppDomainCallRetValueDelegate targetFunc, params object[] payload)
        {
            appDomain.SetData(appDomain.FriendlyName + "_payload", payload);
            appDomain.SetData(appDomain.FriendlyName + "_func", targetFunc);
            appDomain.DoCallBack(InvokedOnAppDomainRet);

            return (T)Convert.ChangeType(appDomain.GetData("result"), typeof(T));
        }

        private static void InvokedOnAppDomain()
        {
            AppDomain currentAppDomain = AppDomain.CurrentDomain;
            string name = currentAppDomain.FriendlyName;

            // Grab payload.
            object[] payload = (object[])currentAppDomain.GetData(name + "_payload");
            CrossAppDomainCallDelegate func = (CrossAppDomainCallDelegate)currentAppDomain.GetData(name + "_func");

            func.Invoke(payload);
        }

        private static void InvokedOnAppDomainRet()
        {
            AppDomain currentAppDomain = AppDomain.CurrentDomain;
            string name = currentAppDomain.FriendlyName;

            // Grab payload.
            object[] payload = (object[])currentAppDomain.GetData(name + "_payload");
            CrossAppDomainCallRetValueDelegate func = (CrossAppDomainCallRetValueDelegate)currentAppDomain.GetData(name + "_func");

            object result = func.Invoke(payload);
            currentAppDomain.SetData("result", result);
        }

        public static IList<AppDomain> GetAppDomains()
        {
            IList<AppDomain> domains = new List<AppDomain>();
            IntPtr enumHandle = IntPtr.Zero;
            ICorRuntimeHost host = new CorRuntimeHost();

            try
            {
                host.EnumDomains(out enumHandle);
                while (true)
                {
                    object domain;
                    host.NextDomain(enumHandle, out domain);
                    if (domain == null) break;
                    AppDomain appDomain = (AppDomain)domain;
                    domains.Add(appDomain);
                }

                return domains;
            }
            finally
            {
                host.CloseEnum(enumHandle);
                Marshal.ReleaseComObject(host);
            }
        } 
    }
}