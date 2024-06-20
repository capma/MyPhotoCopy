using MyPhotoCopy.ExifTool;
using Newtonsoft.Json;
using System.Diagnostics;

Process process = new Process();
process.StartInfo.FileName = "exiftool";
process.StartInfo.Arguments = "-json -sourcefile -make -model -createdate -filemodifydate -fileaccessdate -filecreatedate -r -fast2 -ext mov -ext avi -ext mp4 -ext webm -ext ogg -ext wmv  F:\\Photos";
process.StartInfo.UseShellExecute = false;
process.StartInfo.RedirectStandardOutput = true;
process.StartInfo.CreateNoWindow = true;
process.Start();

string jsonOutput = process.StandardOutput.ReadToEnd();
process.WaitForExit();

List<FileMetadata> metadataList = JsonConvert.DeserializeObject<List<FileMetadata>>(jsonOutput);
System.Console.WriteLine("this is the end!");