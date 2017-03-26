using PickAndBook.Helpers.Contracts;
using System.IO;
using System.Web;

namespace PickAndBook.Helpers
{
    public class FileUploader : IFileUploader
    {
        private IPathProvider pathProvider;
        public FileUploader(IPathProvider pathProvider)
        {
            this.pathProvider = pathProvider;
        }

        public string UploadFile(HttpPostedFileBase upload, string pathToUpload)
        {
            string uploadedImage;
            try
            {
                string fileName = Path.GetFileName(upload.FileName);
                string path = Path.Combine(this.pathProvider.MapPath("~" + pathToUpload), fileName);
                upload.SaveAs(path);

                uploadedImage = Path.Combine(pathToUpload, fileName);
            }
            catch
            {
                uploadedImage = "";
            }
            return uploadedImage;
        }
    }
}