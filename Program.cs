
namespace ConsoleApp1
{
    static class Program
    {
        static void Main()
        {
            string folder = "C:\\Users\\bossm\\YandexDisk\\@university\\Current Semester\\Information Techonology\\Laboratory Works\\ЛР 3 Марзан DONE\\project\\ConsoleApp1\\";
            var FileToZip = new FileInfo(folder + @"text.txt");
            Console.WriteLine("file to zip: ");
            ReadFile(FileToZip);
            var FileToSaveZippedForm = new FileInfo(folder + @"zip.txt");
            RLE_ZIP(FileToZip, FileToSaveZippedForm);
            Console.WriteLine("\n\n");
            Console.WriteLine("zipped file: ");
            ReadFile(FileToSaveZippedForm);
            FileToZip = FileToSaveZippedForm;
            FileToSaveZippedForm = new FileInfo(folder + @"unzip.txt");
            Unzip(FileToZip, FileToSaveZippedForm);

            Console.WriteLine("\n\n");
            Console.WriteLine("unzipped file: ");
            ReadFile(FileToSaveZippedForm);

        }

        static void ReadFile(FileInfo file)
        {
            var reader = file.OpenRead();
            int letter;
            while (true)
            {
                letter = reader.ReadByte();
                if (letter == -1) break;
                Console.Write(letter);
            }
        }

        static byte FindPrefix(FileInfo file)
        {
            var StreamReader = file.OpenRead();
            var count = new long[256];
            int FileLetterForCountingFrequency;
            while (true)
            {
                FileLetterForCountingFrequency = StreamReader.ReadByte();
                if (FileLetterForCountingFrequency == -1) break;
                count[FileLetterForCountingFrequency]++;
            }
            StreamReader.Close();
            if (count[127] == 0) return 127;
            long min = long.MaxValue;
            int IndexOfMinimum = 0;
            for (int i = 0; i < 256; i++)
            {
                if (count[i] == 0) return (byte)i;
                if (count[i] < min)
                {
                    min = count[i];
                    IndexOfMinimum = i;
                }
            }
            return (byte)IndexOfMinimum;
        }

        static void RLE_ZIP(FileInfo input, FileInfo output)
        {
            var StreamReader = input.OpenRead();
            var StreamWriter = output.Open(FileMode.Create);
            var Prefix = FindPrefix(input);
            StreamWriter.WriteByte(Prefix);
            int CurrentLetterToCount = StreamReader.ReadByte();
            int TempFileLetter;
            byte Frequency;
            while (CurrentLetterToCount != -1)
            {
                Frequency = 0;
                do
                {
                    TempFileLetter = StreamReader.ReadByte();
                    Frequency++;
                }
                while (TempFileLetter == CurrentLetterToCount && Frequency != 255);
                if (Frequency > 2 || CurrentLetterToCount == Prefix)
                {
                    StreamWriter.WriteByte(Prefix);
                    StreamWriter.WriteByte(Frequency);
                    StreamWriter.WriteByte((byte)CurrentLetterToCount);
                }
                else if (Frequency == 1) StreamWriter.WriteByte((byte)CurrentLetterToCount);
                else
                {
                    StreamWriter.WriteByte((byte)CurrentLetterToCount);
                    StreamWriter.WriteByte((byte)CurrentLetterToCount);
                }
                CurrentLetterToCount = TempFileLetter;
            }
            StreamWriter.Flush();
            StreamWriter.Dispose();
            StreamReader.Dispose();
        }

        static void Unzip(FileInfo ZipFileForm, FileInfo FileForUnzipForm)
        {
            var StreamReader = ZipFileForm.OpenRead();
            var StreamWriter = FileForUnzipForm.Open(FileMode.Create);
            var Prefix = StreamReader.ReadByte();
            int CurrentLetterToCount;
            int Frequency;
            while (true)
            {
                CurrentLetterToCount = StreamReader.ReadByte();
                if (CurrentLetterToCount == -1) break;
                if (CurrentLetterToCount == Prefix)
                {
                    Frequency = StreamReader.ReadByte();
                    CurrentLetterToCount = StreamReader.ReadByte();
                    StreamWriter.Write(Enumerable.Repeat((byte)CurrentLetterToCount, Frequency).ToArray());
                }
                else StreamWriter.WriteByte((byte)CurrentLetterToCount);
            }
            StreamWriter.Flush();
            StreamWriter.Dispose();
            StreamReader.Dispose();
        }
    }
}
