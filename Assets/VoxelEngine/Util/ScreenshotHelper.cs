using System;
using System.IO;
using UnityEngine;

namespace VoxelEngine.Util {

    public static class ScreenshotHelper {

        private static string screenshotDirectoryName = "screenshots/";

        /// <summary>
        /// Takes a screenshot.  When no path is passes, the default screenshot directory is used.
        /// </summary>
        public static void captureScreenshot(string screenshotPath = null) {
            if (screenshotPath == null) {
                string time = DateTime.Now.ToString();
                time = time.Replace('/', '-').Replace(' ', '_').Replace(':', '.').Substring(0, time.Length - 3);

                if (!Directory.Exists(ScreenshotHelper.screenshotDirectoryName)) {
                    Directory.CreateDirectory(ScreenshotHelper.screenshotDirectoryName);
                }
                screenshotPath = ScreenshotHelper.screenshotDirectoryName + time + ".png";
            }
            Application.CaptureScreenshot(screenshotPath);
        }
    }
}
