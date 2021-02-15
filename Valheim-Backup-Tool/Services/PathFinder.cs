using System;
using System.Runtime.InteropServices;
using Valheim_Backup_Tool.Interfaces;

namespace Valheim_Backup_Tool.Services
{
    public class PathFinder : IPathFinder
    {
        private const string LOCAL_LOW_ID = "A520A1A4-1780-4FF6-BD18-167343C5AF16";
        private const string VALHEIM_REL_PATH = @"\IronGate\Valheim";

        public string GetValheimDefaultFolderPath()
        {
            return $"{GetLocalLowFolderPath()}{VALHEIM_REL_PATH}";
        }

        public string GetLocalLowFolderPath()
        {
            var localLowId = new Guid(LOCAL_LOW_ID);

            return GetKnownFolderPath(localLowId);
        }

        string GetKnownFolderPath(Guid knownFolderId)
        {
            IntPtr pszPath = IntPtr.Zero;
            try
            {
                int hr = SHGetKnownFolderPath(knownFolderId, 0, IntPtr.Zero, out pszPath);
                if (hr >= 0)
                    return Marshal.PtrToStringAuto(pszPath);
                throw Marshal.GetExceptionForHR(hr);
            }
            finally
            {
                if (pszPath != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(pszPath);
            }
        }

        [DllImport("shell32.dll")]
        static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr pszPath);
    }
}
