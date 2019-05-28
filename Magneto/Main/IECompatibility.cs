/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/

using Microsoft.Win32;
using System;
using System.IO;
using System.Security;

namespace Magneto.Main
{
    internal static class IECompatibility
    {
        private const string BaseRegistryKey = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\";

        private const int EmulationKeyValue = 11001;
        private const string FeatureEmulation = "FEATURE_BROWSER_EMULATION";

        private const int DpiKeyValue = 1;
        private const string FeatureDpi = "FEATURE_96DPI_PIXEL";

        public static void FixModeIfNeeded()
        {
            IgnoreAccessErrors(() =>
            {
                FixPrehistoricModeIfNeeded();
            });
            IgnoreAccessErrors(() =>
            {
                FixDpiIfNeeded();
            });
        }

        internal static void IgnoreAccessErrors(Action action)
        {
            try
            {
                action();
            }
            catch (SecurityException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        private static void FixPrehistoricModeIfNeeded()
        {
            if (GetFeatureValue(FeatureEmulation) != EmulationKeyValue)
            {
                SetFeatureValue(FeatureEmulation, EmulationKeyValue, RegistryValueKind.DWord);
            }
        }

        private static void FixDpiIfNeeded()
        {
            if (GetFeatureValue(FeatureDpi) != DpiKeyValue)
            {
                SetFeatureValue(FeatureDpi, DpiKeyValue, RegistryValueKind.DWord);
            }
        }

        private static int GetFeatureValue(string keyName)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(keyName, false))
            {
                if (key != null)
                {
                    var programName = GetProgramName();
                    var value = key.GetValue(programName, 0);
                    return Convert.ToInt32(value);
                }
                return 0;
            }
        }

        private static void SetFeatureValue(string feature, object value, RegistryValueKind kind)
        {
            var programName = GetProgramName();
            using (var parent = Registry.CurrentUser.OpenSubKey(BaseRegistryKey, true))
            {
                if (parent == null)
                {
                    return;
                }
                using (var key = parent.OpenSubKey(feature, true))
                {
                    if (key == null)
                    {
                        using (var newKey = parent.CreateSubKey(feature))
                        {
                            newKey.SetValue(programName, value, kind);
                        }
                    }
                    else
                    {
                        key.SetValue(programName, value, kind);
                    }
                }
            }
        }

        private static string GetProgramName()
        {
            return Path.GetFileName(Environment.GetCommandLineArgs()[0]);
        }
    }
}
