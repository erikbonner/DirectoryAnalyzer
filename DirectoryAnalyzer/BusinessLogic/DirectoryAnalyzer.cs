using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopAnalyzer.BusinessLogic
{
    /// <summary>
    /// Analyzes the subdirectories of a given directory and provides a ranked list
    /// of direcotires, based on their size
    /// </summary>
    public class DirectoryAnalyzer
    {
        /// <summary>
        /// Represents a directory
        /// </summary>
        public class DirectoryEntry
        {
            public enum SizeUnits
            {
                MB,
                KB,
                B
            }

            public string Name { get; set; }

            private double m_sizeInBytes;
            public double Size {
                get
                {
                    if (Units == SizeUnits.MB) return (float)m_sizeInBytes / (1024 * 1024);
                    else if (Units == SizeUnits.KB) return (float)m_sizeInBytes / (1024);
                    else return m_sizeInBytes;
                }
                set
                {
                    if (Units == SizeUnits.MB) m_sizeInBytes = value * 1024 * 1024; 
                    else if (Units == SizeUnits.KB) m_sizeInBytes = value * 1024; 
                    else m_sizeInBytes = value;
                }
            }
            public SizeUnits Units { get; set; }

            public DirectoryEntry()
            {
                Units = SizeUnits.B;
            }
        }

        public IList<DirectoryEntry> Analyze(string path, IProgress<int> progress, CancellationToken ct)
        {
            // create an unsorted list
            List<DirectoryEntry> dirs = new List<DirectoryEntry>();

            // walk through all subdirectories of our input path
            var subDirs = Directory.GetDirectories(path);
            double currIndex = 0;
            double numDirs = subDirs.Count();
            foreach (string d in subDirs)
            {
                var dirInfo = new DirectoryInfo(d);
                var size = DirSize(dirInfo, ct);

                var split = d.Split('\\');
                var baseName = split.Last();
                dirs.Add(new DirectoryEntry() { Name = baseName, Size = size });

                progress.Report( (int)((++currIndex / numDirs) * 100.0));
            }

            // sort list according to dir size
            dirs.Sort(
                (x, y) => {
                    if (x.Size < y.Size) return 1;
                    else if (x.Size == y.Size) return 0;
                    else return -1;
                });

            return dirs;
        }

        // method that computes size of a directory
        private long DirSize(DirectoryInfo d, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            long Size = 0;
            // Add file sizes.
            try
            {
                FileInfo[] fis = d.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    Size += fi.Length;
                }
                // Add subdirectory sizes.
                DirectoryInfo[] dis = d.GetDirectories();
                foreach (DirectoryInfo di in dis)
                {
                    Size += DirSize(di, ct);
                }
            }
            catch (UnauthorizedAccessException)
            {

            }
            catch (DirectoryNotFoundException)
            {

            }
            
            return (Size);

        }

    }
}
