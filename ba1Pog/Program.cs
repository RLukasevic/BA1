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

            Console.Write("Enter path to a file, which comments will be extracted into: ");
            string resPath = Console.ReadLine();

            string middlewarePath = folderPath + "/mid.txt";
            string[] filePaths = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
            foreach(string file in filePaths)
            {
                string comment = ExtractComments(file, middlewarePath);
                if(comment.Length > 0)
                {
                    string fileNamePref = "\n\n-----------------  " + file + "  -----------------\n\n";
                    File.AppendAllText(resPath, fileNamePref);
                    File.AppendAllText(resPath, comment);
                }
                Console.WriteLine("Completed writing of " + file);
            }


            Console.WriteLine("we did it!");
            Console.ReadLine();
        }
        static string ExtractComments(string filePath, string middleware)
        {
            string[] lines = File.ReadAllLines(filePath);
            File.WriteAllLines(middleware, lines);
            string allText = File.ReadAllText(middleware);
            File.Delete(middleware);
            //string allText = String.Join("\n", lines);
            //Console.WriteLine(allText);
            string comments = "";           
            allText = allText.Replace("\\\"", "");

                while (allText.Length > 0)
                {
                    int stringPos = allText.IndexOf("\"");
                    int endLinePos = allText.IndexOf("//");
                    int multiLinePos = allText.IndexOf("/*");

                    if ((stringPos < 0) &&
                        (endLinePos < 0) &&
                        (multiLinePos < 0)) break;

                    if (stringPos < 0) stringPos = allText.Length;
                    if (endLinePos < 0) endLinePos = allText.Length;
                    if (multiLinePos < 0) multiLinePos = allText.Length;

                    if ((stringPos < endLinePos) &&
                        (stringPos < multiLinePos))
                    {
                        int endPos = allText.IndexOf("\"", stringPos + 1);

                        if (endPos < 0)
                        {
                            allText = "";
                        }
                        else
                        {
                            allText = allText.Substring(endPos + 1);
                        }
                    }
                    else if (endLinePos < multiLinePos)
                    {
                        int endPos =
                            allText.IndexOf("\r\n", endLinePos + 2);

                        if (endPos < 0)
                        {
                            comments +=
                                allText.Substring(endLinePos) + "\r\n";
                            allText = "";
                        }
                        else
                        {
                            comments += allText.Substring(
                                endLinePos, endPos - endLinePos) + "\r\n";
                            allText = allText.Substring(endPos + 2);
                        }
                    }
                    else
                    {
                        int endPos = allText.IndexOf(
                            "*/", multiLinePos + 2);

                        if (endPos < 0)
                        {
                            comments +=
                                allText.Substring(multiLinePos) + "\r\n";
                            allText = "";
                        }
                        else
                        {
                            comments += allText.Substring(multiLinePos,
                                endPos - multiLinePos + 2) + "\r\n";
                            allText = allText.Substring(endPos + 2);
                        }
                    }
                }
                return comments;
        }
    }
}
