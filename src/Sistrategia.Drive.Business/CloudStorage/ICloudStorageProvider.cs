using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistrategia.Drive.Business.CloudStorage
{
    public interface ICloudStorageProvider
    {
        CloudStorageContainer CreateCloudStorageContainer(CloudStorageAccount account, string alias, string description);
        CloudStorageContainer CreateCloudStorageContainer(Guid publicKey, string alias, string description);
        CloudStorageContainer CreateCloudStorageContainer(Guid publicKey, string alias, string containerName, string description);
        CloudStorageContainer CreateCloudStorageContainer(CloudStorageAccount account, Guid publicKey, string alias, string containerName, string description);        
    }
}
