using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DataTypeTests.TestHelpers
{
    internal static class SandboxTestHelper
    {
        internal const string SandboxFolderName = "rzr-tests";
        internal const string SentinelFileName = "sentinel.txt";
        internal const string SentinelContent = "sentinel";

        internal static bool IsWindows => Path.DirectorySeparatorChar == '\\';

        internal static string SandboxBase => Path.Combine(Path.GetTempPath(), SandboxFolderName);

        internal static string CreateSandbox()
        {
            var root = Path.Combine(SandboxBase, Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(root);

            return root;
        }

        internal static string InSandbox(string sandboxRoot, string path)
        {
            var root = Path.GetFullPath(sandboxRoot).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       + Path.DirectorySeparatorChar;
            var full = Path.GetFullPath(path);
            var comparison = IsWindows ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            if (full.StartsWith(root, comparison).Equals(false)
                || full.StartsWith(SandboxBase + Path.DirectorySeparatorChar, comparison).Equals(false))
                throw new InvalidOperationException($"Path '{full}' is outside the test sandbox '{root}'.");

            return path;
        }

        internal static void DeleteSandbox(string sandboxRoot)
        {
            if (string.IsNullOrEmpty(sandboxRoot) || Directory.Exists(sandboxRoot).Equals(false))
                return;

            EnsureSandboxRoot(sandboxRoot);
            RemoveLinks(new DirectoryInfo(sandboxRoot));
            Directory.Delete(IsWindows ? ToExtendedPath(sandboxRoot) : sandboxRoot, true);
        }

        internal static string CreateDirectory(string sandboxRoot, params string[] parts)
        {
            var path = InSandbox(sandboxRoot, Path.Combine(new[] { sandboxRoot }.Concat(parts).ToArray()));
            Directory.CreateDirectory(path);

            return path;
        }

        internal static string CreateFile(string sandboxRoot, string content, params string[] parts)
        {
            var path = InSandbox(sandboxRoot, Path.Combine(new[] { sandboxRoot }.Concat(parts).ToArray()));
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, content);

            return path;
        }

        internal static string CreateSentinel(string sandboxRoot, params string[] directoryParts)
            => CreateFile(sandboxRoot, SentinelContent, directoryParts.Concat(new[] { SentinelFileName }).ToArray());

        internal static bool IsLink(string path)
            => EntryExists(path) && (File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0;

        internal static bool EntryExists(string path)
            => File.Exists(path) || Directory.Exists(path) || EntryExistsInParent(path);

        internal static bool TryCreateDirectoryLink(string sandboxRoot, string linkPath, string targetPath, out string output)
        {
            InSandbox(sandboxRoot, linkPath);
            InSandbox(sandboxRoot, targetPath);

            if (TryRejectCmdExpansion(linkPath, targetPath, out output))
                return false;

            var exitCode = IsWindows
                ? Run("cmd.exe", $"/c mklink /J \"{linkPath}\" \"{targetPath}\"", out output)
                : Run("ln", $"-s \"{targetPath}\" \"{linkPath}\"", out output);

            return exitCode == 0 && Directory.Exists(linkPath) && IsLink(linkPath);
        }

        internal static bool TryCreateFileLink(string sandboxRoot, string linkPath, string targetPath, out string output)
        {
            InSandbox(sandboxRoot, linkPath);
            InSandbox(sandboxRoot, targetPath);

            if (TryRejectCmdExpansion(linkPath, targetPath, out output))
                return false;

            var exitCode = IsWindows
                ? Run("cmd.exe", $"/c mklink \"{linkPath}\" \"{targetPath}\"", out output)
                : Run("ln", $"-s \"{targetPath}\" \"{linkPath}\"", out output);

            return exitCode == 0 && File.Exists(linkPath) && IsLink(linkPath);
        }

        internal static bool TryCreateExtendedDirectoryJunction(string sandboxRoot, string linkPath, string targetPath, out string output)
        {
            InSandbox(sandboxRoot, linkPath);
            InSandbox(sandboxRoot, targetPath);

            if (IsWindows.Equals(false))
            {
                var exitCode = Run("ln", $"-s \"{targetPath}\" \"{linkPath}\"", out output);

                return exitCode == 0 && IsLink(linkPath);
            }

            if (TryRejectCmdExpansion(linkPath, targetPath, out output))
                return false;

            var extendedLink = ToExtendedPath(linkPath);
            var psCommand = $"New-Item -ItemType Junction -Path '{extendedLink}' -Target '{targetPath}' -ErrorAction Stop | Out-Null";
            var psExit = Run("powershell.exe", $"-NoProfile -NonInteractive -Command \"{psCommand}\"", out output);
            if (psExit == 0 && ExtendedDirectoryLinkExists(extendedLink))
                return true;

            var cmdExit = Run("cmd.exe", $"/c mklink /J \"{extendedLink}\" \"{targetPath}\"", out var cmdOutput);
            output = (output + " | " + cmdOutput).Trim();

            return cmdExit == 0 && ExtendedDirectoryLinkExists(extendedLink);
        }

        internal static void TryRemoveExtendedDirectoryLink(string linkPath)
        {
            try
            {
                var extendedLink = ToExtendedPath(linkPath);
                if (Directory.Exists(extendedLink))
                    Directory.Delete(extendedLink);
            }
            catch
            {
            }
        }

        internal static void TryRemoveExtendedDirectoryTree(string path)
        {
            try
            {
                var extendedPath = ToExtendedPath(path);
                if (Directory.Exists(extendedPath))
                    Directory.Delete(extendedPath, true);
            }
            catch
            {
            }
        }

        internal static bool ExtendedDirectoryLinkExists(string linkPath)
        {
            var extendedLink = ToExtendedPath(linkPath);

            return Directory.Exists(extendedLink)
                   && (File.GetAttributes(extendedLink) & FileAttributes.ReparsePoint) != 0;
        }

        internal static bool AnyFileNamed(string root, string fileName)
            => Directory.Exists(root)
               && Directory.GetFiles(root, fileName, SearchOption.AllDirectories).Length > 0;

        internal static string CreateExtendedDirectory(string sandboxRoot, params string[] parts)
        {
            var path = InSandbox(sandboxRoot, Path.Combine(new[] { sandboxRoot }.Concat(parts).ToArray()));
            Directory.CreateDirectory(ToExtendedPath(path));

            return path;
        }

        internal static string CreateExtendedFile(string sandboxRoot, string content, params string[] parts)
        {
            var path = InSandbox(sandboxRoot, Path.Combine(new[] { sandboxRoot }.Concat(parts).ToArray()));
            Directory.CreateDirectory(ToExtendedPath(Path.GetDirectoryName(path)));
            File.WriteAllText(ToExtendedPath(path), content);

            return path;
        }

        internal static string[] SnapshotExtended(string root)
        {
            var result = new List<string>();
            var extendedRoot = ToExtendedPath(root);
            if (Directory.Exists(extendedRoot))
                WalkExtended(extendedRoot, extendedRoot, result);

            return result.OrderBy(x => x, StringComparer.Ordinal).ToArray();
        }

        private static void WalkExtended(string directory, string root, List<string> result)
        {
            foreach (var entry in Directory.GetFileSystemEntries(directory))
            {
                var relative = entry.Substring(root.Length).TrimStart(Path.DirectorySeparatorChar);
                var attributes = File.GetAttributes(entry);
                var isLink = (attributes & FileAttributes.ReparsePoint) != 0;

                if ((attributes & FileAttributes.Directory) != 0)
                {
                    result.Add(relative + Path.DirectorySeparatorChar + (isLink ? "@" : string.Empty));
                    if (isLink.Equals(false))
                        WalkExtended(entry, root, result);
                }
                else
                {
                    result.Add(relative + (isLink ? "@" : "=" + File.ReadAllText(entry)));
                }
            }
        }

        internal static string[] Snapshot(string root)
        {
            var result = new List<string>();
            if (Directory.Exists(root))
                Walk(new DirectoryInfo(root), Path.GetFullPath(root), result);

            return result.OrderBy(x => x, StringComparer.Ordinal).ToArray();
        }

        private static void Walk(DirectoryInfo directory, string root, List<string> result)
        {
            foreach (var entry in directory.GetFileSystemInfos())
            {
                var relative = entry.FullName.Substring(root.Length).TrimStart(Path.DirectorySeparatorChar);
                var isLink = (entry.Attributes & FileAttributes.ReparsePoint) != 0;

                if (entry is DirectoryInfo childDirectory)
                {
                    result.Add(relative + Path.DirectorySeparatorChar + (isLink ? "@" : string.Empty));
                    if (isLink.Equals(false))
                        Walk(childDirectory, root, result);
                }
                else
                {
                    result.Add(relative + (isLink ? "@" : string.Empty) + "=" + (isLink ? string.Empty : File.ReadAllText(entry.FullName)));
                }
            }
        }

        private static void EnsureSandboxRoot(string sandboxRoot)
        {
            var root = Path.GetFullPath(sandboxRoot).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var comparison = IsWindows ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            if (root.StartsWith(SandboxBase + Path.DirectorySeparatorChar, comparison).Equals(false)
                || root.Length <= SandboxBase.Length + 1)
                throw new InvalidOperationException($"Path '{root}' is not a test sandbox root under '{SandboxBase}'.");
        }

        private static string ToExtendedPath(string absolutePath)
        {
            if (absolutePath.StartsWith(@"\\?\", StringComparison.Ordinal))
                return absolutePath;
            if (absolutePath.StartsWith(@"\\", StringComparison.Ordinal))
                return @"\\?\UNC\" + absolutePath.Substring(2);

            return @"\\?\" + absolutePath;
        }

        private static bool TryRejectCmdExpansion(string linkPath, string targetPath, out string output)
        {
            output = string.Empty;
            if (IsWindows.Equals(false) || (linkPath.IndexOf('%') < 0 && targetPath.IndexOf('%') < 0))
                return false;

            output = "A path that contains '%' cannot be passed to cmd mklink safely.";

            return true;
        }

        private static void RemoveLinks(DirectoryInfo directory)
        {
            if ((directory.Attributes & FileAttributes.ReparsePoint) != 0)
            {
                DeleteLinkEntry(directory.FullName, true);

                return;
            }

            foreach (var entry in directory.GetFileSystemInfos())
            {
                var isLink = (entry.Attributes & FileAttributes.ReparsePoint) != 0;

                if (entry is DirectoryInfo childDirectory)
                {
                    if (isLink)
                        DeleteLinkEntry(childDirectory.FullName, true);
                    else
                        RemoveLinks(childDirectory);
                }
                else if (isLink)
                {
                    DeleteLinkEntry(entry.FullName, false);
                }
            }
        }

        private static void DeleteLinkEntry(string path, bool isDirectory)
        {
            if (isDirectory && IsWindows)
                Directory.Delete(path);
            else
                File.Delete(path);
        }

        private static bool EntryExistsInParent(string path)
        {
            var full = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var parent = Path.GetDirectoryName(full);
            if (string.IsNullOrEmpty(parent) || Directory.Exists(parent).Equals(false))
                return false;

            var name = Path.GetFileName(full);

            return new DirectoryInfo(parent).GetFileSystemInfos()
                .Any(x => string.Equals(x.Name, name, IsWindows ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        }

        private static int Run(string fileName, string arguments, out string output)
        {
            var startInfo = new ProcessStartInfo(fileName, arguments)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            try
            {
                using (var process = Process.Start(startInfo))
                {
                    if (process == null)
                    {
                        output = "Process could not be started.";

                        return -1;
                    }

                    var standardOutput = process.StandardOutput.ReadToEndAsync();
                    var standardError = process.StandardError.ReadToEndAsync();

                    if (process.WaitForExit(30000).Equals(false))
                    {
                        process.Kill();
                        output = "Process timed out.";

                        return -1;
                    }

                    output = (standardOutput.Result + standardError.Result).Trim();

                    return process.ExitCode;
                }
            }
            catch (Exception exception)
            {
                output = exception.Message;

                return -1;
            }
        }
    }
}
