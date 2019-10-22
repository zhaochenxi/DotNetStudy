using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace FileService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IFileService
    {
        //上传文件
        //上传文件
        [OperationContract]
        UpFileResult UpLoadFile(UpFile filestream);

        //下载文件
        [OperationContract]
        DownFileResult DownLoadFile(DownFile downfile);

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: 在此添加您的服务操作
    }

    // 使用下面示例中说明的数据约定将复合类型添加到服务操作。
    // 可以将 XSD 文件添加到项目中。在生成项目后，可以通过命名空间“FileService.ContractType”直接使用其中定义的数据类型。
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
    [MessageContract]
    public class DownFile
    {
        [MessageHeader]
        public string FileName { get; set; }
    }

    [MessageContract]
    public class UpFileResult
    {
        [MessageHeader]
        public bool IsSuccess { get; set; }
        [MessageHeader]
        public string Message { get; set; }
    }

    [MessageContract]
    public class UpFile
    {
        [MessageHeader]
        public long FileSize { get; set; }
        [MessageHeader]
        public string FileName { get; set; }
        [MessageBodyMember]
        public Stream FileStream { get; set; }
    }

    [MessageContract]
    public class DownFileResult
    {
        [MessageHeader]
        public long FileSize { get; set; }
        [MessageHeader]
        public bool IsSuccess { get; set; }
        [MessageHeader]
        public string Message { get; set; }
        [MessageBodyMember]
        public Stream FileStream { get; set; }
    }
}
