using System;
using System.IO;
using UnityEngine;

public class ScreenshotHelper {

    private static string dirName = "screenshots/";

    public static void captureScreenshot() {
        string s = DateTime.Now.ToString();
        s = s.Replace('/', '-').Replace(' ', '_').Replace(':', '.').Substring(0, s.Length - 3);

        if (!Directory.Exists(ScreenshotHelper.dirName)) {
            Directory.CreateDirectory(ScreenshotHelper.dirName);
        }
        Application.CaptureScreenshot(ScreenshotHelper.dirName + s + ".png");
    }
}
