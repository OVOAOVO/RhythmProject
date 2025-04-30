#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;

public class BeatDataPostprocessor : IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
        string src = "Assets/BeatBook/beat_data.json";

        // 拷贝到 .exe 同级目录
        string buildFolder = Path.GetDirectoryName(report.summary.outputPath);
        string dst = Path.Combine(buildFolder, "beat_data.json");

        File.Copy(src, dst, true);
    }
}
#endif
