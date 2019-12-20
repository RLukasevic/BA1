using System;
using System.IO;

namespace ba1Pog
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter folder path: ");
            string folderPath = Console.ReadLine();
            string[] filePaths = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
            foreach(string file in filePaths)
            {
                string comment = ExtractComments(file);
                string fileNamePref = "\n\n-----------------  " + file + "  -----------------\n\n";
                File.AppendAllText("C:/users/robert/desktop/ba/222.txt", fileNamePref);
                File.AppendAllText("C:/users/robert/desktop/ba/222.txt", comment);
                Console.WriteLine("Completed writing of " + file);
            }


            Console.WriteLine("we did it!");
            Console.ReadLine();
        }
        static string ExtractComments(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            string middleware = "C:/users/robert/desktop/ba/555.txt";
            File.WriteAllLines(middleware, lines);
            string all_text = File.ReadAllText(middleware);
            string comments = "";           
            all_text = all_text.Replace("\\\"", "");

                while (all_text.Length > 0)
                {
                    int string_pos = all_text.IndexOf("\"");
                    int end_line_pos = all_text.IndexOf("//");
                    int multi_line_pos = all_text.IndexOf("/*");

                    if ((string_pos < 0) &&
                        (end_line_pos < 0) &&
                        (multi_line_pos < 0)) break;

                    if (string_pos < 0) string_pos = all_text.Length;
                    if (end_line_pos < 0) end_line_pos = all_text.Length;
                    if (multi_line_pos < 0) multi_line_pos = all_text.Length;

                    if ((string_pos < end_line_pos) &&
                        (string_pos < multi_line_pos))
                    {
                        int end_pos = all_text.IndexOf("\"", string_pos + 1);

                        if (end_pos < 0)
                        {
                            all_text = "";
                        }
                        else
                        {
                            all_text = all_text.Substring(end_pos + 1);
                        }
                    }
                    else if (end_line_pos < multi_line_pos)
                    {
                        int end_pos =
                            all_text.IndexOf("\r\n", end_line_pos + 2);

                        if (end_pos < 0)
                        {
                            comments +=
                                all_text.Substring(end_line_pos) + "\r\n";
                            all_text = "";
                        }
                        else
                        {
                            comments += all_text.Substring(
                                end_line_pos, end_pos - end_line_pos) + "\r\n";
                            all_text = all_text.Substring(end_pos + 2);
                        }
                    }
                    else
                    {
                        int end_pos = all_text.IndexOf(
                            "*/", multi_line_pos + 2);

                        if (end_pos < 0)
                        {
                            comments +=
                                all_text.Substring(multi_line_pos) + "\r\n";
                            all_text = "";
                        }
                        else
                        {
                            comments += all_text.Substring(multi_line_pos,
                                end_pos - multi_line_pos + 2) + "\r\n";
                            all_text = all_text.Substring(end_pos + 2);
                        }
                    }
                }
                return comments;
        }
    }
}
