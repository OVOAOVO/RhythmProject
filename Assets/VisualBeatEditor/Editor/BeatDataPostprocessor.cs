#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;

public class BeatDataPostprocessor : IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
        string sourceFolder = "Assets/BeatBook";
        string buildFolder = Path.GetDirectoryName(report.summary.outputPath);
        string destFolder = Path.Combine(buildFolder, "BeatBook");

        // 创建目标文件夹（如果不存在）
        if (!Directory.Exists(destFolder))
        {
            Directory.CreateDirectory(destFolder);
        }

        // 拷贝所有 .json 文件
        string[] jsonFiles = Directory.GetFiles(sourceFolder, "*.json", SearchOption.TopDirectoryOnly);

        foreach (var file in jsonFiles)
        {
            string fileName = Path.GetFileName(file);
            string destPath = Path.Combine(destFolder, fileName);
            File.Copy(file, destPath, true);
        }

    }
}
#endif
