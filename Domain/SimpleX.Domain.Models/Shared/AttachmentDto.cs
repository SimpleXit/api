using SimpleX.Domain.Models.Base;

namespace SimpleX.Domain.Models.Shared
{
    public class AttachmentDto : Dto<AttachmentDto>
    {
        private long _id;
        private string _title;
        private string _description;
        private string _fileName;
        private byte[] _fileContent;

        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }
        public byte[] FileContent
        {
            get => _fileContent;
            set => SetProperty(ref _fileContent, value);
        }

        public override string ToString()
        {
            return $"{FileName}";
        }
    }

}
