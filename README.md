# AIMCS: An artificial intelligence based method for compression of short strings
The basic source code of the AIMCS algorithm (Abedi Method) that we presented in our paper in SAMI 2020.

## General description 

For improving the bandwidth utilization and reducing the costs, we proposed AIMCS which is a compression algorithm that is specially designed for the compression of short strings (with an average length of 160 characters). The AIMCS receives a short string as the input and uses an indexing approach. Then it creates a table in which each character is mapped to an index. Therefore, in the next repetition of characters, indexes are used instead of characters, which leads to reduction of size of data. 
As is mentioned in the description, here we discuss about the basic compression algorithm that we have presented in our paper. Therefore, `α` and `β` parameters must be set manually. As an example, we have prepared several tiny strings that are considered to be generated continuously by the sender, and sent one by one to the receiver. More information is available in [this article](https://ieeexplore.ieee.org/abstract/document/9108719). 

In the following, we show how the AIMCS code works for the compression of ASCIII and Unicode tiny strings. 

## Execution Requirements
We programmed the AIMCS algorithm by C# in Windows. It must be noticed that .NET framework 2 and Visual Studio are required for the code execution process. In addition, the AIMCSAv1.cs file must be added to the project file.

![How to add the file to your project in visual studio](https://user-images.githubusercontent.com/64810541/81226768-a82f9a80-8feb-11ea-8847-7b1c1f7eb81d.jpg)

Here we have provided two examples for compression of ASCIII tiny strings. In the first example (which could be considered as the schema of our method) we simply show how AIMCS works for compression of one tiny string.

### Example 1:

```cs
AIMCSAv1 AIMCSSend = new AIMCSAv1();
AIMCSAv1 AIMCSReceive = new AIMCSAv1();
byte[] t_byte = AIMCSSend.Compress("Mytext");
string decompressed = AIMCSReceive.Decompress(t_byte);
```


And in the second example we show how AIMCS works for compression of several different tiny strings. 

### Example 2:

The generated tiny strings can be found in the attached file “TinyStringExmples.text” and in the following we have provided explanations about different parts of the code. 

The following .NET libraries must be used:
```cs
using System;
using System.Text;
using System.IO;
using AIMCSA; 
```
The following function reads the tiny strings and stores them in the global Temp array. The readTextFile function definition is presented after the main function in this code. 
```cs
readTextFile("TinyStringExmples.txt");
```
This part of the code is used for printing the number of messages (tiny strings) and the size of the temp array which includes the contents of the “TinyStringExmples.text”.
```cs
Int32 originalSize = 0;
{
     int i = 0;
     for (; i < Temp.Length && Temp[i] != null; i++)
             originalSize += Temp[i].Length;
     Console.WriteLine("The number of strings:" + i.ToString());
     Console.WriteLine("\nThe size of original strings:" + '\t' + originalSize.ToString());
}
```
In the following part, the initial values of the variables are set and the compression class (called AIMCS) for both the sender and receiver is created independently. As is shown, the alpha and beta parameters are set manually.  
```cs
double sizeofNewMethod = 0;
int counterChar = 0;
int useResort = 0;
AIMCSAv1 AIMCSSend = new AIMCSAv1();
AIMCSAv1 AIMCSReceive = new AIMCSAv1();
AIMCSSend.beta = 70000;//beta
AIMCSSend.alpha = 0.045;//alpha
 ```
In the following for-loop, the tiny strings gets compressed in each repetition by the object of the AIMCS class. The first if-condition also checks to find out if the table needs to be re-ordered or not. If a re-orders was required, the second if-condition would re-order the table.  
 ```cs
for (int i = 0; i < Temp.Length && Temp[i] != null; i++)
{
   byte[] t_byte = AIMCSSend.Compress(Temp[i]);
   counterChar += Temp[i].Length;
   sizeofNewMethod += t_byte.Length;
   string decompressed = AIMCSReceive.Decompress(t_byte);
   if (counterChar > AIMCSSend.beta)
    {
       counterChar = 0;
       AIMCSSend.checkUseOfChar();
       if (AIMCSSend.needResort)
       {
            useResort++;
            AIMCSSend.needResort = false;
            AIMCSSend.Resort();
            byte[] newTable = AIMCSSend.makeTableforSending();
            sizeofNewMethod += newTable.Length;
            AIMCSReceive.Decompress(newTable);
       }
    }

}
 ```
 
The next part of the code calculates and prints the compression ratio. 
  ```cs
double percent = Math.Round((-1 * (((originalSize - sizeofNewMethod ) / originalSize) - 1)), 3);
Console.WriteLine("\nThe size of strings after getting compressed by AIMCS:" + '\t' + (sizeofNewMethod).ToString() +
                                 '\t' + percent.ToString() + "%" + '\t' + "Sort=" + useResort.ToString());         
Console.ReadKey();
 ```

And the following functions is used for reading the tiny strings that must be compressed.  
 ```cs
public static void readTextFile(String path)
{
   path = AppDomain.CurrentDomain.BaseDirectory + "\\" + path;
   if (!File.Exists(path))
   {
       using (FileStream fs = File.Create(path))
       {
           Byte[] info =
                 new UTF8Encoding(true).GetBytes("The File Should replace this file.");
               fs.Write(info, 0, info.Length);
       }
   }
       int index = 0;
       string s = "";
       using (StreamReader sr = File.OpenText(path))
           while ((s = sr.ReadLine()) != null)
              Temp[index++] = s;
}
 ``` 
 ### The Output of Example 2:
 ```cmd
The number of strings:10101
The size of original strings:   584630
The size of strings compression with new method:        453206  0.775%  Sort=6
 ```   

It is worth mentioning that this algorithm can be used for compression of any types of short strings on any application.

For compression of the Unicode tiny strings, in the abovementioned code, instead of AIMCSAv1, AIMCSUv1 class must be used. 



## Citation
If you find AIMCS useful please cite us in your work:
 ```
@inproceedings{Abedi_2020,
	doi = {10.1109/sami48414.2020.9108719},
	url = {https://doi.org/10.1109%2Fsami48414.2020.9108719},
	year = 2020,
	month = {jan},
	publisher = {{IEEE}},
	author = {Masoud Abedi and Mohammadreza Pourkiani},
	title = {{AIMCS}: An Artificial Intelligence based Method for Compression of Short Strings},
	booktitle = {2020 {IEEE} 18th World Symposium on Applied Machine Intelligence and Informatics ({SAMI})}
}
}
 ```  
## Authors

 - Abedi, Masoud 
 - Pourkiani, Mohammadreza 
 
 ## License
 
 This project is licensed under the GNU Affero General Public License v3.0  - see the [LICENSE](https://github.com/MasoudAbedi/AIMCS-an-artificial-intelligence-based-method-for-compression-of-short-strings/blob/master/LICENSE) file for details.

## Acknowledgment

We would like to thank Prof. Meike Klettke, the Graduate Academy Rostock of University and Prof. Ahmad Baraani-Dastjerdi for supporting this research. 

![uni-logo](https://user-images.githubusercontent.com/64810541/81230695-43c40980-8ff2-11ea-8b3b-8b9e89388047.png)

