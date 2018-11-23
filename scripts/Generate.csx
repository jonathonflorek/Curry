IEnumerable<Int32> GetAllLevels(Int32 numLevels)
    =>
    Enumerable.Range(1, numLevels);
String GetTypeParameters(IEnumerable<Int32> allLevels)
    =>
    "<" + String.Join(", ", allLevels.Select(x => $"T{x}").Append("TResult")) + ">";
IEnumerable<String> ToNamedArguments(IEnumerable<Int32> allLevels)
    =>
    allLevels.Select(x => $"arg{x}");

String AddCurry(Int32 numLevels)
{
    var allLevels = GetAllLevels(numLevels);
            
    var typeParameters = GetTypeParameters(allLevels);
    var namedArguments = ToNamedArguments(allLevels);

    return $@"
        public static Curry{typeParameters} Curry{typeParameters}(
            this Func{typeParameters} func)
            =>
            {String.Join(@" =>
            ", namedArguments)} =>
            func.Invoke(
                {String.Join(@",
                ", namedArguments)});";
}

String AddUncurry(Int32 numLevels)
{
    var allLevels = GetAllLevels(numLevels);

    var typeParameters = GetTypeParameters(allLevels);
    var namedArguments = ToNamedArguments(allLevels);

    return $@"
        public static Func{typeParameters} Uncurry{typeParameters}(
            this Curry{typeParameters} curry)
            =>
            ({String.Join(@"
                ,", namedArguments)})
            =>
            curry
            {String.Join(@"
            ", namedArguments.Select(x => $".Invoke({x})"))}
            ;";
}

String AddDelegate(Int32 numLevels)
{
    var prevLevels = GetAllLevels(numLevels - 1).Select(x => x + 1);
    var allLevels = GetAllLevels(numLevels);

    var prevTypeParams = GetTypeParameters(prevLevels);
    var typeParameters = GetTypeParameters(allLevels);

    return $@"
    public delegate Curry{prevTypeParams} Curry{typeParameters}(T1 arg);";
}

void Run() 
{
    var path = Args[0];
    var count = Int32.Parse(Args[1]);

    var directory = Directory.CreateDirectory(path);
    var dirPath = directory.FullName.TrimEnd('/') + "/";

    using (var curry = System.IO.File.OpenWrite(dirPath + "Curry.cs"))
    using (var writer = new StreamWriter(curry))
    {
        writer.Write($@"namespace Curry
{{
    using System;

    public delegate Func<T2, TResult> Curry<T1, T2, TResult>(T1 arg);
{String.Join(@"
", Enumerable.Range(3, count - 2).Select(AddDelegate))}
}}");
        
    }

    using (var currying = System.IO.File.OpenWrite(dirPath + "Currying.cs"))
    using (var writer = new StreamWriter(currying))
    {
        writer.Write($@"namespace Curry
{{
    using System;

    public static class Currying
    {{
{String.Join(@"
", Enumerable.Range(2, count - 1).Select(AddCurry))}

{String.Join(@"
", Enumerable.Range(2, count - 1).Select(AddUncurry))}
    }}
}}");
    }
}

Run();
