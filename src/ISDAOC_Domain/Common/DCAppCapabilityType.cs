namespace Domain.Common
{
    public enum DCAppCapabilityType
    {
        Database_Create,
        Database_Update,
        Database_Delete,
        Database_Read,

        Notify_Email,
        Notify_SMS,

        App_DashboardEntry,
        App_Review, //Review and approval  - status

        Compute,

        Search_Entity,

        Generate_UniqueId,

        Upload_Image,
        Upload_Video,
        Upload_Document,
        Upload_File,

        Data_Export,
        Data_Import
    }
}