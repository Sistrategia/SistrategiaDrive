using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistrategia.Drive.Business.CloudStorage.Amazon
{
    internal class AmazonCloudStorageProvider : ICloudStorageProvider
    {
        internal AmazonCloudStorageProvider() {

        }

        public CloudStorageContainer CreateCloudStorageContainer(CloudStorageAccount account, string alias, string description) {
            throw new NotImplementedException();
        }

        public CloudStorageContainer CreateCloudStorageContainer(Guid publicKey, string alias, string description) {
            throw new NotImplementedException();
        }

        public CloudStorageContainer CreateCloudStorageContainer(Guid publicKey, string alias, string containerName, string description) {
            throw new NotImplementedException();
        }

        public CloudStorageContainer CreateCloudStorageContainer(CloudStorageAccount account, Guid publicKey, string alias, string containerName, string description) {
            throw new NotImplementedException();
        }
    }
}
