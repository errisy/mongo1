Module Module1
    Sub Main()
        Dim baseFolder As New IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)

        While (baseFolder.GetFiles("*.vbproj").Length = 0)
            baseFolder = baseFolder.Parent
        End While

        Dim nodeProcess As New Process
        With nodeProcess.StartInfo
            .Arguments = """" + baseFolder.FullName + "\node.js" + """"
            .WorkingDirectory = baseFolder.FullName
            .UseShellExecute = False
            .RedirectStandardError = True
            .RedirectStandardOutput = True
            .RedirectStandardInput = False
            .FileName = "node"
        End With
        AddHandler nodeProcess.OutputDataReceived, AddressOf OutputReceived
        AddHandler nodeProcess.ErrorDataReceived, AddressOf ErrorReceived
        nodeProcess.EnableRaisingEvents = True

        Console.WriteLine("Begin:")
        nodeProcess.Start()
        nodeProcess.BeginOutputReadLine()
        nodeProcess.BeginErrorReadLine()

        nodeProcess.WaitForExit()

        If hasError Then
            Console.WriteLine("Error Occured: Press Any Key to Exit.")
            Console.ReadKey()
        Else
            Console.WriteLine("Executed Successfully. Press Any Key to Exit.")
            Console.ReadKey()
        End If
        'Dim address = AppDomain.CurrentDomain.BaseDirectory()
        'Dim extended = "bin\Debug\"
        'Dim vshost = "vshost.ext"
        'If address.EndsWith(extended) Then
        '    address = address.Substring(0, address.Length - extended.Length)
        '    Debug.WriteLine(address)
        '    'here we need to get all the ts files and call the tsc the project
        '    Console.WriteLine(AppDomain.CurrentDomain.FriendlyName)

        '    Dim AssemblyName = Reflection.Assembly.GetExecutingAssembly().FullName
        '    AssemblyName = AssemblyName.Substring(0, AssemblyName.IndexOf(","))
        '    Console.WriteLine(AssemblyName)

        '    Dim proj = address + AssemblyName + ".vbproj"

        '    Dim doc As Xml.XmlDocument = New Xml.XmlDocument()
        '    doc.Load(proj)
        '    Dim dotTS As New System.Text.RegularExpressions.Regex(".ts$", Text.RegularExpressions.RegexOptions.IgnoreCase)

        '    Dim programfiles = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
        '    Dim typescript = programfiles + "\Microsoft SDKs\TypeScript"
        '    Dim typescriptInfo As New IO.DirectoryInfo(typescript)
        '    Dim versionReg As New System.Text.RegularExpressions.Regex("(\d+)\.(\d+)")
        '    Dim maxVersion As String = Nothing
        '    Dim major As Integer = 0
        '    Dim minor As Integer = 0
        '    If typescriptInfo.Exists Then
        '        For Each dirInfo In typescriptInfo.GetDirectories
        '            For Each match As System.Text.RegularExpressions.Match In versionReg.Matches(dirInfo.Name)
        '                Dim ma As Integer = 0
        '                Dim mi As Integer = 0
        '                Integer.TryParse(match.Groups(1).Value, ma)
        '                Integer.TryParse(match.Groups(2).Value, mi)
        '                If ma > major Then
        '                    major = ma
        '                    minor = mi
        '                    maxVersion = dirInfo.Name
        '                ElseIf ma = major Then
        '                    If mi > minor Then
        '                        minor = mi
        '                        maxVersion = dirInfo.Name
        '                    End If
        '                End If
        '            Next
        '        Next
        '    End If
        '    If maxVersion IsNot Nothing Then
        '        Dim tsc = typescript + "\" + maxVersion + "\tsc.exe"
        '        Dim tsFiles As New List(Of String)

        '        For Each node In doc.Find("TypeScriptCompile[Include]")
        '            'Console.WriteLine(String.Format("{0}", node.GetAttribute("Include")))
        '            Dim tsFile = node.GetAttribute("Include")

        '            Console.WriteLine(tsFile)

        '            tsFiles.Add(tsFile)
        '        Next
        '        Dim files = tsFiles.Reduce(Function(item As String, sum As String) As String
        '                                       Return sum + """" + address + item + """ "
        '                                   End Function, "")


        '        Dim compilerInfo As New System.Diagnostics.ProcessStartInfo
        '        compilerInfo.FileName = tsc
        '        compilerInfo.WorkingDirectory = address
        '        compilerInfo.RedirectStandardOutput = True
        '        compilerInfo.UseShellExecute = False
        '        compilerInfo.Arguments = files
        '        Dim compiler = Process.Start(compilerInfo)
        '        While Not compiler.StandardOutput.EndOfStream
        '            Dim line = compiler.StandardOutput.ReadLine
        '            Console.WriteLine(line)
        '        End While
        '        compiler.WaitForExit()
        '        'this console system will invoke nodejs and save its process id

        '        Dim nodeInfo As New System.Diagnostics.ProcessStartInfo
        '        nodeInfo.FileName = "nodejs"
        '        nodeInfo.UseShellExecute = True



        '    End If
        'End If
        'Console.WriteLine("Press any key to exit...")
        'Console.ReadKey()
    End Sub
    Private hasError As Boolean = False
    Sub ErrorReceived(sender As Object, e As DataReceivedEventArgs)
        If e.Data IsNot Nothing Then
            Console.WriteLine(e.Data)
            hasError = True
        End If
    End Sub
    Sub OutputReceived(sender As Object, e As DataReceivedEventArgs)
        If e.Data IsNot Nothing Then Console.WriteLine(e.Data)
    End Sub
End Module

Module ArrayExtension
    <System.Runtime.CompilerServices.Extension>
    Public Iterator Function AsList(Of T)(array As IEnumerable) As IEnumerable(Of T)
        For Each item In array
            Yield item
        Next
    End Function
    <System.Runtime.CompilerServices.Extension>
    Public Function Reduce(Of T, TSum)(array As IEnumerable(Of T), method As Func(Of T, TSum, TSum), initial As TSum) As TSum
        Dim sum = initial
        For Each item In array
            sum = method(item, sum)
        Next
        Return sum
    End Function
End Module
Module JQuery
    <System.Runtime.CompilerServices.Extension()>
    Public Iterator Function Find(_xmlNode As Xml.XmlNode, value As String) As IEnumerable(Of Xml.XmlElement)
        Dim regAttr As New System.Text.RegularExpressions.Regex("\[(\w+)(|=""([^""]+)"")\]")
        Dim regNode As New System.Text.RegularExpressions.Regex("^(\w+)")
        Dim nodeType As String = Nothing
        Dim attrs As New Dictionary(Of String, String)

        Console.WriteLine("node:")
        For Each match As System.Text.RegularExpressions.Match In regNode.Matches(value)
            nodeType = match.Groups(1).Value
            Console.WriteLine(nodeType)
        Next
        Console.WriteLine("attrs")
        For Each match As System.Text.RegularExpressions.Match In regAttr.Matches(value)
            Console.WriteLine(match.Groups(3).Value.Length)
            attrs.Add(match.Groups(1).Value, match.Groups(3).Value)
        Next
        Dim searchAttributes = Function(keyvalue As KeyValuePair(Of String, String), attributes As Xml.XmlAttributeCollection) As Boolean
                                   Dim _Key = keyvalue.Key
                                   Dim _Value = keyvalue.Value
                                   If _Value.Length = 0 Then
                                       Return attributes.AsList(Of Xml.XmlAttribute).Any(Function(obj) obj.Name = _Key)
                                   Else
                                       Return attributes.AsList(Of Xml.XmlAttribute).Any(Function(obj) obj.Name = _Key And obj.Value = _Value)
                                   End If
                               End Function
        Dim searchNode As Func(Of Xml.XmlNode, IEnumerable(Of Xml.XmlNode))
        searchNode = Iterator Function(node As Xml.XmlNode) As IEnumerable(Of Xml.XmlNode)
                         Dim found As Boolean = True
                         If nodeType IsNot Nothing Then found = found AndAlso node.Name = nodeType
                         If attrs.Count > 0 Then
                             found = found AndAlso attrs.All(Function(keyvalue As KeyValuePair(Of String, String)) searchAttributes(keyvalue, node.Attributes))
                         End If
                         If found Then Yield node
                         For Each child As Xml.XmlNode In node.ChildNodes
                             For Each node In searchNode(child)
                                 Yield node
                             Next
                         Next
                     End Function
        For Each node In searchNode(_xmlNode)
            Yield node
        Next

    End Function
End Module