# STDFLib <span style="margin-left: 20px"></span>![](https://cdn.travis-ci.com/images/favicon-076a22660830dc325cc8ed70e7146a59.png) ![TravisCI Build Status](https://travis-ci.com/stephensonb/stdflib.svg?token=6L8cHxYxjZVMDjs9zazy&branch=master&status=passed)

STDF Reader/Writer Library for STDF format files for ATE applications.

The current version reads and writes STDF files conforming to Version 4 of the STDF specification.

# Basic Usage

---

- Clone the repository and build the library (latest build using Visual Studio 19).
- Include the built library into your project and add a reference to it.
- Sample code to read a file as a list of records:

  ### _Note: The **STDFRecordFormatter** class implements the IDisposable interface. Use with 'using' to auto release managed resources when we go out of scope at the end of the enclosing block._

```csharp
public IEnumerable<ISTDFRecord> ReadFile(string path)
{
    List<ISTDFRecord> Records = new List<ISTDFRecord>();

    ISTDFRecord record;

    using Stream stream = File.OpenRead(path);

    // Create a new STDFRecordFormatter.  This class knows how to
    // read and write STDF formatted records.
    //
    using STDFRecordFormatter recordFormatter = new STDFRecordFormatter();

    // Include this line if the source file you are reading has a
    // different endiannes compared to the machine you  are running on.
    // If both source and target machine architectures match byte order
    // then this is not needed (the STDFRecordFormatter class defaults
    // to SwapBytes = false)
    recordFormatter.Converter.SwapBytes = true;

    // read in each record until the end of the stream is reached.
    do
    {
        record = (ISTDFRecord)recordFormatter.Deserialize(stream);
        try
        {
            Records.Add(record);
        }
        catch (Exception)
        {
        }
    } while (!recordFormatter.EndOfStream);

    return Records;
}
```

- Sample code to write out a list of records (note: no error checking is done to determine if the sequence of records is valid, however, the format of each record written is written to adhere to the STDF V4 specification).

```csharp
public void WriteFile(string path, IEnumerable<ISTDFRecord> records)
{
    using Stream stream = File.Open(path, FileMode.Create, FileAccess.Write);

    // Note: STDFRecordFormatter implements the IDisposable interface.
    //       Use with 'using' to auto release managed resources when we go
    //       out of scope at the end of the method.
    using STDFRecordFormatter recordFormatter = new STDFRecordFormatter();

    // Loop through the list of records and write out each one sequentially.
    // You can stream records by creating  the RecordFormatter and opening
    // the output stream at the start of testing and then serialize each
    // type of record as it becomes available.
    foreach (var record in records)
    {
        recordFormatter.Serialize(stream, record);
    }
}
```

# Requirements

- .NET version: .NET Core 3.1 (x64)

# Licensing

Copyright 2020, Brian Stephenson

Licensed under the MIT License, (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

https://opensource.org/licenses/MIT

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

A copy of the license is available in the repository's LICENSE file.
