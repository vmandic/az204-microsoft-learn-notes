# Additional notes

Here I will write some stuff in my learnings that I did not know well.

## Azure Active Directory app manifest

- url: <https://docs.microsoft.com/en-us/azure/active-directory/develop/reference-app-manifest>
- The application manifest contains a definition of all the attributes of an application object in the Microsoft identity platform
  - To define permissions and roles that your app supports, you must modify the application manifest.
  - this is a JSON document specifying a schembe expected list of properties to configure your Azure AD application like of OAuth2 implicit flow is allowed with `oauth2AllowImplicitFlow: true` or `logoutUrl: "https://www.<mysite>.com/logout"`

## Logic Apps - Securing access with Integration Service Environment

An **Integration Service Environment** is a fully isolated and dedicated environment for all enterprise-scale integration needs, it is used especially for securing Logic Apps on own vNet. The ISE allows a secure integration of Azure virtual network connected services with Logic Apps

This is a deployment feature of Logic Apps where you choose ISE instead of a region on deployment. Having the Logic App deployed in ISE you can connect it to your vNet connected services.

## Azure AD CLI and role assignment

- use the `az ad group create --display-name mygroup --mail-nickname mygroupnick` to create an azure AD group
- use the `az role assignment create --assigne $azGroupObjectId --role "Some fancy role name" --scope $someAzureServiceId`

## Azure Notification Hubs how to

- one needs to createa notification hubs service, a hub on them and register the endservice Apple or Google on Azure, then the clients need to implement their side and the sender C# needs to use `NotificationHubClient.CreateClientFromConnectionString("<your hub's DefaultFullSharedAccessSignature>","<hub name>")` to create a hub instance through with device Registrations are obtained and messages are pushed
- url: <https://docs.microsoft.com/en-us/azure/notification-hubs/push-notifications-android-specific-users-firebase-cloud-messaging>

## Using azcopy tool to copy from local drive to storage account

- AzCopy is a command-line utility that you can use to copy blobs or files to or from a storage account
- for usage it should be stored in PATH variable as it is an executable
- authorizing via Azure AD is only for Blob container storage and once so you do not need SAS tokens, you will need SAS for destination storage account if you move from storage account to other storage account (target)
- example command: `azcopy copy "C:\local\path" "https://account.blob.core.windows.net/mycontainer1/?sv=2018-03-28&ss=bjqt&srt=sco&sp=rwddgcup&se=2019-05-01T05:01:17Z&st=2019-04-30T21:01:17Z&spr=https&sig=MGCXiyEzbtttkr3ewJIh2AR8KrghSy1DGM9ovN734bQF4%3D" --recursive=true`
- full reference: <https://docs.microsoft.com/en-us/azure/storage/common/storage-ref-azcopy-copy>
- example upload full directory: `azcopy cp "/path/to/dir" "https://[account].blob.core.windows.net/[container]/[path/to/directory]?[SAS]" --recursive`

## Organizing Event Hub partitions

- partitions should be based on incoming data to maximize throughput

## Azure Storage account soft delete feature

- when enabled soft delete enables recovering deleted blobs and blob snapshots and also if an overwrite of a blob occurs we can restore the previous version of a blob

## Customize KUDU deployment with .deployment file

- has to be in repository root in valid .ini format, eg. specify a custom depoyment command as:

```ini
[config]
command = deploy.cmd
```

or deploy a specific project

```ini
[config]
project = WebProject/WebProject.csproj
```

- more info on: <https://github.com/projectkudu/kudu/wiki/Customizing-deployments>

## Azure Database Migration Service

Azure Database Migration Service is a fully managed service designed to enable seamless migrations from multiple database sources to Azure data platforms with minimal downtime (online migrations).

## Manage azure storage account blob properties

There are two types of additional data you can attach to blobs: system metadaa (some are settable and some not) and user-defined properties (name-value pairs).

Depending on the API version .NET v12 or v11 you assign properties and then call SetPropertiesAsync() or SetHttpHeadersAsync(bloblHttpHeaders) function to apply them.

- more details: <https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-properties-metadata?tabs=dotnet>

## Cosmos DB consistency levels

CosmosDB offeres various levels of data consistency: Strong, Bunded staleness, Session, Consistent prefix and Eventual (from strongest to weakes consistency level).

- more info on: https://docs.microsoft.com/en-us/azure/cosmos-db/consistency-levels>
- **strong** ensures replication of write to all other geographic regions
- **bounded stalness** ensures a K (number of versions) or T (after some delay time) of maxium delay for which a read in other region may delay for original region write
  - "For a single region account, the minimum value of K and T is 10 write operations or 5 seconds. For multi-region accounts the minimum value of K and T is 100,000 write operations or 300 seconds."
  - "Bounded staleness is frequently chosen by globally distributed applications that expect low write latencies but require total global order guarantee."
  - same region reads occure the same as writes, but other regions are in K/T delay
- **session** enables creating a session group that spans over to other geographic region ensuring read consistency but other zones outside session are in delay, the oreder of writes is maintained for other session reads
- **consistent prefix** ensures the order of writes but all reads in delay even in same region
- **eventual consistency** does not guarantee read order anywhere, the consumer might get older data, ideal for like counts, retweet, non-threaded comments...

