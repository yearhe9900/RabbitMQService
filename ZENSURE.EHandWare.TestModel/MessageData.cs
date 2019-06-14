using System;

namespace ZENSURE.EHandWare.TestModel
{
    public class MessageData
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Context { get; set; }

        public bool IsHtml { get; set; }

        public int Status { get; set; }

        public string CreateMan { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
