using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuffaloRenamer
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Select file operation
            Console.WriteLine("Please enter one of the following options...");
            Console.WriteLine("00 -> Copy only, no renaming");
            Console.WriteLine("01 -> Move Only, no renaming");
            Console.WriteLine("10 -> Copy and rename");
            Console.WriteLine("11 -> Move and rename");
            Console.Write("Operation Choice -> ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            string operationChoice = Console.ReadLine();
            Console.ResetColor();

            //Continue options for all renaming operations
            if (operationChoice == "10" || operationChoice == "11")
            {
                //Choose whether or not they want to rename by directory or by specific string
                Console.WriteLine();
                Console.WriteLine("Please enter one of the following options for what to use to rename...");
                Console.WriteLine("00 -> Directory name");
                Console.WriteLine("01 -> Custom string");
                Console.Write("Renaming scheme choice -> ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                string renameStringChoice = Console.ReadLine();
                Console.ResetColor();
                Console.WriteLine();

                //Choose whether or not to prepend or append selected string above
                Console.WriteLine("Please choose whether to prepend or to append this naming scheme to the original filename...");
                Console.WriteLine("00 -> Prepend");
                Console.WriteLine("01 -> Append");
                Console.Write("Refactoring scheme choice -> ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                string refactoringChoice = Console.ReadLine();
                Console.ResetColor();
                Console.WriteLine();

                if (operationChoice == "10")
                {
                    if (renameStringChoice == "00")
                    {
                        if (refactoringChoice == "00")
                        {
                            MoveFilesAndRename(true, true, false, "", true, false); // renames, prepends, based on directory name, copies only
                        }
                        else
                        {
                            MoveFilesAndRename(true, false, false, "", true, false);// renames, appends, based on directory name, copies only
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter the custom string to append or prepend to your filename below...");
                        Console.Write("Custom String -> ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        string customString = Console.ReadLine();
                        Console.ResetColor();
                        Console.WriteLine();
                        if (refactoringChoice == "00")
                        {
                            MoveFilesAndRename(true, true, false, customString, true, false);
                        }
                        else
                        {
                            MoveFilesAndRename(true, false, false, customString, true, false);
                        }
                    }
                }
                else
                {
                    if (renameStringChoice == "00")
                    {
                        if (refactoringChoice == "00")
                        {
                            MoveFilesAndRename(false, true, false, "", false, true);
                        }
                        else
                        {
                            MoveFilesAndRename(false, false, false, "", false, true);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter the custom string to append or prepend to your filename below...");
                        Console.Write("Custom String -> ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        string customString = Console.ReadLine();
                        Console.ResetColor();
                        Console.WriteLine();
                        if (refactoringChoice == "00")
                        {
                            MoveFilesAndRename(true, true, false, customString, false, true);
                        }
                        else
                        {
                            MoveFilesAndRename(true, false, false, customString, false, true);
                        }
                    }
                }


            }
            else if (operationChoice == "00" || operationChoice == "01")
            {
                if (operationChoice == "00")
                {
                    MoveFilesAndRename(false, false, false, "", true, false);
                }
                else
                {
                    MoveFilesAndRename(false, false, false, "", false, true);
                }
            }
            
            
            

            
            Console.WriteLine("done...");
            Console.ReadKey();
        }

        [STAThread]
        public static void MoveFilesAndRename(bool renameOption, bool prependSubDirName, bool customRename, string customRenameString, bool copy, bool move)
        {
            Console.WriteLine("Please Select Parent Directory...");
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            string sourcePath = fbd.SelectedPath;
            Console.WriteLine("Source Path:: " + sourcePath);
            Console.WriteLine();

            Console.WriteLine("Please select or create folder to move or copy files to...");
            FolderBrowserDialog fbdDispo = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbdDispo.ShowDialog();
            string dispoPath = fbdDispo.SelectedPath;
            Console.WriteLine("Disposition Path:: " + dispoPath);
            fbdDispo.Dispose();
            Console.WriteLine();
            bool appendSubDirName = false;
            if (prependSubDirName == false)
            {
                appendSubDirName = true;
            }
            
            if (Directory.GetDirectories(sourcePath).Length > 0)
            {
                if (Directory.GetFiles(sourcePath).Length > 0)
                {
                    string subDir = sourcePath;
                    string subDirName = Path.GetFileName(subDir);
                    foreach (var file in Directory.GetFiles(subDir, "*", SearchOption.TopDirectoryOnly))
                    {
                        string originalFileName = Path.GetFileName(file);
                        string fileLocation = Directory.GetParent(file).FullName;
                        string newFileName;
                        if (renameOption && prependSubDirName)
                        {
                            if (customRenameString != "")
                            {
                                newFileName = Path.Combine(dispoPath, customRenameString + "-" + originalFileName);
                                Console.WriteLine("PREPEND RENAME; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                                MoveOrCopyFunction(file, newFileName, copy, move);
                            }
                            else
                            {
                                newFileName = Path.Combine(dispoPath, subDirName + "-" + originalFileName);
                                Console.WriteLine("PREPEND RENAME; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                                MoveOrCopyFunction(file, newFileName, copy, move);
                            }
                        }
                        else if (renameOption && appendSubDirName)
                        {
                            if (customRenameString != "")
                            {
                                string extension = Path.GetExtension(originalFileName);
                                newFileName = Path.Combine(dispoPath, Path.GetFileNameWithoutExtension(originalFileName) + "-" + customRenameString + extension);
                                Console.WriteLine("APPEND RENAME; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                                MoveOrCopyFunction(file, newFileName, copy, move);
                            }
                            else
                            {
                                string extension = Path.GetExtension(originalFileName);
                                newFileName = Path.Combine(dispoPath, Path.GetFileNameWithoutExtension(originalFileName) + "-" + subDirName + extension);
                                Console.WriteLine("APPEND RENAME; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                                MoveOrCopyFunction(file, newFileName, copy, move);
                            }
                        }
                        else if (renameOption == false)
                        {
                            newFileName = Path.Combine(dispoPath, originalFileName);
                            Console.WriteLine("NO RENAME; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                            MoveOrCopyFunction(file, newFileName, copy, move);
                        }
                    }
                }
                foreach (var subDir in Directory.GetDirectories(sourcePath))
                {
                    Console.WriteLine(subDir);
                    if (Directory.GetFiles(subDir).Length > 0)
                    {
                        string subDirName = Path.GetFileName(subDir);


                        Console.WriteLine(subDirName);
                        foreach (var file in Directory.GetFiles(subDir, "*", SearchOption.AllDirectories))
                        {
                            string originalFileName = Path.GetFileName(file);
                            string fileLocation = Directory.GetParent(file).FullName;
                            string newFileName;
                            if (renameOption && prependSubDirName)
                            {
                                if (customRenameString != "")
                                {
                                    newFileName = Path.Combine(dispoPath, customRenameString + "-" + originalFileName);
                                    Console.WriteLine("PREPEND RENAME SUBDIR; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                                    MoveOrCopyFunction(file, newFileName, copy, move);
                                }
                                else
                                {
                                    newFileName = Path.Combine(dispoPath, subDirName + "-" + originalFileName);
                                    Console.WriteLine("PREPEND RENAMESUBDIR; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                                    MoveOrCopyFunction(file, newFileName, copy, move);
                                }
                            }
                            else if (renameOption && appendSubDirName)
                            {
                                if (customRenameString != "")
                                {
                                    string extension = Path.GetExtension(originalFileName);
                                    newFileName = Path.Combine(dispoPath, Path.GetFileNameWithoutExtension(originalFileName) + "-" + customRenameString + extension);
                                    Console.WriteLine("APPEND RENAMESUBDIR0; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                                    MoveOrCopyFunction(file, newFileName, copy, move);
                                }
                                else
                                {
                                    string extension = Path.GetExtension(originalFileName);
                                    newFileName = Path.Combine(dispoPath, Path.GetFileNameWithoutExtension(originalFileName) + "-" + subDirName + extension);
                                    Console.WriteLine("APPEND RENAMESUBDIR1; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                                    MoveOrCopyFunction(file, newFileName, copy, move);
                                }
                            }
                            else if (renameOption == false)
                            {
                                newFileName = Path.Combine(dispoPath, originalFileName);
                                Console.WriteLine("NO RENAME; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                                MoveOrCopyFunction(file, newFileName, copy, move);
                            }
                        }
                    }

                }
            }
            else
            {
                foreach (var file in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
                {
                    string originalFileName = Path.GetFileName(file);
                    string fileLocation = Directory.GetParent(file).FullName;
                    string subDirName = Path.GetFileName(Directory.GetParent(file).Name);
                    string newFileName;
                    if (renameOption && prependSubDirName)
                    {
                        if (customRenameString != "")
                        {
                            newFileName = Path.Combine(dispoPath, customRenameString + "-" + originalFileName);
                            Console.WriteLine("PREPEND RENAME; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                            MoveOrCopyFunction(file, newFileName, copy, move);
                        }
                        else
                        {
                            newFileName = Path.Combine(dispoPath, subDirName + "-" + originalFileName);
                            Console.WriteLine("PREPEND RENAME; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                            MoveOrCopyFunction(file, newFileName, copy, move);
                        }
                    }
                    else if (renameOption && appendSubDirName)
                    {
                        if (customRenameString != "")
                        {
                            string extension = Path.GetExtension(originalFileName);
                            newFileName = Path.Combine(dispoPath, Path.GetFileNameWithoutExtension(originalFileName) + "-" + subDirName + extension);
                            Console.WriteLine("APPEND RENAME; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                            MoveOrCopyFunction(file, newFileName, copy, move);
                        }
                        else
                        {
                            string extension = Path.GetExtension(originalFileName);
                            newFileName = Path.Combine(dispoPath, Path.GetFileNameWithoutExtension(originalFileName) + "-" + subDirName + extension);
                            Console.WriteLine("APPEND RENAME; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                            MoveOrCopyFunction(file, newFileName, copy, move);
                        }
                    }
                    else if (renameOption == false)
                    {
                        newFileName = Path.Combine(dispoPath, originalFileName);
                        Console.WriteLine("NO RENAME; Original File Name:: " + originalFileName + "; file location:: " + fileLocation + "; subDirName = " + subDirName + "; New path = " + newFileName + "; COPY = " + copy);
                        MoveOrCopyFunction(file, newFileName, copy, move);
                    }
                }
            }
        }

        public static void MoveOrCopyFunction(string file, string newFileName, bool copy = false, bool move = false)
        {
            try
            {
                if (copy)
                {
                    File.Copy(file, newFileName);
                }
                else if (move && copy == false)
                {
                    File.Move(file, newFileName);
                }

            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Could not find part of path...");
                Console.ResetColor();
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
