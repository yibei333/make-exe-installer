using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MakeExeInstaller.Extensions
{
    /// <summary>
    /// 文件扩展
    /// </summary>
    public static class FileExtension
    {
        /// <summary>
        /// 断言一个可枚举对象是否为Null或者长度为0
        /// </summary>
        /// <typeparam name="T">需要断言的可枚举对象反省类型</typeparam>
        /// <param name="source">需要断言的可枚举对象</param>
        /// <returns>可枚举对象是否为Null或者长度为0</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source is null || source.Count() <= 0;

        /// <summary>
        /// 断言一个可枚举对象是否不为Null并且长度大于0
        /// </summary>
        /// <typeparam name="T">需要断言的可枚举对象反省类型</typeparam>
        /// <param name="source">需要断言的可枚举对象</param>
        /// <returns>可枚举对象是否不为Null并且长度大于0</returns>
        public static bool NotNullOrEmpty<T>(this IEnumerable<T> source) => source != null && source.Count() > 0;

        /// <summary>
        /// 获取文件的扩展名
        /// </summary>
        /// <param name="filePath">文件路径,文件名也可以</param>
        /// <param name="includePoint">是否包含"."</param>
        /// <returns>扩展名</returns>
        /// <exception cref="ArgumentNullException">当filePath参数为空时引发异常</exception>
        public static string GetFileExtension(this string filePath, bool includePoint = true)
        {
            if (filePath.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(filePath));
            var extension = new FileInfo(filePath).Extension;
            return includePoint ? extension : extension.TrimStart(".");
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="includeExtension">是否包含扩展名</param>
        /// <returns>文件名</returns>
        /// <exception cref="ArgumentNullException">当filePath参数为空时引发异常</exception>
        public static string GetFileName(this string filePath, bool includeExtension = true)
        {
            if (filePath.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(filePath));
            var fileInfo = new FileInfo(filePath);
            return includeExtension ? fileInfo.Name : fileInfo.Name.TrimEnd(fileInfo.Extension);
        }

        /// <summary>
        /// 将字节数组保存到文件中
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="throwIfFileExist">当文件已存在时是否抛出异常,true-抛出异常,false-覆盖</param>
        /// <exception cref="ArgumentNullException">当bytes或filePath参数为空时引发异常</exception>
        /// <exception cref="InvalidOperationException">当文件已存在且throwIfFileExist为true时引发异常</exception>
        public static void SaveToFile(this byte[] bytes, string filePath, bool throwIfFileExist = false)
        {
            if (bytes.IsNullOrEmpty()) throw new ArgumentNullException(nameof(bytes));
            if (filePath.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(filePath));
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                if (throwIfFileExist) throw new InvalidOperationException($"file '{filePath}' already existed");
                fileInfo.Delete();
            }
            fileInfo.CreateFileIfNotExist();
            var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Dispose();
        }

        /// <summary>
        /// 将字节数组保存到文件中
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <param name="throwIfFileExist">当文件已存在时是否抛出异常,true-抛出异常,false-覆盖</param>
        /// <exception cref="ArgumentNullException">当bytes或filePath参数为空时引发异常</exception>
        /// <exception cref="InvalidOperationException">当文件已存在且throwIfFileExist为true时引发异常</exception>
        public static async Task SaveToFileAsync(this byte[] bytes, string filePath, CancellationToken? cancellationToken, bool throwIfFileExist = false)
        {
            if (bytes.IsNullOrEmpty()) throw new ArgumentNullException(nameof(bytes));
            if (filePath.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(filePath));
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                if (throwIfFileExist) throw new InvalidOperationException($"file '{filePath}' already existed");
                fileInfo.Delete();
            }
            fileInfo.CreateFileIfNotExist();
            var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken ?? CancellationToken.None);
            await stream.FlushAsync(cancellationToken ?? CancellationToken.None);
            stream.Dispose();
        }

        /// <summary>
        /// 将流保存到文件中
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="throwIfFileExist">当文件已存在时是否抛出异常,true-抛出异常,false-覆盖</param>
        /// <exception cref="ArgumentNullException">当stream或filePath参数为空时引发异常</exception>
        /// <exception cref="InvalidOperationException">当文件已存在且throwIfFileExist为true时引发异常</exception>
        public static void SaveToFile(this Stream stream, string filePath, bool throwIfFileExist = false)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));
            if (filePath.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(filePath));
            if (File.Exists(filePath))
            {
                if (throwIfFileExist) throw new InvalidOperationException($"file '{filePath}' already existed");
                File.Delete(filePath);
            }

            var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            stream.CopyTo(fileStream);
            fileStream.Flush();
            fileStream.Dispose();
        }

        /// <summary>
        /// 将流保存到文件中
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <param name="throwIfFileExist">当文件已存在时是否抛出异常,true-抛出异常,false-覆盖</param>
        /// <exception cref="ArgumentNullException">当stream或filePath参数为空时引发异常</exception>
        /// <exception cref="InvalidOperationException">当文件已存在且throwIfFileExist为true时引发异常</exception>
        public static async Task SaveToFileAsync(this Stream stream, string filePath, CancellationToken? cancellationToken, bool throwIfFileExist = false)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));
            if (filePath.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(filePath));
            if (File.Exists(filePath))
            {
                if (throwIfFileExist) throw new InvalidOperationException($"file '{filePath}' already existed");
                File.Delete(filePath);
            }

            var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            await stream.CopyToAsync(fileStream, cancellationToken ?? CancellationToken.None);
            await fileStream.FlushAsync(cancellationToken ?? CancellationToken.None);
            fileStream.Dispose();
        }

        /// <summary>
        /// 如果文件夹不存在抛出异常
        /// </summary>
        /// <param name="directory">文件夹路径</param>
        /// <exception cref="ArgumentNullException">当directory参数为空时引发异常</exception>
        /// <exception cref="Exception">当文件夹不存在时引发异常</exception>
        public static void ThrowIfDirectoryNotExist(this string directory)
        {
            if (directory.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(directory));
            new DirectoryInfo(directory).ThrowIfDirectoryNotExist();
        }

        /// <summary>
        /// 如果文件夹不存在抛出异常
        /// </summary>
        /// <param name="directory">文件夹路径</param>
        /// <exception cref="ArgumentNullException">当directory参数为空时引发异常</exception>
        /// <exception cref="Exception">当文件夹不存在时引发异常</exception>
        public static void ThrowIfDirectoryNotExist(this DirectoryInfo directory)
        {
            if (directory is null) throw new ArgumentNullException(nameof(directory));
            if (!directory.Exists) throw new Exception($"directory '{directory}' not exist");
        }

        /// <summary>
        /// 如果文件不存在抛出异常
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <exception cref="ArgumentNullException">当filePath参数为空时引发异常</exception>
        /// <exception cref="FileNotFoundException">当文件不存在时引发异常</exception>
        public static void ThrowIfFileNotExist(this string filePath)
        {
            if (filePath.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(filePath));
            new FileInfo(filePath).ThrowIfFileNotExist();
        }

        /// <summary>
        /// 如果文件不存在抛出异常
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <exception cref="ArgumentNullException">当fileInfo参数为空时引发异常</exception>
        /// <exception cref="FileNotFoundException">当文件不存在时引发异常</exception>
        public static void ThrowIfFileNotExist(this FileInfo fileInfo)
        {
            if (fileInfo is null) throw new ArgumentNullException(nameof(fileInfo));
            if (!fileInfo.Exists) throw new FileNotFoundException(null, fileInfo.FullName);
        }

        /// <summary>
        /// 如果文件夹不存在则创建
        /// </summary>
        /// <param name="directory">文件夹路径</param>
        /// <exception cref="ArgumentNullException">当directory参数为空时引发异常</exception>
        public static void CreateDirectoryIfNotExist(this string directory)
        {
            if (directory.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(directory));
            new DirectoryInfo(directory).CreateDirectoryIfNotExist();
        }

        /// <summary>
        /// 如果文件夹不存在则创建
        /// </summary>
        /// <param name="directory">文件夹路径</param>
        /// <exception cref="ArgumentNullException">当directory参数为空时引发异常</exception>
        public static void CreateDirectoryIfNotExist(this DirectoryInfo directory)
        {
            if (directory is null) throw new ArgumentNullException(nameof(directory));
            if (!directory.Exists) Directory.CreateDirectory(directory.FullName);
        }

        /// <summary>
        /// 如果文件不存在则创建
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <exception cref="ArgumentNullException">当filePath参数为空时引发异常</exception>
        public static void CreateFileIfNotExist(this string filePath)
        {
            if (filePath.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(filePath));
            new FileInfo(filePath).CreateFileIfNotExist();
        }

        /// <summary>
        /// 如果文件不存在则创建
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <exception cref="ArgumentNullException">当fileInfo参数为空时引发异常</exception>
        public static void CreateFileIfNotExist(this FileInfo fileInfo)
        {
            if (fileInfo is null) throw new ArgumentNullException(nameof(fileInfo));
            if (!fileInfo.Exists)
            {
                fileInfo.DirectoryName.CreateDirectoryIfNotExist();
                File.Create(fileInfo.FullName).Dispose();
            }
        }

        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="leftPath">左边路径</param>
        /// <param name="rightPath">右边路径</param>
        /// <returns>路径</returns>
        public static string CombinePath(this string leftPath, string rightPath) => Path.Combine(leftPath.Trim(), rightPath.Trim().TrimStart('/').TrimStart('\\')).FormatPath();

        /// <summary>
        /// 格式化路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>格式化字符串</returns>
        /// <exception cref="ArgumentNullException">当path参数为空时引发异常</exception>
        public static string FormatPath(this string path) => path?.Trim().Replace("\\", "/") ?? throw new ArgumentNullException(nameof(path));

        /// <summary>
        /// 如果文件存在则删除
        /// </summary>
        /// <param name="path">文件路径</param>
        public static void RemoveFileIfExist(this string path)
        {
            if (path.IsNullOrWhiteSpace()) return;
            if (File.Exists(path)) File.Delete(path);
        }

        /// <summary>
        /// 打开或创建文件流
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <returns>文件流</returns>
        public static FileStream OpenOrCreate(this FileInfo fileInfo)
        {
            fileInfo.CreateFileIfNotExist();
            return new FileStream(fileInfo.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        }

        /// <summary>
        /// 拷贝流
        /// </summary>
        /// <param name="source">原始流</param>
        /// <param name="target">目标流</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <param name="transfered">传输回调(本次传输字节数)</param>
        /// <returns>task</returns>
        public static async Task CopyToAsync(this Stream source, Stream target, CancellationToken cancellationToken, Action<long> transfered = null)
        {
            await Task.Yield();
            var buffer = new byte[2048];
            int length;
            if (source.CanSeek) source.Seek(0, SeekOrigin.Begin);

            while ((length = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (cancellationToken.IsCancellationRequested) break;
                target.Write(buffer, 0, length);

                transfered?.Invoke(length);
            }
        }
    }

}
