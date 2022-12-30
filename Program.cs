using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Dictionary<string, float> variables = new Dictionary<string, float>();
        Dictionary<string, int> func = new Dictionary<string, int>();
        Dictionary<string, int> funcE = new Dictionary<string, int>();
        Dictionary<string, string> funca = new Dictionary<string, string>();
        variables["E"] = (float)Math.E;
        variables["pi"] = (float)Math.PI;
        variables["phi"] = (float)(1.0 + Math.Pow(5, 1 / 2)) / 2;
        int i = 0;
        string[] code = new string[1];
        while (true)
        {
            Console.Write("> ");
            Array.Resize(ref code, i + 1);
            code[i] = Console.ReadLine();
            if (code[i] == "RUN")
            {
                code[i] = "";
                break;
            }
            i++;
        }
        for (i = 0; i < code.Length; i++)
        {
            string[] words = code[i].Split(" ");
            if (words[0] == "var")
            {
                string varName = words[1];
                if (words[2] == "=")
                {
                    variables[varName] = float.Parse(words[3]);
                }
                if (words[2] == "+=")
                {
                    variables[varName] = variables[varName] + float.Parse(words[3]);
                }
                if (words[2] == "*=")
                {
                    variables[varName] = variables[varName] * float.Parse(words[3]);
                }
                if (words[2] == "/=")
                {
                    variables[varName] = variables[varName] * float.Parse(words[3]);
                }
                if (words[2] == "^=")
                {
                    variables[varName] = (float)Math.Pow(variables[varName], float.Parse(words[3]));
                }
            }
            else if (words[0] == "bool")
            {
                string varName = words[1];
                if (words[2] == "=")
                {
                    variables[varName] = float.Parse(IntEvalCond(words[3], words[4], words[5], variables).ToString());
                    //Console.WriteLine(variables[varName]);
                }
            }
            else if (words[0] == "print")
            {
                if (words[1].Substring(0, 1) == "$")
                {
                    string varPrint = words[1].Substring(1);
                    Console.WriteLine(variables[varPrint]);
                }
                else
                {
                    Console.Write("\n");
                    for (int j = 1; j < words.Length; j++)
                    {
                        Console.Write(words[j]);
                    }
                }
            }
            else if (words[0] == "func")
            {
                string funcName = words[1];
                string funcArgs = words[2];
                int k = i;
                int pre = i;
                while (code[k].Split(" ").Contains("EndF"))
                {
                    k++;
                }
                k++;
                i = k;
                func[funcName] = pre + 1;
                funcE[funcName] = i;
                funca[funcName] = funcArgs;
            }
            else if (words[0] == "call")
            {
                string funcName = words[1];
                string funcArgs = words[2];
                int startLine = func[funcName];
                int endLine = funcE[funcName];
                string[] funcCode = new string[endLine - startLine];
                Array.Copy(code, startLine, funcCode, 0, endLine - startLine);

                // parse the arguments string and store them in a dictionary
                Dictionary<string, float> args1 = new Dictionary<string, float>();
                string[] argList = funcArgs.Split(",");
                foreach (string arg in argList)
                {
                    string[] parts = arg.Split("=");
                    string argName = parts[0];
                    float argValue = float.Parse(parts[1]);
                    args1[argName] = argValue;
                }

                // pass the arguments dictionary to the InterpretCode method
                InterpretCode(funcCode, variables, args1, func, funcE, funca);
            }
            else if (words[0] == "if")
            {
                string name = words[1];
                if (!EvalCond(words[2], words[3], words[4], variables))
                {
                    int k = i;
                    while (code[k].Split(" ").Contains("EndIf" + name))
                    {
                        k++;
                    }
                    k++;
                    i = k;
                }
            }
            else if (words[0] == "goto")
            {
                i = int.Parse(words[1]);
            }
            else if (words[0].Split("")[0] == "#")
            {
                i++;
            }
        }
    }

    public static void InterpretCode(string[] code, Dictionary<string, float> variables, Dictionary<string, float> args1, Dictionary<string, int> func, Dictionary<string, int> funcE, Dictionary<string, string> funca)
    {
        foreach (string key in args1.Keys)
        {
            variables.Add(key, args1[key]);
        }
        Console.WriteLine(variables);
        variables["E"] = (float)Math.E;
        variables["pi"] = (float)Math.PI;
        variables["phi"] = (float)(1.0 + Math.Pow(5, 1 / 2)) / 2;
        int i = 0;
        while (true)
        {
            Console.Write("> ");
            Array.Resize(ref code, i + 1);
            code[i] = Console.ReadLine();
            if (code[i] == "RUN")
            {
                code[i] = "";
                break;
            }
            i++;
        }
        for (i = 0; i < code.Length; i++)
        {
            string[] words = code[i].Split(" ");
            if (words[0] == "var")
            {
                string varName = words[1];
                if (words[2] == "=")
                {
                    variables[varName] = float.Parse(words[3]);
                }
                if (words[2] == "+=")
                {
                    variables[varName] = variables[varName] + float.Parse(words[3]);
                }
                if (words[2] == "*=")
                {
                    variables[varName] = variables[varName] * float.Parse(words[3]);
                }
                if (words[2] == "/=")
                {
                    variables[varName] = variables[varName] * float.Parse(words[3]);
                }
                if (words[2] == "^=")
                {
                    variables[varName] = (float)Math.Pow(variables[varName], float.Parse(words[3]));
                }
            }
            else if (words[0] == "bool")
            {
                string varName = words[1];
                if (words[2] == "=")
                {
                    variables[varName] = float.Parse(IntEvalCond(words[3], words[4], words[5], variables).ToString());
                    //Console.WriteLine(variables[varName]);
                }
            }
            else if (words[0] == "print")
            {
                if (words[1].Substring(0, 1) == "$")
                {
                    string varPrint = words[1].Substring(1);
                    Console.WriteLine(variables[varPrint]);
                }
                else
                {
                    Console.Write("\n");
                    for (int j = 1; j < words.Length; j++)
                    {
                        Console.Write(words[j]);
                    }
                }
            }
            else if (words[0] == "func")
            {
                string funcName = words[1];
                string funcArgs = words[2];
                int k = i;
                int pre = i;
                while (code[k].Split(" ").Contains("EndF"))
                {
                    k++;
                }
                k++;
                i = k;
                func[funcName] = pre + 1;
                funcE[funcName] = i;
                funca[funcName] = funcArgs;
            }
            else if (words[0] == "call")
            {
                string funcName = words[1];
                string funcArgs = words[2];
                int startLine = func[funcName];
                int endLine = funcE[funcName];
                string[] funcCode = new string[endLine - startLine];
                Array.Copy(code, startLine, funcCode, 0, endLine - startLine);

                // parse the arguments string and store them in a dictionary
                Dictionary<string, float> args2 = new Dictionary<string, float>();
                string[] argList = funcArgs.Split(",");
                foreach (string arg in argList)
                {
                    string[] parts = arg.Split("=");
                    string argName = parts[0];
                    float argValue = float.Parse(parts[1]);
                    args2[argName] = argValue;
                }

                // pass the arguments dictionary to the InterpretCode method
                InterpretCode(funcCode, variables, args1, func, funcE, funca);
            }
            else if (words[0] == "if")
            {
                string name = words[1];
                if (!EvalCond(words[2], words[3], words[4], variables))
                {
                    int k = i;
                    while (code[k].Split(" ").Contains("EndIf" + name))
                    {
                        k++;
                    }
                    k++;
                    i = k;
                }
            }
            else if (words[0] == "goto")
            {
                i = int.Parse(words[1]);
            }
            else if (words[0].Split("")[0] == "#")
            {
                i++;
            }
        }
    }

    public static bool EvalCond(string a, string b, string c, Dictionary<string, float> variables)
    {
        float arg1;
        float arg2;
        if (a.Substring(0, 1) == "$")
        {
            string varName = a.Substring(1);
            arg1 = variables[varName];
        }
        else
        {
            try
            {
                arg1 = float.Parse(a);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input for argument 1. Please enter a valid number.");
                arg1 = float.Parse(Console.ReadLine());
            }
        }
        if (c.Substring(0, 1) == "$")
        {
            string varName = c.Substring(1);
            arg2 = variables[varName];
        }
        else
        {
            try
            {
                arg2 = float.Parse(c);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input for argument 2. Please enter a valid number.");
                arg2 = float.Parse(Console.ReadLine());
            }
        }
        if (b == "==")
        {
            return arg1 == arg2;
        }
        if (b == ">")
        {
            return arg1 > arg2;
        }
        if (b == "<")
        {
            return arg1 < arg2;
        }
        if (b == ">=")
        {
            return arg1 >= arg2;
        }
        if (b == "<=")
        {
            return arg1 <= arg2;
        }
        if (b == "!=")
        {
            return arg1 != arg2;
        }
        return false;
    }

    public static int IntEvalCond(string a, string b, string c, Dictionary<string, float> variables)
    {
        float arg1;
        float arg2;
        if (a.Substring(0, 1) == "$")
        {
            string varName = a.Substring(1);
            arg1 = variables[varName];
        }
        else
        {
            try
            {
                arg1 = float.Parse(a);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input for argument 1. Please enter a valid number.");
                arg1 = float.Parse(Console.ReadLine());
            }
        }
        if (c.Substring(0, 1) == "$")
        {
            string varName = c.Substring(1);
            arg2 = variables[varName];
        }
        else
        {
            try
            {
                arg2 = float.Parse(c);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input for argument 2. Please enter a valid number.");
                arg2 = float.Parse(Console.ReadLine());
            }
        }
        bool ans = false;
        if (b == "=")
        {
            ans = arg1 == arg2;
        }
        else if (b == "==")
        {
            ans = arg1 == arg2;
        }
        else if (b == ">")
        {
            ans = arg1 > arg2;
        }
        else if (b == "<")
        {
            ans = arg1 < arg2;
        }
        else if (b == ">=")
        {
            ans = arg1 >= arg2;
        }
        else if (b == "<=")
        {
            ans = arg1 <= arg2;
        }
        else if (b == "!=")
        {
            ans = arg1 != arg2;
        }

        int ansI;

        if (ans)
        {
            ansI = 1;
        }
        else
        {
            ansI = 0;
        }
        return ansI;
    }
}
