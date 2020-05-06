# AIMCS-an-artificial-intelligence-based-method-for-compression-of-short-strings
The basic source code of the AIMCS algorithm (Abedi Method) that we presented in our paper in SAMI 2020.

#General description

For improving the bandwidth utilization and reducing the costs, we proposed AIMCS which is a compression algorithm that is specially designed for the compression of short strings (with an average length of 160 characters). The AIMCS receives a short string as the input and uses an indexing approach. Then it creates a table in which each character is mapped to an index. Therefore, in the next repetition of characters, indexes are used instead of characters, which leads to reduction of size of data. 
As is mentioned in the description, here we discuss about the basic compression algorithm that we have presented in our paper. Therefore, α and β parameters must be set manually. As an example, we have prepared several tiny strings that are considered to be generated continuously by the sender, and sent one by one to the receiver.   
In the following, we show how the AIMCS code works for the compression of considered tiny strings. 
