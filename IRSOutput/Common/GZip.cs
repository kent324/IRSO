using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput {
    public class GZip {
        /// <summary>
        /// 把多个指定的文件压缩成一个文件
        /// </summary>
        /// <param name="files">文件列表</param>
        /// <param name="zipedFile">压缩后的文件</param>
        /// <returns></returns>
        public static bool ZipFiles(List<string> files, string zipedFile) {
            if (files.Count == 0)
                return false;
            try {
                using (FileStream zipToOpen = new FileStream(zipedFile, FileMode.CreateNew)) {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create)) {
                        foreach (var f in files) {
                            FileInfo fi = new FileInfo(f);
                            if (fi.Exists) {
                                ZipArchiveEntry readMeEntry = archive.CreateEntry(fi.Name);
                                using (Stream stream = readMeEntry.Open()) {
                                    byte[] bytes = File.ReadAllBytes(f);
                                    stream.Write(bytes, 0, bytes.Length);
                                }
                            }
                        }
                    }
                }
                foreach (string file in files) {
                    if (File.Exists(file)) {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            return true;
        }
        /// <summary>
        /// 将OrgDirectory目录下所有的文件压缩到zipedFile中
        /// </summary>
        /// <param name="OrgDirectory"></param>
        /// <param name="zipedFile"></param>
        /// <returns></returns>
        public static bool ZipDirectory(string OrgDirectory, string zipedFile) {
            try {
                ZipFile.CreateFromDirectory(OrgDirectory, zipedFile);
            }
            catch (Exception ex) {
                throw ex;
            }
            return true;
        }
    }
}
