using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Tracing; // For Select

namespace Game
{
    class GameMain
    {
         private static int point=0;
         
         public static int getPoint(){return point;}
        private static int level=0;
        
        public static void Main(string[] args){ 

            IntroScreen.setState(1);
            IntroScreen.displayIntroText();

            Console.Clear();

            FirstSequence.flow();

            if(point>8) ReadScript("escape.txt"); else ReadScript("lose.txt");
        }  

        public static bool strcmp(string s1,string s2){
            return s1.Equals(s2, StringComparison.OrdinalIgnoreCase);
        }   

        public static void write(string str){
            for(int i=0;i<str.Length;i++)
            {
                Console.Write(str[i]);
                Thread.Sleep(20);
            }

            Console.WriteLine();
        }


        public static void ReadScript(string filePath)
        {
            string filePathShort = filePath;
            filePath = "scripts/" + filePath;
            try{
                level=int.Parse(filePathShort.Replace(".txt","").Substring(6));
            }catch(Exception e){Console.Write("caugh error: " + e);}
                
            
            if (File.Exists(filePath))
            {
                string currentText = "";
                string currentColorCode = "";
                int currentWait = 0;
                string currentPrompt = "";
                List<string> expectedInputs = new List<string>();
                bool waitingForInput = false;
                string? input = "";
                string? nextScript = null; // To store the name of the next script

                foreach (string line in File.ReadLines(filePath))
                {
                    if (line.StartsWith("COLOR:"))
                    {
                        currentColorCode = line.Substring("COLOR:".Length).Trim('"');
                    }
                    else if (line.StartsWith("TEXT:"))
                    {
                        currentText = line.Substring("TEXT:".Length).Trim('"').Replace("[YOUR_ALIAS]", FirstSequence.PlayerAlias);
                        string colorPrefix = string.IsNullOrEmpty(currentColorCode) ? "" : "\x1b[";
                        string colorSuffix = string.IsNullOrEmpty(currentColorCode) ? "" : "m";
                        write(colorPrefix + currentColorCode + colorSuffix + currentText + "\x1b[0m");
                        currentText = "";
                    }
                    else if (line.StartsWith("WAIT:"))
                    {
                        if (int.TryParse(line.Substring("WAIT:".Length), out currentWait))
                        {
                            Thread.Sleep(currentWait);
                            Console.Clear();
                        }
                    }
                    else if (line.StartsWith("PROMPT:"))
                    {
                        currentPrompt = line.Substring("PROMPT:".Length).Trim('"');
                        Console.WriteLine(currentPrompt);
                        waitingForInput = true;
                    }
                    else if (line.StartsWith("EXPECTED_INPUT:"))
                    {
                        expectedInputs.AddRange(line.Substring("EXPECTED_INPUT:".Length).Split(',').Select(s => s.Trim('"')));
                    }

                    while (waitingForInput)
                    {
                        input = Console.ReadLine();
                        if (input != null)
                        {
                            input = input.Trim();
                        }
                        else
                        {
                            input = string.Empty; // Handle null input gracefully
                        }
                        waitingForInput = false;
                        bool valid = false;
                        foreach (string expected in expectedInputs)
                        {
                            if (strcmp(input, expected))
                            {
                                valid = true;
                                nextScript="script" + (++level) + ".txt";
                                if(expectedInputs.IndexOf(input) == 1) point++;
                                break;
                            }
                        }

                        if (!valid)
                        {
                            Console.WriteLine(currentPrompt); // Re-display the prompt
                            waitingForInput = true;
                        }
                    }
                    
                }
                if (!string.IsNullOrEmpty(nextScript))
                {
                    ReadScript(nextScript);
                }
            }
            else
            {
                Console.WriteLine($"Error: Script file not found at {filePath}");
            }
        }
    }
}