using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace CSharpCrawler.Util
{
    public struct ACCENTPOLICY
    {
        public int nAccentState;
        public int nFlags;
        public int nColor;
        public int nAnimationId;
    }

    public struct WINCOMPATTRDATA
    {
        public int nAttribute;
        public IntPtr pData;
        public int ulDataSize;
    }

    public class WindowUtil
    {
        [DllImport("user32.dll")]
        public static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WINCOMPATTRDATA data);

        private const int ACCENT_ENABLE_BLURBEHIND = 3;
        private const int WCA_ACCENT_POLICY = 19;

        public static void BlurWindow(System.Windows.Window window)
        {
            var winhelp = new WindowInteropHelper(window);

            ACCENTPOLICY policy_Blur = new ACCENTPOLICY();
            policy_Blur.nAccentState = ACCENT_ENABLE_BLURBEHIND;
            policy_Blur.nFlags = 0;
            policy_Blur.nColor = 0;
            policy_Blur.nAnimationId = 0;

            WINCOMPATTRDATA wINCOMPATTRDATA = new WINCOMPATTRDATA();
            wINCOMPATTRDATA.nAttribute = WCA_ACCENT_POLICY;
            IntPtr pData = Marshal.AllocHGlobal(Marshal.SizeOf(policy_Blur));
            Marshal.StructureToPtr(policy_Blur, pData, false);
            wINCOMPATTRDATA.pData = pData;
            wINCOMPATTRDATA.ulDataSize = Marshal.SizeOf(policy_Blur);

            SetWindowCompositionAttribute(winhelp.Handle, ref wINCOMPATTRDATA);

            Marshal.FreeHGlobal(pData);
        }
    }
}
