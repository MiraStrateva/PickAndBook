using System.Web;

namespace PickAndBook.Helpers.Contracts
{
    public interface IFileUploader
    {
        string UploadFile(HttpPostedFileBase upload, string pathToUpload);
    }
}
